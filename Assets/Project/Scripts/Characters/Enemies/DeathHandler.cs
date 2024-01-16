using PocketHeroes.Combat;
using PocketHeroes.Pooling;
using UnityEngine;

namespace PocketHeroes.Characters
{
    public class DeathHandler : MonoBehaviour
    {
        [SerializeField] float deathTimer = 2f;
        [Header("Components")]
        [Tooltip("Required to access DieEvent")]
        [SerializeField] Health health;

        private void OnEnable()
        {
            health.DieEvent += OnDie;
        }


        private void OnDisable()
        {
            health.DieEvent -= OnDie;
        }

        void OnDie()
        {
            if (deathTimer > 0f)
                Invoke(nameof(Despawn), deathTimer);
        }

        void Despawn()
        {
            Pooler.Despawn(gameObject);
        }
    }
}
