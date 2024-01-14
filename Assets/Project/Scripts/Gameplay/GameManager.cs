using Sirenix.OdinInspector;
using UnityEngine;

namespace PocketHeroes
{
    public class GameManager : MonoBehaviour
    {

        [Required][SerializeField] GameStateSO gameState;

        void Start()
        {
            StartGame();
        }

        void StartGame()
        {
            gameState.UpdateGameState(GameState.Gameplay);
        }
    }
}
