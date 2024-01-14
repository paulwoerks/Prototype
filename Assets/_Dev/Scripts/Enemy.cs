using PocketHeroes.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes.Characters
{
    public abstract class Enemy : MonoBehaviour, IDamagable
    {
        [Header("Components & References")]
        [SerializeField] Animator animator;

        [SerializeField] TransformGroupSO enemies;

        int takeDamageHash;

        public virtual void Awake()
        {
            enemies.Add(transform);
            takeDamageHash = Animator.StringToHash("TakeDamage");
        }
        public virtual void TakeDamage(int damage)
        {
            animator.SetTrigger(takeDamageHash);
        }
    }
}
