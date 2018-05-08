using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public Dropdown GameModeDropDown;
    public Object Coop;
    public Object Versus;
    private NetworkManager NetworkManager;
    private NetworkDiscovery NetworkDiscoverer;

    private void Start()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        NetworkDiscoverer = FindObjectOfType<NetworkDiscovery>();
        NetworkManager.onlineScene = Coop.name;
    }

    public void SelectGameMode()
    {
        if (GameModeDropDown.value == 0)
        {
            //coop
            NetworkManager.onlineScene = Coop.name;
        }
        else if (GameModeDropDown.value == 1)
        {
            //Versus
            NetworkManager.onlineScene = Versus.name;
        }
    }

    public void ButtonStartHost()
    {
        NetworkDiscoverer.Initialize();
        NetworkDiscoverer.StartAsServer();
        NetworkManager.StartHost();
    }

    public void ButtonStartClient()
    {
        NetworkDiscoverer.Initialize();
        NetworkDiscoverer.StartAsClient();
        NetworkManager.StartClient();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
