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
    private string Coop = "TestScene_Scurvy_Teddi";
    private string Versus = "TestScene_Versus";
    private NetworkManager NetworkManager;

    private void Start()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        NetworkManager.onlineScene = Coop;
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
        NetworkManager.StartHost();
    }

    public void ButtonStartClient()
    {
        NetworkManager.StartClient();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
