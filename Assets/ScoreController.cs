using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreController : NetworkBehaviour
{
    public GameObject VersusEndScreen;
    public GameObject ScoreOverlay;
    [SyncVar(hook = "OnUpdateScoreHost")]
    public int HostScore;
    [SyncVar(hook = "OnUpdateScoreClient")]
    public int ClientScore;

    private Text ScoreTextEndScreen;
    private Text WinLossText;
    private Text ScoreText;
    private Transform MainCanvas;
    private NetworkManager NetworkManager;

    public void Initialize()
    {
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        var timerText = MainCanvas.Find("TimeText");
        timerText.gameObject.SetActive(false);
        var scoreOverlay = Instantiate(ScoreOverlay, MainCanvas);
        ScoreText = scoreOverlay.transform.Find("ActualScore").GetComponent<Text>();
        NetworkManager = FindObjectOfType<NetworkManager>();
        HostScore = 0;
        ClientScore = 0;
    }

    public void GivePlayerScore(int pPlayerId)
    {
        if (pPlayerId == 1)
        {
            HostScore++;
        }
        else
        {
            ClientScore++;
        }
        CheckWhoWon();
    }

    public void OnUpdateScoreHost(int pScoreHost)
    {
        HostScore = pScoreHost;
        UpdateOverlay();
    }

    public void OnUpdateScoreClient(int pScoreClient)
    {
        ClientScore = pScoreClient;
        UpdateOverlay();
    }

    private void UpdateOverlay()
    {
        if(isClient)
        {
            ScoreText.text = HostScore + " - " + ClientScore;
        }
        else
        {
            ScoreText.text = ClientScore + " - " + HostScore;
        }
    }

    public void CheckWhoWon()
    {
        if (HostScore >= 3)
        {
            RpcDisplayScoreScreen(true);
        }
        else if (ClientScore >= 3)
        {
            RpcDisplayScoreScreen(false);
        }
    }

    [ClientRpc]
    public void RpcDisplayScoreScreen(bool pPlayerWon)
    {
        var endScreen = Instantiate(VersusEndScreen, MainCanvas);
        ScoreTextEndScreen = endScreen.transform.Find("ScoreText").GetComponent<Text>();
        WinLossText = endScreen.transform.Find("WinLossText").GetComponent<Text>();
        if (!isClient)
        {
            if (pPlayerWon)
            {
                WinLossText.text = "You Won!";
                ScoreTextEndScreen.text = HostScore + " - " + ClientScore;
            }
            else
            {
                WinLossText.text = "You Lost!";
                ScoreTextEndScreen.text = HostScore + " - " + ClientScore;
            }
        }
        else
        {
            if (pPlayerWon)
            {
                WinLossText.text = "You Won!";
                ScoreTextEndScreen.text = ClientScore + " - " + HostScore;
            }
            else
            {
                WinLossText.text = "You Lost!";
                ScoreTextEndScreen.text = ClientScore + " - " + HostScore;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LeaveGame()
    {
        if (!isClient)
        {
            NetworkManager.StopHost();
        }
        else
        {
            NetworkManager.StopClient();
        }
    }

}
