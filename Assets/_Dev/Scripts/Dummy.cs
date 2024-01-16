using UnityEngine;

namespace PocketHeroes.Characters
{
    public class Dummy : Enemy
    {

        enum State { Idle, Running, Attacking, TakeDamage, Dead }

        [Header("Stats")]
        [SerializeField] float moveSpeed = 0.5f;
        [SerializeField] float rotationSpeed = 2.5f;

        [Header("Behaviour")]
        [SerializeField] State state;

        bool isMoving => Animator.GetBool("isMoving");
        private void Update()
        {
            if (!Health.IsDead)
            {
                FacePlayer();
                Move();
            } 
            else if (isMoving)
            {
                Animator.SetBool("isMoving", false);
            }
        }

        private void Move()
        {
            if (!isMoving)
            {
                Animator.SetBool("isMoving", true);
            }

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

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
    }
}
