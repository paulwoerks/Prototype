using UnityEngine;
using PocketHeroes.Effects;

namespace PocketHeroes.Characters
{
    public class EarthWorm : Enemy
    {
        enum State { Idle, Hidden, Attacking, TakeDamage, Dead }

        #region Fields
        [Header("Stats")]
        [SerializeField] float rotationSpeed = 5f;

        [Header("Behaviour")]
        [SerializeField] State state;
        // WORM BEHAVIOUR LOOP
        // Make the Worm show for 4 Seconds
        // Shoot a bullet
        // Wait for 1 seconds
        // Then hide underground
        // Wait for 5 Seconds
        // Repeat

        [SerializeField] SpecialEffectSO spawnEffect;

        int isHiddenHash;
        int attackProjectileHash;
        #endregion

        #region LifeCycle
        public override void Awake()
        {
            isHiddenHash = Animator.StringToHash("IsHidden");
            attackProjectileHash = Animator.StringToHash("AttackProjectile");
            base.Awake();
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
            Appear();
        }

        public override void OnDisable()
        {
            CancelInvoke();
            base.OnDisable();
        }

        private void Update()
        {
            if (!Health.IsDead)
            {
                RotateTowardsPlayer();
            }
        }
        #endregion

        #region Public
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Health.IsDead)
                return;

            state = State.TakeDamage;
            CancelInvoke(); // Interrupt current behaviour
            Hide();
        }

        public override void OnDie()
        {
            CancelInvoke();
            state = State.Dead;
            base.OnDie();
        }
        #endregion

        #region Behaviour
        void Appear()
        {
            Vector3 spawnPosition = GetRandomPosition();

            transform.position = spawnPosition;

            Animator.SetBool(isHiddenHash, false);;
            state = State.Idle;

            Group.Add(transform);

            spawnEffect.Play(spawnPosition);

            Invoke(nameof(Attack), 3f);
        }

        void Attack()
        {
            Animator.SetTrigger(attackProjectileHash);
            state = State.Attacking;

            Invoke(nameof(Hide), 2f);
        }

        void Hide()
        {
            Group.Remove(transform);
            Animator.SetBool(isHiddenHash, true);
            state = State.Hidden;
            spawnEffect.Play(transform.position);

            Invoke(nameof(Appear), 3f);
        }
        #endregion

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

        Vector3 GetRandomPosition()
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            return randomPosition;
        }
    }
}
