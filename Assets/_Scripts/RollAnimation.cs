﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAnimation : MonoBehaviour
{

    public Rigidbody ShipRigidbody;

    public float angularVelocity;

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    angularVelocity = ShipRigidbody.angularVelocity.y * Mathf.Rad2Deg;  
	}
}
