using UnityEngine;
using TMPro;
using DG.Tweening;

namespace PocketHeroes.Effects
{
    // https://www.youtube.com/watch?v=iD1_JczQcFY
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] TextMeshPro textMesh;
        [SerializeField] float animationDuration = 0.6f;
        [SerializeField] float startScale = 2f;
        float height;

        public void SetDamage(int damage, float height = 2f)
        {
            this.height = height;
            Reset();
            textMesh.SetText(damage.ToString());
            PerformAnimation();
        }

        void Reset()
        {
            Transform textTransform = textMesh.transform;
            textTransform.localScale = Vector3.one * startScale;

            float offset = 0.3f;
            textTransform.localPosition = Vector3.up * height;
            textTransform.localPosition += Vector3.right * Random.Range(-offset, offset);
        }

        void PerformAnimation()
        {
            Transform textTransform = textMesh.transform;

            textTransform.DOScale(0.01f, animationDuration);
            textTransform.DOMoveY(height + 0.5f, animationDuration);
        }
    }
}
