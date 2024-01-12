using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace PocketHeroes
{
    public class AvatarAnimator : MonoBehaviour
    {
        [Required]
        public Animator Animator { get; private set; }

        public Action OnPerform;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void Perform()
        {
            OnPerform?.Invoke();
        }
    }
}
