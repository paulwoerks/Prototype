using System.Collections;
using UnityEngine;
using PocketHeroes.Anchors;
using PocketHeroes.Characters;
using PocketHeroes.Effects;

namespace PocketHeroes.Combat
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] Vector2 damage = new Vector2(1, 3);
        [SerializeField] float attackRange = 5f;
        [SerializeField] float cooldown = 1f;

        [SerializeField] string animationName = "Attack_Sword";
        [SerializeField] Animator animator;
        [SerializeField] AnimationEventHandler animationEventHandler;

        [SerializeField] SpecialEffectSO swordSlashFX;

        [SerializeField] TransformGroupSO enemies;

        bool canAttack = true;

        Transform target;

        private void Start()
        {
            StartCoroutine(CheckForTargets());
        }

        void StartAttack()
        {
            animator.SetTrigger(animationName);
            animationEventHandler.Subscribe(animationName, InflictDamage);
        }

        void InflictDamage()
        {
            swordSlashFX.Play(transform.position, transform.rotation, transform);
            target.GetComponent<IDamagable>().InflictDamage(Random.Range((int)damage.x, (int)damage.y) + 1);
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
                    StartAttack();
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
