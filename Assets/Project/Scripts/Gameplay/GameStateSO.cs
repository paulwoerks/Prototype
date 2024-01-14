using UnityEngine;

namespace PocketHeroes
{
    public enum GameState
    {
        Gameplay,
        Pause, // Pause menu is opened, whole game world is frozen.
        Inventory,
    }

    /// <summary>
    /// Is the Current GAMEstate
    /// </summary>
    [CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState")]
    public class GameStateSO : BaseSO
    {
        public GameState CurrentGameState => currentGameState;

        [SerializeField] GameState currentGameState = default;
        [SerializeField] GameState previousGameState = default;

        public void UpdateGameState(GameState newGameState)
        {
            if (newGameState == CurrentGameState) { return; }

            previousGameState = currentGameState;
            currentGameState = newGameState;
        }

        public void ResetToPreviousGameState()
        {
            if (previousGameState == currentGameState) { return; }

            GameState stateToReturnTo = previousGameState;
            previousGameState = currentGameState;
            currentGameState = stateToReturnTo;
        }
    }
}
