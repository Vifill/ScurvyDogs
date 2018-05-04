using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class AIMovement : NetworkBehaviour
{
    private GameObject Player;

    private AIState CurrentState;
    private Vector3 NextDestination;

    private Quaternion TargetAngle;
    private Rigidbody Rigidbody;
    private ShootingSystem ShootingSystem;

    public float MaxWaypointDistance;
    public float EngageDistance;
    public float ShootDistance;
    public float Speed;
    public float SpeedWhileFiring;
    public float MinAngleToShoot = 10;
    public float RotationSpeed = 2;
    public float TargetingCooldown = 2.5f;

    private enum AIState
    {
        Patrol,
        Engaged
    }

	// Use this for initialization
	private void Start ()
	{
	    Rigidbody = GetComponent<Rigidbody>();
	    StartCoroutine(FindNearestTargetCoroutine());
        ShootingSystem = GetComponent<ShootingSystem>();
	    CreateNextDestinationPoint();
	    CurrentState = AIState.Patrol;
	}

    // Update is called once per frame
	private void Update ()
	{
	    //Debug.Log("Distance: " + Vector3.Distance(Player.transform.position, transform.position));
	    UpdateState();
	}

    private void UpdateState()
    {
        switch (CurrentState)
        {
            case AIState.Patrol: UpdatePatrolState(); break;
            case AIState.Engaged: UpdateEngagedState(); break;
        }
    }

    private IEnumerator FindNearestTargetCoroutine()
    {
        while (true)
        {
            try
            {
                Player = FindNearestTarget();
            }
            catch
            {
                print("Couldn't find the nearest target");
            }
            yield return new WaitForSeconds(TargetingCooldown);
        }
    }

    private GameObject FindNearestTarget()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        return players.OrderBy(a => Vector3.Distance(a.transform.position, transform.position)).First();
    }

    private void UpdateEngagedState()
    {
        if (!IsPlayerClose())
        {
            CurrentState = AIState.Patrol;
            return;
        }

        if (!IsPlayerCloseEnoughToFireAt())
        {
            MoveTowards(Player.transform.position);
        }
        else
        {
            AlignShipForFire();
            if (AngleCloseEnough())
            {
                FireCannons();
            }

            Move(SpeedWhileFiring);
            //transform.Translate(Vector3.forward * SpeedWhileFiring * Time.deltaTime);
        }

        Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, TargetAngle, Time.deltaTime * RotationSpeed));
    }

    private void Move(float pSpeed)
    {
        Rigidbody.velocity = transform.forward * pSpeed;
    }

    private bool AngleCloseEnough()
    {
        return true;
    }
    
    private void FireCannons()
    {
        //ShootingSystem.CmdShoot(PlayerSide());
    }

    private ShootingSystem.ShipSide PlayerSide()
    {
        Vector3 dirToPlayer = Player.transform.position - transform.position;
        
        var cross = Vector3.Cross(dirToPlayer, transform.forward);

        return cross.y < 0 ? ShootingSystem.ShipSide.Right : ShootingSystem.ShipSide.Left;
    }
    
    private void AlignShipForFire()
    {
        Player.GetComponent<MovementController>();

        Vector3 dirToPlayer = Player.transform.position - transform.position + Player.transform.forward * 25;
        dirToPlayer.y = 0;
        var angle = Vector3.Angle(dirToPlayer, transform.forward);
        var cross = Vector3.Cross(dirToPlayer, transform.forward);
        //angle = cross.y < 0 ? 360-angle : angle;

        var rotation = Quaternion.LookRotation(dirToPlayer);
        rotation *= Quaternion.Euler(0, cross.y < 0 ? -90 : 90, 0);

        Debug.DrawRay(transform.position, rotation.eulerAngles);

        TargetAngle = rotation;
    }

    private bool IsPlayerCloseEnoughToFireAt()
    {
        return Vector3.Distance(Player.transform.position, transform.position) < ShootDistance;
    }

    private void UpdatePatrolState()
    {
        if (Player != null && IsPlayerClose())
        {
            CurrentState = AIState.Engaged;
            return;
        }

        if (Vector3.Distance(transform.position, NextDestination) < 10)
        {
            CreateNextDestinationPoint();
        }

        MoveTowardsNextWaypoint();
    }

    private void MoveTowardsNextWaypoint()
    {
        MoveTowards(NextDestination);
    }

    private void MoveTowards(Vector3 dest)
    {
        var pos = dest - transform.position;
        var newRot = Quaternion.LookRotation(pos);
        Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime));
        Move(Speed);
    }

    private void CreateNextDestinationPoint()
    {
        var randDir = UnityEngine.Random.Range(0, 361);
        Vector3 directionMod = Quaternion.AngleAxis(randDir, Vector3.up) * Vector3.forward;
        var distance = UnityEngine.Random.Range(0f, 1f) * MaxWaypointDistance;
        
        NextDestination = transform.position + (directionMod * distance);
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(Player.transform.position, transform.position) < EngageDistance;
    }
}
