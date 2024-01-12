using UnityEngine;
using PocketHeroes.Input;

namespace PocketHeroes.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5f;

        [SerializeField] JoystickReader joystick;

        [SerializeField] Animator avatarAnimator;

        private void Start()
        {
            joystick.Initialize();
        }

        void Update()
        {
            HandleSpeed();
            HandleDirection();
        }

        void HandleSpeed()
        {
            float desiredSpeed = joystick.GetDistance();
            float currentSpeed = maxSpeed * desiredSpeed;
            transform.position += transform.forward * currentSpeed * Time.deltaTime;

            avatarAnimator.SetFloat("MoveSpeed", desiredSpeed);
        }

        void HandleDirection()
        {
            float offset = Camera.main.transform.rotation.y  -45f;
            Vector3 joystickDirection = joystick.GetDirection(offset);

            if (joystickDirection == Vector3.zero) { return; }

            float rotationSpeed = 5f;
            Quaternion desiredRotation = Quaternion.LookRotation(joystickDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

}