using System.Collections;
using UnityEngine;
using PocketHeroes.Core;
using PocketHeroes.Audio;

namespace PocketHeroes
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float attackRange = 5f;
        [SerializeField] float cooldown = 1f;

        [SerializeField] string triggerName;
        [SerializeField] AvatarAnimator avatarAnimator;

        [SerializeField] PoolableAudio attackSound;

        [SerializeField] TransformGroup enemies;

        bool canAttack = true;

        Transform target;

        private void Start()
        {
            StartCoroutine(CheckForTargets());
        }

        void Perform()
        {
            attackSound.Play();
            avatarAnimator.SetTrigger(triggerName);
            avatarAnimator.OnPerform += InflictDamage;
        }

        void InflictDamage()
        {
            target.GetComponent<IDamagable>().TakeDamage(damage);
            avatarAnimator.OnPerform -= InflictDamage;
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
