using System;
using UnityEngine;

namespace PocketHeroes.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;

        public bool IsDead => currentHealth <= 0;

        public Action OnReceiveDamage;
        public Action OnDie;
        
        public void SetMaxHealth(int newAmount)
        {
            maxHealth = newAmount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

        public void SetCurrentHealth(int newAmount)
        {
            currentHealth = newAmount;
        }

        public void InflictDamage(int amount)
        {
            OnReceiveDamage?.Invoke();
            currentHealth -= amount;
            if (IsDead)
            {
                currentHealth = 0;
                OnDie?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            currentHealth += amount;

            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
    }
}
