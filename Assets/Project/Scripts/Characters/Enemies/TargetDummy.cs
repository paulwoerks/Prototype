using PocketHeroes.Pooling;
using UnityEngine;

namespace PocketHeroes.Characters
{
    public class TargetDummy : Enemy
    {
        enum State { Idle, Running, Attacking, TakeDamage, Dead }

        #region Fields
        [Header("Stats")]
        [SerializeField] float moveSpeed = 0.5f;
        [SerializeField] float rotationSpeed = 2.5f;

        [Header("Behaviour")]
        [SerializeField] State state;

        int isMovingHash;

        bool isMoving => Animator.GetBool(isMovingHash);
        #endregion

        #region LifeCycle
        public override void Awake()
        {
            isMovingHash = Animator.StringToHash("IsMoving");
            base.Awake();
        }
        private void Update()
        {
            if (!Health.IsDead)
            {
                RotateTowardsPlayer();
                Move();
            } 
            else if (isMoving)
            {
                Animator.SetBool(isMovingHash, false);
            }
        }
        #endregion

        private void Move()
        {
            if (!isMoving)
            {
                Animator.SetBool(isMovingHash, true);
            }

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        void RotateTowardsPlayer()
        {
            bool isActive = state.Equals(State.Idle) || state.Equals(State.Attacking);
            if (!isActive)
                return;

            Vector3 targetDirection = Hero.position - transform.position;
            targetDirection.y = transform.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public override void Despawn()
        {
            Vector3 deathEffectPosition = transform.position + (transform.forward * -1f);
            DeathEffect?.Play(deathEffectPosition);
            Pooler.Despawn(gameObject);
        }
    }
}
