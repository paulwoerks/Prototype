using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace PocketHeroes
{
    public class AvatarAnimator : MonoBehaviour
    {
        [Required]
        [SerializeField] Animator animator;

        public Action OnPerform;

        public void SetTrigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        public void Perform()
        {
            OnPerform?.Invoke();
        }
    }
}
