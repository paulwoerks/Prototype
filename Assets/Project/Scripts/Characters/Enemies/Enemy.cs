using PocketHeroes.Anchors;
using PocketHeroes.Combat;
using UnityEngine;

namespace PocketHeroes.Characters
{
    public abstract class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields
        [Header("Components")]
        [SerializeField] Health health;
        [SerializeField] Animator animator;

        [Header("References")]
        [SerializeField] TransformGroupSO group;
        [SerializeField] TransformAnchor heroAnchor;

        public Health Health => health;
        public Animator Animator => animator;
        public TransformGroupSO Group => group;
        public Transform Hero => heroAnchor.Value;

        #endregion

        #region LifeCycle
        public virtual void OnEnable()
        {
            Animator.SetBool("isDead", false);
            group.Add(transform);
            health.OnDie += Die;
        }

        public virtual void OnDisable()
        {
            group.Remove(transform);
            health.OnDie -= Die;
        }
        #endregion

        public virtual void TakeDamage(int amount)
        {
            if (health.IsDead)
                return;

            health.InflictDamage(amount);

            if (amount <= 0)
                return;

            Animator.SetTrigger("TakeDamage");
        }


        public virtual void Die()
        {
            Animator.SetBool("isDead", true);
            group.Remove(transform);
        }
    }
}
