using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HealthSystem : NetworkBehaviour
{
    public float MaxHp;
    [SyncVar]
    public float CurrentHp;
    public AudioClip DamageSFX;
    public GameObject HitParticle;
    public GameObject DeathParticle;

    public int SpawnsLeft = 0;

    private Animator Animator;
    private UIManager UIManager;
    private float AudioTimer;
    private bool AudioTimerStart;
    private bool HasDied;

    private List<NetworkStartPosition> SpawnPoints;
    private MovementController MovementController;
    private ScoreController ScoreController;

    // Use this for initialization
    void Start()
    {
        Animator = GetComponent<Animator>();
        CurrentHp = MaxHp;
        UIManager = FindObjectOfType<UIManager>();
        MovementController = GetComponent<MovementController>();
        ScoreController = FindObjectOfType<ScoreController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (AudioTimerStart)
        {
            AudioTimer += Time.deltaTime;
            if (AudioTimer >= 1)
            {
                AudioTimerStart = false;
                AudioTimer = 0;
            }
        }
        if (CurrentHp >= MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }
    
    private void CheckIfDead()
    {
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            if (!HasDied)
            {
                HasDied = true;
                //Disable the visuals

                RpcDisableShip();

                if (ScoreController != null)
                {
                    if (isLocalPlayer)
                    {
                        ScoreController.GivePlayerScore(2);
                    }
                    else
                    {
                        ScoreController.GivePlayerScore(1);
                    }
                }

                //Respawn the player if he has spawns left
                if (SpawnsLeft > 0)
                {
                    Invoke("RunRespawn", 2);
                    SpawnsLeft--;
                }
                else
                {
                    Invoke("RunChangeCamera", 2);
                    //TODO: Call endscreen
                }
            }
        }
    }

    private void RunRespawn()
    {
        RpcRespawn();
    }

    private void RunChangeCamera()
    {
        RpcChangeCamera();
    }

    private IEnumerator RunActionWithDelay(Action pAction, int pTime)
    {
        yield return new WaitForSeconds(pTime);
        pAction();
    }

    [ClientRpc]
    private void RpcDisableShip()
    {
        transform.Find("TrailHolder").gameObject.SetActive(false);
        Instantiate(DeathParticle, transform.position, transform.rotation, null);
        MovementController.HullModel.gameObject.SetActive(false);

        if (isLocalPlayer)
        {
            MovementController.SetMovement(false);
        }
    }

    [ClientRpc]
    private void RpcChangeCamera()
    {
        if (isLocalPlayer)
        {
            var otherCam = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault(a => a != gameObject).transform.Find("Camera");
            otherCam.gameObject.SetActive(true);
            CmdKillMe();
        }
    }

    [Command]
    private void CmdKillMe()
    {
        NetworkServer.Destroy(gameObject);
    } 

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = /*Vector3.zero*/ new Vector3(0f, 1.5f, 0);

            // If there is a spawn point array and the array is not empty, pick one at random
            if (SpawnPoints != null && SpawnPoints.Count > 0)
            {
                spawnPoint = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].transform.position;
            }
            transform.position = spawnPoint;

            //Enable everything again
            MovementController.HullModel.gameObject.SetActive(true);
            transform.Find("TrailHolder").gameObject.SetActive(true);
            MovementController.SetMovement(true);
            CurrentHp = MaxHp;
            HasDied = false;
            SetAnimation(1);
            //Animator.playbackTime = 0;
            Animator.Play("Spawn", -1, 0f);
            //Animator.
            //Animator.SetBool(0, true);
        }
    }

    public void GotHit(int pDamage)
    {
        if (!isServer)
        {
            return;
        }
        //Make all clients instantiate 'hit' particle effect
        RpcParticleHit();
        ChangeHp(-pDamage);
    }

    [ClientRpc]
    private void RpcParticleHit()
    {
        if (!AudioTimerStart)
        {
            var particle = Instantiate(HitParticle, transform.position, transform.rotation, transform);
            gameObject.GetComponent<AudioSource>().PlayOneShot(DamageSFX, 0.7f);
            AudioTimerStart = true;

            Destroy(particle, 3);
        }
    }

    public void SetAnimation(int pValue)
    {
        Animator.enabled = pValue != 0;
    }

    public void Death()
    {
        UIManager.DeathText.text = "Until you died";
        UIManager.ShowDeathMenu();
    }

    public void ChangeHp(int pValue)
    {
        CurrentHp += pValue;
        CheckIfDead();
    }
}