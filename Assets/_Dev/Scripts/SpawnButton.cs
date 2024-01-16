using UnityEngine;
using PocketHeroes.Pooling;

namespace PocketHeroes
{
    public class SpawnButton : MonoBehaviour
    {
        [SerializeField] EnemySpawner spawner;
        [SerializeField] GameObject enemyType;
        
        public void SpawnEnemy()
        {
            spawner.Spawn(enemyType);
        }
    }
}
