using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private ShootingSystem ShootingSystem;

    // Use this for initialization
    private void Start ()
	{
        ShootingSystem = GetComponent<ShootingSystem>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootingSystem.Shoot(ShootingSystem.ShipSide.Left);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ShootingSystem.Shoot(ShootingSystem.ShipSide.Right);
        }
    }
}
