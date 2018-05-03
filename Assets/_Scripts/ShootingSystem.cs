using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour {

    private List<GameObject> LeftCannonsBulletSpawn = new List<GameObject>();
    private List<GameObject> RightCannonsBulletSpawn = new List<GameObject>();

    public GameObject BlastParticles;

    public GameObject LeftCannons;
    public GameObject RightCannons;
    public GameObject CannonBallPrefab;
    public AudioClip CannonSFX;
    
    public float BulletStartVelocity = 1000;

    private float LeftCooldown = 0;
    private float RightCooldown = 0;
    private bool StartLeftCooldown;
    private bool StartRightCooldown;
    public int CooldownTime = 3;

    // Use this for initialization
    void Start ()
    {
        foreach (Transform cannon in LeftCannons.transform)
        {
            LeftCannonsBulletSpawn.Add(cannon.gameObject);
        }

        foreach (Transform cannon in RightCannons.transform)
        {
            RightCannonsBulletSpawn.Add(cannon.gameObject);
        }
    }

    private void Update()
    {
        if (LeftCooldown < CooldownTime)
        {
            LeftCooldown += Time.deltaTime;
        }
        if (RightCooldown < CooldownTime)
        {
            RightCooldown += Time.deltaTime;
        }
    }

    public void Shoot(ShipSide pSide)
    {
        if (!CanShoot(pSide))
        {
            return;
        }

        var cannonSpawnPoints = pSide == ShipSide.Left ? LeftCannonsBulletSpawn : RightCannonsBulletSpawn;
        gameObject.GetComponent<AudioSource>().PlayOneShot(CannonSFX);
        foreach (GameObject spawnPoint in cannonSpawnPoints)
        {
            var cannonBall = Instantiate(CannonBallPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            var rigidbody = cannonBall.GetComponent<Rigidbody>();
            var particle = Instantiate(BlastParticles, spawnPoint.transform.position, spawnPoint.transform.rotation, spawnPoint.transform);
            rigidbody.AddRelativeForce(new Vector3(0, 0, BulletStartVelocity), ForceMode.VelocityChange);

            Destroy(particle, 2);
        }
    }

    private bool CanShoot(ShipSide pSide)
    {
        if (pSide == ShipSide.Left)
        {
            if (LeftCooldown > CooldownTime)
            {
                LeftCooldown = 0;
                return true;
            }
            return false;
        }
        if (pSide == ShipSide.Right)
        {
            if (RightCooldown > CooldownTime)
            {
                RightCooldown = 0;
                return true;
            }
            return false;
        }
        return false;
    }

    //private void Shoot(List<GameObject> pCannonSpawnPoints)
    //{
    //    foreach (GameObject spawnPoint in pCannonSpawnPoints)
    //    {
    //        var cannonBall = Instantiate(CannonBallPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    //        var rigidbody = cannonBall.GetComponent<Rigidbody>();
    //        var particle = Instantiate(BlastParticles, spawnPoint.transform.position, spawnPoint.transform.rotation, spawnPoint.transform);
    //        //rigidbody.velocity = GetComponent<Rigidbody>().velocity;
    //        rigidbody.AddRelativeForce(new Vector3(0, 0, BulletStartVelocity), ForceMode.VelocityChange);

    //        Destroy(particle, 2);
    //    }
    //}

    public enum ShipSide
    {
        Left,
        Right
    }
}
