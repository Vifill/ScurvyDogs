using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootingSystem : NetworkBehaviour
{
    private List<GameObject> LeftCannonsBulletSpawn = new List<GameObject>();
    private List<GameObject> RightCannonsBulletSpawn = new List<GameObject>();

    public GameObject BlastParticles;

    public GameObject LeftCannons;
    public GameObject RightCannons;
    public GameObject CannonBallPrefab;
    public AudioClip CannonSFX;
    
    public float BulletStartVelocity = 1000;

    private float LeftCooldown;
    private float RightCooldown;
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

    [Command]
    public void CmdShoot(ShipSide pSide)
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
            NetworkServer.Spawn(cannonBall);
            var particle = Instantiate(BlastParticles, spawnPoint.transform.position, spawnPoint.transform.rotation, spawnPoint.transform);

            Destroy(particle, 2);
        }
    }

    private IEnumerator ApplyForce(GameObject pCannonBall)
    {
        yield return new WaitForEndOfFrame();
        pCannonBall.GetComponent<Rigidbody>()
            .AddRelativeForce(new Vector3(0, 0, BulletStartVelocity), ForceMode.VelocityChange);
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

    public enum ShipSide
    {
        Left,
        Right
    }
}
