using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : NetworkBehaviour
{
    public Dropdown GameModeDropDown;
    private NetworkManager NetworkManager;

    public void SelectGameMode()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();

        if (GameModeDropDown.value == 0)
        {
            //coop
            var scene = SceneManager.GetSceneByBuildIndex(1).name;
            NetworkManager.onlineScene = scene;
        }
        else if (GameModeDropDown.value == 1)
        {
            //Versus
            var scene = SceneManager.GetSceneByBuildIndex(2).name;
            NetworkManager.onlineScene = scene;
        }
    }

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
