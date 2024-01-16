using PocketHeroes.Anchors;
using PocketHeroes.Combat;
using PocketHeroes.Pooling;
using PocketHeroes.Effects;
using UnityEngine;

namespace PocketHeroes.Characters
{
    /// <summary>
    /// Base Class for enemies. Handles Damage and Death, Including animations. Also responsible for the enemy group subscription
    /// </summary>
    public abstract class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields
        [Header("Components")]
        [SerializeField] Health health;
        [SerializeField] Animator animator;

        [Header("References")]
        [SerializeField] TransformGroupSO group;
        [SerializeField] TransformAnchor heroAnchor;

        [SerializeField] SpecialEffectSO deathEffect;

        public Health Health => health;
        public Animator Animator => animator;
        public TransformGroupSO Group => group;
        public Transform Hero => heroAnchor.Value;

        int takeDamageHash;
        int isDeadHash;
        #endregion

        #region LifeCycle
        public virtual void Awake()
        {
            takeDamageHash = Animator.StringToHash("TakeDamage");
            isDeadHash = Animator.StringToHash("IsDead");
        }
        public virtual void OnEnable()
        {
            Animator.SetBool(isDeadHash, false);
            group.Add(transform);
            health.DieEvent += OnDie;

            health.SetCurrentHealth(health.MaxHealth);
        }

        public virtual void OnDisable()
        {
            group.Remove(transform);
            health.DieEvent -= OnDie;
        }
        #endregion

        public virtual void TakeDamage(int amount)
        {
            if (health.IsDead)
                return;

            health.InflictDamage(amount);

            if (amount <= 0)
                return;

            Animator.SetTrigger(takeDamageHash);
        }

        public virtual void OnDie()
        {
            Animator.SetBool(isDeadHash, true);
            group.Remove(transform);

            float deathTimer = 2f;
            Invoke(nameof(Despawn), deathTimer);
        }

        void Despawn()
        {
            deathEffect?.Play(transform.position);
            Pooler.Despawn(gameObject);
        }
    }
}
