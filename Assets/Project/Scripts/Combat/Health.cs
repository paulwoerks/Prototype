using UnityEngine;
using UnityEngine.Events;

namespace PocketHeroes.Combat
{
    /// <summary>
    /// Managing health and invoking events on damage and death
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;

        public bool IsDead => currentHealth <= 0;

        public UnityAction ReceiveDamageEvent = delegate { };
        public UnityAction DieEvent = delegate { };
        
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
            ReceiveDamageEvent.Invoke();
            currentHealth -= amount;
            if (IsDead)
            {
                currentHealth = 0;
                DieEvent.Invoke();
            }
        }

        public void Heal(int amount)
        {
            currentHealth += amount;

            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
    }
}
