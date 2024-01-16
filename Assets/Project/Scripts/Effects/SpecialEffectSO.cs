using UnityEngine;
using PocketHeroes.Audio;
using PocketHeroes.Pooling;

namespace PocketHeroes.Effects
{
    [CreateAssetMenu(fileName ="SpecialEffect", menuName = "Effects/Special Effect SO")]
    public class SpecialEffectSO : BaseSO
    {
        [SerializeField] GameObject visualFX;
        [SerializeField] PoolableAudio soundFX;

        /// <summary>
        /// Spawns a visual and a sound effect
        /// </summary>
        public void Play(Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
            if (rotation.Equals(default))
            {
                rotation = visualFX.transform.rotation;
            }

            soundFX.SetParent(Pooler.Spawn(visualFX, position, rotation, parent).transform);

            soundFX.Play();
        }
    }
}
