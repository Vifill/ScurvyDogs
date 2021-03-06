﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float LowSpeed;
    public float MediumSpeed;
    public float MaximumSpeed;
    public float TransitionSpeed;
    public float CurrentSpeed;
    public Vector3 EulerAngleVelocity;

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

    private GameObject CurrentSails;
    private Rigidbody Rigidbody;

    public enum SpeedMode
    {
        Low,
        Medium,
        High
    }

	// Use this for initialization
	private void Start ()
	{
	    Rigidbody = GetComponent<Rigidbody>();
	    ChangeSails();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        RegisterInput();
        ModulateSpeed();
        Rotate();
        RotateHull();
    }
    
    private void RegisterInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (CurrentSpeedMode < SpeedMode.High)
            {
                CurrentSpeedMode++;
                ChangeSails();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (CurrentSpeedMode > SpeedMode.Low)
            {
                CurrentSpeedMode--;
                ChangeSails();
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            Turning = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Turning = 1;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Turning = 0;
        }
    }

    private void ChangeSails()
    {
        Destroy(CurrentSails);
        CurrentSails = Instantiate(Sails[(int)CurrentSpeedMode], HullModel, false);
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
        Rigidbody.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
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
}
