using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PocketHeroes.Pooling
{
    ///<summary>
    ///Pools Objects dynamically
    ///</summary>
    public class Pooler : Singleton<Pooler>
    {
        #region Fields
        [Tooltip("Pools that preload on Awake.")]
        [SerializeField] Pool[] preloadedPools = new Pool[] { };
        Dictionary<string, Pool> pools = new();

        [Tooltip("Parent of Active Instances. Created automatically, if not set manually.")]
        [SerializeField] Transform activeInstances;
        [Tooltip("Parent of Inactive Instances. Created automatically, if not set manually.")]
        [SerializeField] Transform inactiveInstances;
        #endregion

        #region LifeCycle
        public override void Awake()
        {
            base.Awake();
            CreateHierarchyStructure();
            PreloadPoolObjects();
        }
        #endregion

        #region Public
        ///<summary>
        /// Spawn Instance of a Prefab. (Optional) Set the parent.
        ///</summary>
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Instance.SpawnPoolObject(prefab, position, rotation, parent);
        }

        ///<summary>
        ///Despawn existing Instance of Prefab.
        ///</summary>
        public static void Despawn(GameObject instance, float delay = 0f)
        {
            Instance.DespawnPooledObject(instance, delay);
        }
        #endregion

        #region Private
        GameObject SpawnPoolObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Transform pooledObject = GetPool(prefab).GetFromPool().transform;

            parent ??= activeInstances;

            pooledObject.SetPositionAndRotation(position, rotation);
            pooledObject.SetParent(parent, true);

            if (pooledObject.TryGetComponent<ISpawnable>(out ISpawnable ISpawn))
            {
                ISpawn.OnSpawn();
            }

            this.Log($"[+] '{prefab.name}'", debug);

            return pooledObject.gameObject;
        }

        async void DespawnPooledObject(GameObject instance, float delay = 0f)
        {
            if (delay > 0f)
            {
                await Task.Delay((int)(delay * 1000));
            }

            if (pools.TryGetValue(instance.name, out Pool pool))
            {
                if (instance.TryGetComponent<IDespawnable>(out IDespawnable IDespawn))
                    IDespawn.OnDespawn();

                pool.AddToPool(instance);
                instance.transform.SetParent(inactiveInstances);
                this.Log($"[-] '{pool.Prefab.name}'", debug);
                return;
            }
            else
            {
                this.LogWarning($"Pool '{instance.name.Replace("(Clone)", "")}' not found.", debug);
            }
        }

        void CreateHierarchyStructure()
        {
            if (Instance.inactiveInstances == null)
            {
                Instance.inactiveInstances = new GameObject("[-]Cache").transform;
                Instance.inactiveInstances.SetParent(transform);
            }
            Instance.inactiveInstances.gameObject.SetActive(false);

            if (Instance.activeInstances == null)
            {
                Instance.activeInstances = new GameObject("[+]Active").transform;
                Instance.activeInstances.SetParent(transform);
            }
            Instance.activeInstances.gameObject.SetActive(true);
        }

        void PreloadPoolObjects()
        {
            if (preloadedPools.Length == 0) { return; }

            foreach (Pool pool in preloadedPools)
            {
                pool.Preload();
                pools.Add($"{pool.Prefab.name}(Clone)", pool);
            }

            this.Log(preloadedPools.Length + " Pools preloaded", debug);
        }

        Pool GetPool(GameObject prefabOfPool)
        {
            string key = $"{prefabOfPool.name}(Clone)";

            return pools.TryGetValue(key, out Pool existingPool) ? existingPool : pools[key] = new Pool(prefabOfPool);
        }
        #endregion
    }
}