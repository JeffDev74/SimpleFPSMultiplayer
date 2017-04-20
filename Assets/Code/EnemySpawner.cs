using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class EnemySpawner : NetworkBehaviour
	{
        public GameObject enemyPrefab;
        public int numberOfEnemies;

        public override void OnStartServer()
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                var spawnPosition = new Vector3(Random.Range(-8, 8), 0.0f, Random.Range(-8, 8));

                var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);

                var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
                NetworkServer.Spawn(enemy);
            }
        }
    }
}