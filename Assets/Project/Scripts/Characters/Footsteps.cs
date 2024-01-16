using UnityEngine;
using PocketHeroes.Audio;

namespace PocketHeroes.Characters
{
    public class Footsteps : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] LayerMask groundLayer;

        [Header("Sounds")]
        [SerializeField] PoolableAudio defaultStep;

        /// <summary>
        /// Gets called by animation event
        /// </summary>
        void TakeStep()
        {
            // PSEUDOCODE
            // Raycast to the Ground
            // Check for Ground Material Name by materialname.contains or check for Tag, Objectname
            // return Step according to check
            // Play footstep for material
            defaultStep.Play();
        }
    }
}
