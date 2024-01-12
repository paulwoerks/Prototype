using PocketHeroes.Core;
using UnityEngine;

namespace PocketHeroes
{
    public class Dummy : MonoBehaviour, IDamagable
    {
        [SerializeField] TransformGroup enemyTransforms;

        [SerializeField] Animator animator;

        void Awake()
        {
            enemyTransforms.Add(transform);
        }
        public void TakeDamage(int damage)
        {
            animator.SetTrigger("GetHurt");
        }
    }
}
