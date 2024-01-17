using System.Collections;
using UnityEngine;
using PocketHeroes.Anchors;
using PocketHeroes.Audio;
using PocketHeroes.Characters;

namespace PocketHeroes.Combat
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float attackRange = 5f;
        [SerializeField] float cooldown = 1f;

        [SerializeField] string animationName = "Attack_Sword";
        [SerializeField] Animator animator;
        [SerializeField] AnimationEventHandler animationEventHandler;

        [SerializeField] PoolableAudio attackSound;

        [SerializeField] TransformGroupSO enemies;

        bool canAttack = true;

        Transform target;

        private void Start()
        {
            StartCoroutine(CheckForTargets());
        }

        void Perform()
        {
            attackSound.Play();
            animator.SetTrigger(animationName);
            animationEventHandler.Subscribe(animationName, InflictDamage);
        }

        void InflictDamage()
        {
            target.GetComponent<IDamagable>().TakeDamage(damage);
            StartCoroutine(Cooldown());
        }

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldown);
            canAttack = true;
            StartCoroutine(CheckForTargets());
        }

        IEnumerator CheckForTargets()
        {
            while (canAttack)
            {
                target = enemies.GetClosest(transform.position, attackRange);

                if (target)
                {
                    Perform();
                    canAttack = false;
                } 
                else
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }
}
