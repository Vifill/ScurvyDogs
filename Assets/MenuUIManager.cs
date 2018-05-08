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
    private NetworkManager NetworkManager;
    private NetworkDiscovery NetworkDiscoverer;

    private void Start()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        NetworkManager.onlineScene = Coop;
        NetworkDiscoverer = FindObjectOfType<NetworkDiscovery>();
    }

    public void SelectGameMode()
    {
        if (GameModeDropDown.value == 0)
        {
            //coop
            NetworkManager.onlineScene = Coop;
        }
        else if (GameModeDropDown.value == 1)
        {
            //Versus
            NetworkManager.onlineScene = Versus;
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
