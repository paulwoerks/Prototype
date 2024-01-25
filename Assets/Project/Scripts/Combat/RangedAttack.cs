using UnityEngine;
using PocketHeroes.Pooling;

namespace PocketHeores.Combat
{
    public class RangedAttack : MonoBehaviour
    {
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform muzzle;
        public void Perform()
        {
            GameObject projectile = Pooler.Spawn(projectilePrefab, muzzle.position, transform.rotation);
        }
    }
}
