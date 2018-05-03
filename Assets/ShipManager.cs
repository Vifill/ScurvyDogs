using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipManager : NetworkBehaviour 
{
    public GameObject UIManager;
    public GameObject MyCamera;

    public override void OnStartLocalPlayer()
    {
        var manager = Instantiate(UIManager).GetComponent<UIManager>();
        manager.Initialize(gameObject);
        MyCamera.SetActive(true);
    }
}
