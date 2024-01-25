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
    public abstract class Enemy : MonoBehaviour, ISpawnable, IDamagable
    {
        #region Fields
        [Header("Components")]
        [SerializeField] Health health;
        [SerializeField] Animator animator;

        [Header("References")]
        [SerializeField] TransformGroupSO group;
        [SerializeField] TransformAnchor heroAnchor;

        [SerializeField] SpecialEffectSO deathEffect;
        [SerializeField] GameObject damagePopup;

        public Health Health => health;
        public Animator Animator => animator;
        public TransformGroupSO Group => group;
        public Transform Hero => heroAnchor.Value;

        public SpecialEffectSO DeathEffect => deathEffect;

        int takeDamageHash;
        int isDeadHash;
        #endregion

        #region LifeCycle
        public virtual void Awake()
        {
            takeDamageHash = Animator.StringToHash("TakeDamage");
            isDeadHash = Animator.StringToHash("IsDead");
        }
        public virtual void OnSpawn()
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

        public virtual void InflictDamage(int damage)
        {
            if (health.IsDead)
                return;

            health.InflictDamage(damage);

            DamagePopup popup = Pooler.Spawn(damagePopup, transform.position, Quaternion.identity).GetComponent<DamagePopup>();
            popup.SetDamage(damage);

            if (damage <= 0)
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

        public virtual void Despawn()
        {
            deathEffect?.Play(transform.position);
            Pooler.Despawn(gameObject);
        }
    }
}
