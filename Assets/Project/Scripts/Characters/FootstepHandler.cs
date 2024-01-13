using UnityEngine;
using PocketHeroes.Audio;

namespace PocketHeroes.Player
{
    public class FootstepHandler : MonoBehaviour
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
            PoolableAudio clip = GetMaterialSound();

            clip.Play();
        }

        /// <summary>
        /// Detects ground surface by name and returns sound
        /// </summary>
        /// <returns></returns>
        PoolableAudio GetMaterialSound()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);

            if (Physics.Raycast(ray, out hit, 1.0f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                string materialName = hit.collider.gameObject.name;

                if (materialName.Contains("Stone"))
                {
                    return defaultStep;
                } 
                else if (materialName.Contains("Grass")) 
                {
                    return defaultStep;
                }
                else if (materialName.Contains("Wood"))
                {
                    return defaultStep;
                }
            }

            return defaultStep;
        }


    }
}
