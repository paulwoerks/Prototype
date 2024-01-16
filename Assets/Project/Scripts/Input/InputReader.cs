using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PocketHeroes.Input
{
    // Reference:
    // Unity Chop Chop InputReader: https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Input/InputReader.cs

    /// <summary>
    /// Reads Input from the new Input System
    /// </summary>
    public class InputReader : MonoBehaviour, GameInput.IGameplayActions
    {
        [SerializeField] bool debug;

        [Header("References")]
        [Required][SerializeField] GameStateSO gameStateManager;

        [Header("Broadcasting on")]
        [SerializeField] Vector2EventChannelSO MoveEvent;

        GameInput gameInput;

        #region LifeCyle
        void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
                gameInput.Gameplay.SetCallbacks(this);
            }
            EnableGameplayInput();
        }

        void OnDisable()
        {
            DisableAllInput();
        }
        #endregion

        #region Public
        /// <summary>
        /// Is called by new Inputsystem.
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveDirection = context.ReadValue<Vector2>();
            MoveEvent?.Invoke(moveDirection);

            this.Log($"Move: {context.ReadValue<Vector2>()}", debug);
        }
        #endregion

        #region Private
        void EnableGameplayInput()
        {
            gameInput.Gameplay.Enable();
            // gameInput.Menus.Disable();
            // gameInput.Dialogues.Disable();
            this.Log("Gameplay Enabled", debug);
        }

        void DisableAllInput()
        {
            gameInput.Gameplay.Disable();
            // gameInput.Menus.Disable();
            // gameInput.Dialogues.Disable();
            this.Log("All disabled", debug);
        }
        #endregion
    }
}
