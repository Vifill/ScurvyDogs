using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputController : NetworkBehaviour
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
	    if (!isLocalPlayer)
	    {
            return;
	    }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootingSystem.CmdShoot(ShootingSystem.ShipSide.Left);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ShootingSystem.CmdShoot(ShootingSystem.ShipSide.Right);
        }
    }
}
