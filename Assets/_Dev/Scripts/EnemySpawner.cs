using UnityEngine;

namespace PocketHeroes.Pooling
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Vector2 mapSize = new Vector2(10, 10);
        [SerializeField] Transform parent;
        public void Spawn(GameObject prefab, float instances = 1)
        {
            for (int i = 0; i < instances; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-mapSize.x, mapSize.x), 0, Random.Range(-mapSize.y, mapSize.y));
                Pooler.Spawn(prefab, spawnPosition, Quaternion.identity, parent);
            }
        }
    }
}
