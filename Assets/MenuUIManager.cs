using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MenuUIManager : NetworkBehaviour
{

    public NetworkManager NetworkManager;

    public void ButtonStartHost()
    {
        NetworkManager.StartHost();
    }

    public void ButtonStartClient()
    {
        NetworkManager.StartClient();
    }
}
