using UnityEngine;
using PocketHeroes.Input;

namespace PocketHeroes.Characters
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] bool debug;

        [SerializeField] float moveSpeed = 5f;
        Animator animator;

        CharacterController characterController;

        Vector3 inputDirection;
        Vector3 gravity;
        float magnitude;

        // 25:10 https://www.youtube.com/watch?v=bXNFxQpp2qk&t=9s
        int isWalkingHash;
        int isRunningHash;

        #region LifeCycle
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = transform.Find("Avatar").GetComponent<Animator>();

            isWalkingHash = Animator.StringToHash("isWalking");
            isRunningHash = Animator.StringToHash("isRunning");
        }
        private void OnEnable()
        {
            InputReader.Instance.MoveEvent += OnMove;
        }

        private void OnDisable()
        {
            InputReader.Instance.MoveEvent -= OnMove;
        }

        private void Update()
        {
            HandleRotation();
            HandleMovement();
            HandleGravity();

            HandleAnimations();
        }
        #endregion
        void HandleRotation()
        {
            if (magnitude <= 0) { return; }

            float rotationSpeed = 25f;

            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void HandleMovement()
        {
            characterController.Move((inputDirection * moveSpeed + gravity) * Time.deltaTime);
        }
        
        void HandleGravity()
        {
            if (characterController.isGrounded)
            {
                gravity.y = -0.05f;
            } else
            {
                gravity.y = -9.8f;
            }
        }

        void HandleAnimations()
        {

            bool isWalking = animator.GetBool(isWalkingHash);
            bool isRunning = animator.GetBool(isRunningHash);

            switch (magnitude)
            {
                case 0f:
                    if (isWalking) { animator.SetBool(isWalkingHash, false); }
                    if (isRunning) { animator.SetBool(isRunningHash, false); }
                    break;
                case <= 0.5f:
                    if (!isWalking) { animator.SetBool(isWalkingHash, true); }
                    if (isRunning) { animator.SetBool(isRunningHash, false); }
                    break;
                case > 0.5f:
                    if (isWalking) { animator.SetBool(isWalkingHash, false); }
                    if (!isRunning) { animator.SetBool(isRunningHash, true); }
                    break;
            }
        }

        void OnMove(Vector2 inputVector)
        {
            float cameraAngle = Camera.main.transform.rotation.y - 45f;

            magnitude = inputVector.magnitude;

            inputDirection = Quaternion.Euler(0, cameraAngle, 0) * new Vector3(inputVector.x, 0, inputVector.y);
        }
    }
}