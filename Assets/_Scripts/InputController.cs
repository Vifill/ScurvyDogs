using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputController : NetworkBehaviour
{
    private ShootingSystem ShootingSystem;
    private UIManager UIManager;

    // Use this for initialization
    private void Start ()
	{
        ShootingSystem = GetComponent<ShootingSystem>();
	    StartCoroutine(FindUIManager());
	}

    private IEnumerator FindUIManager()
    {
        yield return new WaitForEndOfFrame();
        UIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
	private void Update () 
	{
	    if (!isLocalPlayer)
	    {
            return;
	    }

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    ShootingSystem.CmdShoot(ShootingSystem.ShipSide.Left);
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    ShootingSystem.CmdShoot(ShootingSystem.ShipSide.Right);
        //}

	    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
	    {
	        if (Time.timeScale == 1)
	        {
	            CmdSetPause(true, 0);
	        }
	        else if (Time.timeScale == 0)
	        {
	            CmdSetPause(false, 1);
	        }
	    }
    }

    [Command]
    public void CmdSetPause(bool pValue, float pTimeScale)
    {
        RpcShowPauseMenu(pValue, pTimeScale);
    }

    [ClientRpc]
    public void RpcShowPauseMenu(bool pValue, float pTimeScale)
    {
        UIManager.ShowPauseMenu(pValue, pTimeScale);
    }
}
