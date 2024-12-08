using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        
        public float minDelay = 10;
        public float maxDelay = 15;

        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;
        public Player.Player player;
        public Collider2D[] spawnAreas;
        private bool stopSpawning = false;
        private float time;
        [SerializeField] private int initialEnemiesNumber;
        [SerializeField] private float increaseDifficultyInSeconds;

        private void Start()
        {
            //StartCoroutine(SpawnEnemyAfterDelay(Random.Range(minDelay, maxDelay)));
            for (int i = 0; i < initialEnemiesNumber; i++)
            {
                SpawnEnemy();
            }

        }

        private void SpawnEnemy(int overrideSize = -1)
        {
            if (!player.isActiveAndEnabled)
                return;
            
            var spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
            var minSpawnPos = spawnArea.bounds.min;
            var maxSpawnPos = spawnArea.bounds.max;
            var spawnPos = new Vector2(Random.Range(minSpawnPos.x, maxSpawnPos.x), Random.Range(minSpawnPos.y, maxSpawnPos.y));
            var randomEnemy = Random.Range(0, enemyPrefabs.Length);
            var newEnemy = Instantiate(enemyPrefabs[randomEnemy], this.transform);
            
            newEnemy.transform.position = spawnPos;
            newEnemy.GetComponent<Enemy>().target = player;
            var randomScale = Random.Range(minScale, maxScale);
            newEnemy.transform.localScale += new Vector3(randomScale, randomScale, 0);

            StartCoroutine(SpawnEnemyAfterDelay(Random.Range(minDelay, maxDelay)));
        }

        void Update()
        {
            time += Time.deltaTime;

            if (time > increaseDifficultyInSeconds)
            {
                if (minScale < 0.7f)
                {
                    minScale += 0.1f;
                }
                maxDelay += (minDelay - maxDelay) / 3;
                time = 0;
            }
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