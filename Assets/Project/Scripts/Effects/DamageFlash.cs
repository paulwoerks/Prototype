using UnityEngine;
using PocketHeroes.Combat;

namespace PocketHeroes.Effects
{
    /// <summary>
    /// A simple flashing up of a material when health component gets hit.
    /// </summary>
    public class DamageFlash : MonoBehaviour
    {
        Color color = Color.white;
        float intensity = 5f;
        float duration = 0.1f;

        [Header("Components")]
        [SerializeField] Health health;
        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
        Material material;

        private void Awake()
        {
            material = skinnedMeshRenderer.material;
        }

        void OnEnable()
        {
            health.ReceiveDamageEvent += Blink;
        }

        void OnDisable()
        {
            health.ReceiveDamageEvent -= Blink;
            CancelInvoke();
        }

        void Blink()
        {
            material.color = color * intensity;
            Invoke(nameof(EndBlink), duration);
        }

        void EndBlink()
        {
            material.color = color * 1f;
        }
    }
}
