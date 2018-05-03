using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float maxHP;
    public float currentHP;
    public GameObject Orange;
    public AudioClip DamageSFX;
    public GameObject HitParticle;
    public GameObject DestroyParticle;

    private float AudioTimer = 0;
    private bool AudioTimerStart = false;
    private bool isDead = false;
    private EnemySpawner eSpawn;

    // Use this for initialization
    void Start ()
    {
        eSpawn = FindObjectOfType<EnemySpawner>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckIfDead();
        if (AudioTimerStart)
        {
            AudioTimer += Time.deltaTime;
            if (AudioTimer >= 1)
            {
                AudioTimerStart = false;
                AudioTimer = 0;
            }
        }
    }

    private void CheckIfDead()
    {
        if (!isDead)
        {
            if (currentHP <= 0)
            {
                ShipDied();
                isDead = true;
            }
        }
    }

    private void ShipDied()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, 3f, transform.position.z);
        Instantiate(Orange, spawnPos, Quaternion.identity);
        Instantiate(Orange, spawnPos, Quaternion.identity);
        Instantiate(Orange, spawnPos, Quaternion.identity);
        Instantiate(DestroyParticle, transform.position, Quaternion.Euler(Vector3.zero));
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<ShootingSystem>().enabled = false;
        eSpawn.enemyCount -= 1;
        Invoke("DestroyTheShip", 2f);
    }

    public void GotHit(int pDamage)
    {
        currentHP -= pDamage;
        if (!AudioTimerStart)
        {
            var particle = Instantiate(HitParticle, transform.position, transform.rotation, transform);
            gameObject.GetComponent<AudioSource>().PlayOneShot(DamageSFX, 0.7f);
            AudioTimerStart = true;

            Destroy(particle, 3);
        }
    }

    private void DestroyTheShip()
    {
        Destroy(gameObject);
    }
}
