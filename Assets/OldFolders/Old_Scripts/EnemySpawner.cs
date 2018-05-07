using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour {

    public float SpawnDist = 200;
    public float spawnRate = 20f;
    public float maxEnemy = 10f;
    public float enemyCount;
    public GameObject EnemyPrefab;

	// Use this for initialization
	void Start ()
	{
        StartCoroutine(SpawnDelay());
	}
	
    private Vector3 FindSpawnPoint()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (!players.Any())
        {
            return Vector3.zero;
        }
        var randDir = Random.Range(0, 360);
        Vector3 directionMod = Quaternion.AngleAxis(randDir, Vector3.up) * Vector3.forward;

        return players[Random.Range(0,players.Length)].transform.position + (directionMod * SpawnDist);
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(EnemyPrefab, FindSpawnPoint(), Quaternion.identity);
        NetworkServer.Spawn(enemy);
    }

   private IEnumerator SpawnDelay()
    {
        while (true)
        {
            if (enemyCount <= maxEnemy)
            {
                SpawnEnemy();
                enemyCount++;
            }

            if (spawnRate >= 5f)
            {
                spawnRate -= 1;
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
