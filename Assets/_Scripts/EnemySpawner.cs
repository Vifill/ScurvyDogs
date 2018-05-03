using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float SpawnDist = 200;
    public float spawnRate = 20f;
    public float maxEnemy = 10f;
    public float enemyCount = 0;
    public GameObject Enemy;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnDelay());
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    private Vector3 FindSpawnPoint()
    {
        var randDir = Random.Range(0, 361);
        Vector3 directionMod = Quaternion.AngleAxis(randDir, Vector3.up) * Vector3.forward;

        return transform.position + (directionMod * SpawnDist);
    }

    private void SpawnEnemy()
    {
        Instantiate(Enemy, FindSpawnPoint(), Quaternion.identity);
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
