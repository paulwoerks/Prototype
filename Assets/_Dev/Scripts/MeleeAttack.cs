using PocketHeroes.Core;
using System.Collections;
using UnityEngine;

namespace PocketHeroes
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] string triggerName;
        [SerializeField] TransformGroup enemies;

        [SerializeField] AvatarAnimator avatarAnimator;

        [SerializeField] int damage = 1;
        [SerializeField] float attackRange = 5f;
        [SerializeField] float cooldown = 1f;

        bool canAttack = true;

        Transform target;

        private void Start()
        {
            StartCoroutine(CheckForTargets());
        }

        void Perform()
        {
            Debug.Log("Perform");
            avatarAnimator.Animator.SetTrigger(triggerName);
            avatarAnimator.OnPerform += InflictDamage;
        }

        void InflictDamage()
        {
            Debug.Log("InflictDamage");
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
