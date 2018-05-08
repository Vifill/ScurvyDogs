using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkDiscoverer : NetworkDiscovery
{
    public static CustomNetworkDiscoverer Instance;

    public Action<string, string> onServerDetected;

    void OnServerDetected(string fromAddress, string data)
    {
        if (onServerDetected != null)
        {
            onServerDetected.Invoke(fromAddress, data);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        InitializeNetworkDiscovery();
    }

    public bool InitializeNetworkDiscovery()
    {
        return Initialize();
    }

    public void StartBroadcasting()
    {
        StartAsServer();
    }

    public void ReceiveBraodcast()
    {
        StartAsClient();
    }

    public void StopBroadcasting()
    {
        StopBroadcast();
    }

    public void SetBroadcastData(string broadcastPayload)
    {
        broadcastData = broadcastPayload;
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        OnServerDetected(fromAddress.Split(':')[3], data);
    }
}
