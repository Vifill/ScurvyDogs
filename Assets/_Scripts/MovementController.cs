using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovementController : NetworkBehaviour
{
    public float LowSpeed;
    public float MediumSpeed;
    public float MaximumSpeed;
    public float TransitionSpeed;
    public float CurrentSpeed;
    public Vector3 EulerAngleVelocity;

    [SyncVar(hook = "ChangeSails")]
    public SpeedMode CurrentSpeedMode;

    public float MaxAngularVelocity;
    public float AngularVelocity;
    public float AngularAcceleration;
    public float Turning;

    public float displayTurn;

    public float MaxHullRotation;
    public float HullRotationAcceleration;

    public GameObject HullToRotate;
    public GameObject[] Sails;
    public Transform HullModel;
    public Joystick Joystick;

    private GameObject CurrentSails;
    private Rigidbody Rigidbody;
    private bool IsStopMovement;

    public enum SpeedMode
    {
        Low,
        Medium,
        High
    }

	// Use this for initialization
	private void Start()
	{
	    Rigidbody = GetComponent<Rigidbody>();
        ChangeSails(CurrentSpeedMode);
        Joystick = FindObjectOfType<FixedJoystick>();
        Turning = 0;
    }
	
	// Update is called once per frame
	private void Update()
    {
        //Don't do anything if you're not the player
        if (!isLocalPlayer || IsStopMovement) return;
        RegisterInput();
        ModulateSpeed();
        Rotate();
        RotateHull();
    }
    
    private void RegisterInput()
    {
        Debug.Log("Horizontal = "+Joystick.Horizontal);
        Debug.Log("Vertical = "+Joystick.Vertical);

        if (Joystick.Vertical >= 0.9f)
        {
            if (CurrentSpeedMode < SpeedMode.High)
            {
                CmdChangeSails(1);
            }
        }
        if (Joystick.Vertical <= -0.9f)
        {
            if (CurrentSpeedMode > SpeedMode.Low)
            {
                CmdChangeSails(-1);
            }
        }
        Turning = Joystick.Horizontal;
        
        //if (Input.GetKeyDown(KeyCode.W) || Joystick.Horizontal >= .5f)
        //{
        //    if (CurrentSpeedMode < SpeedMode.High)
        //    {
        //        CmdChangeSails(1);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.S) || Joystick.Horizontal >= -.5f)
        //{
        //    if (CurrentSpeedMode > SpeedMode.Low)
        //    {
        //        CmdChangeSails(-1);
        //    }
        //}

        //if (Input.GetKey(KeyCode.A) || Joystick.Vertical >= .1f)
        //{
        //    Turning = -1;
        //}
        //if (Input.GetKey(KeyCode.D) || Joystick.Vertical >= -.1f)
        //{
        //    Turning = 1;
        //}
        //else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Joystick.Vertical == 0)
        //{
        //    Turning = 0;
        //}
    }

    [Command]
    private void CmdChangeSails(int pValue)
    {
        CurrentSpeedMode += pValue;
    }

    private void ChangeSails(SpeedMode pSpeedMode)
    {
        CurrentSpeedMode = pSpeedMode;
        Destroy(CurrentSails);
        CurrentSails = Instantiate(Sails[(int)pSpeedMode], HullModel, false);
    }

    private void RotateHull()
    {
        var targetRotation = Quaternion.Euler(new Vector3(HullToRotate.transform.rotation.eulerAngles.x, HullToRotate.transform.rotation.eulerAngles.y, MaxHullRotation * Turning));
        HullToRotate.transform.rotation = Quaternion.Lerp(HullToRotate.transform.rotation, targetRotation, Time.deltaTime * HullRotationAcceleration);
    }

    private void Rotate()
    {
        var targetAngularVelocity = MaxAngularVelocity * Turning;
        AngularVelocity = Mathf.Lerp(AngularVelocity, targetAngularVelocity, Time.deltaTime * AngularAcceleration);

        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, AngularVelocity, 0) * Time.deltaTime);
        Rigidbody.MoveRotation(Rigidbody.rotation * deltaRotation);
    }

    private void ModulateSpeed()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, GetTargetSpeed(), TransitionSpeed * Time.deltaTime);
        Rigidbody.velocity = transform.forward * CurrentSpeed;
        //Rigidbody.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
    }

    private float GetTargetSpeed()
    {
        switch (CurrentSpeedMode)
        {
            case SpeedMode.Low:
                return LowSpeed;
            case SpeedMode.Medium:
                return MediumSpeed;
            case SpeedMode.High:
                return MaximumSpeed;
        }
        return 0;
    }

    public void SetMovement(bool pValue)
    {
        if (pValue)
        {
            CurrentSpeedMode = SpeedMode.Low;
            IsStopMovement = false;
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.rotation = Quaternion.identity;
            IsStopMovement = true;
        }
    }
}
