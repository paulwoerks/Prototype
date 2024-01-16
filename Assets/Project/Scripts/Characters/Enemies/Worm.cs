using UnityEngine;

namespace PocketHeroes.Characters
{
    public class Worm : Enemy
    {
        enum State { Idle, Hidden, Attacking, TakeDamage, Dead }

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

        #region LifeCycle
        public override void OnEnable()
        {
            Appear();
            base.OnEnable();
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
                FacePlayer();
            }
        }
        #endregion

        void FacePlayer()
        {
            bool isActive = state.Equals(State.Idle) || state.Equals(State.Attacking);
            if (!isActive)
                return;

            Vector3 targetDirection = Hero.position - transform.position;
            targetDirection.y = transform.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        void Appear()
        {
            transform.position = GetRandomPosition();

            Animator.SetBool("isHidden", false);;
            state = State.Idle;

            Group.Add(transform);

            Invoke("Attack", 3f);
        }

        void Attack()
        {
            Animator.SetTrigger("AttackProjectile");
            state = State.Attacking;

            Invoke("Hide", 2f);
        }

        void Hide()
        {
            Group.Remove(transform);
            Animator.SetBool("isHidden", true);
            state = State.Hidden;

            Invoke("Appear", 3f);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Health.IsDead)
                return;

            state = State.TakeDamage;
            CancelInvoke(); // Interrupt current behaviour
            Hide();
        }

        public override void Die()
        {
            CancelInvoke();
            state = State.Dead;
            base.Die();
        }

        Vector3 GetRandomPosition()
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            return randomPosition;
        }
    }
}
