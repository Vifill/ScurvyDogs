using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public Dropdown GameModeDropDown;
    private string Coop = "Game_Coop";
    private string Versus = "Game_Versus";
    //private NetworkManager NetworkManager;
    //private NetworkDiscovery NetworkDiscoverer;

    private void Start()
    {
        NetworkManager.singleton.onlineScene = Coop;
        CustomNetworkDiscoverer.Instance.onServerDetected += OnReceiveBraodcast;
    }

    public void SelectGameMode()
    {
        if (GameModeDropDown.value == 0)
        {
            //coop
            NetworkManager.singleton.onlineScene = Coop;
        }
        else if (GameModeDropDown.value == 1)
        {
            //Versus
            NetworkManager.singleton.onlineScene = Versus;
        }
    }

    public void ButtonStartHost()
    {
        CustomNetworkDiscoverer.Instance.StartBroadcasting();
        NetworkManager.singleton.StartHost();
    }

    public void ButtonStartClient()
    {
        CustomNetworkDiscoverer.Instance.ReceiveBraodcast();
    }

    public void JoinGame(string pIp)
    {
        NetworkManager.singleton.networkAddress = pIp;
        NetworkManager.singleton.StartClient();
        CustomNetworkDiscoverer.Instance.StopBroadcasting();
    }

    public void OnReceiveBraodcast(string fromIp, string data)
    {
        JoinGame(fromIp);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
