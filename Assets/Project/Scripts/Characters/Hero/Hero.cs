using UnityEngine;
using PocketHeroes.Input;
using PocketHeroes.Anchors;
using PocketHeroes.Combat;

namespace PocketHeroes.Characters
{
    public class Hero : MonoBehaviour, IDamagable
    {
        [Header("Stats")]
        [SerializeField] bool debug;
        [SerializeField] float moveSpeed = 5f;

        [Header("Components")]
        [SerializeField] Health health;
        Animator animator;
        CharacterController characterController;

        [Header("References")]
        [SerializeField] TransformAnchor heroAnchor;

        [Header("Listening to")]
        [SerializeField] Vector2EventChannelSO OnMoveEvent;

        Vector3 inputDirection;
        Vector3 gravity;
        float magnitude;

        // 25:10 https://www.youtube.com/watch?v=bXNFxQpp2qk&t=9s
        int isWalkingHash;
        int isRunningHash;
        int isDeadHash;

        #region LifeCycle
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = transform.Find("Avatar").GetComponent<Animator>();

            isWalkingHash = Animator.StringToHash("IsWalking");
            isRunningHash = Animator.StringToHash("IsRunning");
            isDeadHash = Animator.StringToHash("IsDead");
        }
        private void OnEnable()
        {
            heroAnchor.Provide(transform);
            OnMoveEvent.Subscribe(OnMove, this);
        }

        private void OnDisable()
        {
            heroAnchor.Unset();
            OnMoveEvent.Unsubscribe(OnMove, this);
        }

        private void Update()
        {
            RotateToInput();
            MoveForward();
            HandleGravity();

            AnimateMovement();
        }
        #endregion

        #region Public
        public void TakeDamage(int amount)
        {
            if (health.IsDead)
                return;

            health.InflictDamage(amount);

            animator.SetTrigger(isDeadHash);
        }
        #endregion

        #region Movement
        void OnMove(Vector2 inputVector)
        {
            this.Log($"{inputVector}, {inputVector.magnitude}", debug);

            float cameraAngle = Camera.main.transform.rotation.y - 45f;

            magnitude = inputVector.magnitude;

            inputDirection = Quaternion.Euler(0, cameraAngle, 0) * new Vector3(inputVector.x, 0, inputVector.y);
        }

        void RotateToInput()
        {
            if (magnitude <= 0) { return; }

            float rotationSpeed = 25f;

            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void MoveForward()
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
        void AnimateMovement()
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
        #endregion
    }
}