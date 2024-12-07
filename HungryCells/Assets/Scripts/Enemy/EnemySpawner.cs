using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        
        public float minDelay = 10;
        public float maxDelay = 15;

        [SerializeField] private GameObject[] enemyPrefabs;
        public Player.Player player;
        public Collider2D[] spawnAreas;
        private bool stopSpawning = false;
        private void Start()
        {
            StartCoroutine(SpawnEnemyAfterDelay(7));
        }

        private void SpawnEnemy()
        {
            var spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
            var minSpawnPos = spawnArea.bounds.min;
            var maxSpawnPos = spawnArea.bounds.max;
            var spawnPos = new Vector2(Random.Range(minSpawnPos.x, maxSpawnPos.x), Random.Range(minSpawnPos.y, maxSpawnPos.y));
            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            var newEnemy = Instantiate(enemyPrefabs[randomEnemy], this.transform);
            
            
            newEnemy.transform.position = spawnPos;
            newEnemy.GetComponent<Enemy>().target = player.gameObject;

            StartCoroutine(SpawnEnemyAfterDelay(Random.Range(minDelay, maxDelay)));
        }

        public void DestroyAllEnemies()
        {
            stopSpawning = true;

            foreach (var enemy in GetComponentsInChildren<Enemy>())
            {
                Destroy(enemy.gameObject);
            }
        }

        public void ResumeSpawning()
        {
            stopSpawning = false;
        }

        IEnumerator SpawnEnemyAfterDelay(float spawnDelay)
        {
            yield return new WaitForSeconds(spawnDelay);

            while (stopSpawning)
            {
                yield return null;
            }

            SpawnEnemy();
        }
    }
}