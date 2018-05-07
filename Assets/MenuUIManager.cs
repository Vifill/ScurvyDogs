using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MenuUIManager : NetworkBehaviour
{
    private NetworkManager NetworkManager;

    public void ButtonStartHost()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        NetworkManager.StartHost();
    }

    public void ButtonStartClient()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        NetworkManager.StartClient();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
