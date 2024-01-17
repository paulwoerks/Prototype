using UnityEngine;

namespace PocketHeroes.Pooling
{
    /// <summary>
    /// Feeds pooled object back into pooler after fixed amount of time
    /// </summary>
    public class DespawnTimer : MonoBehaviour
    {
        [SerializeField] bool debug;
        [SerializeField] float timer = 1f;

        private void OnEnable()
        {
            if (timer <= 0f)
            {
                this.LogWarning($"Value invalid. Timer set to '{timer}s'");
            }
            this.Log($"{gameObject.name}: {timer}s", debug);
            Invoke(nameof(Despawn), timer);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        void Despawn()
        {
            Pooler.Despawn(gameObject);
            this.Log($"Despawn {gameObject.name}", debug);
        }
    }
}
