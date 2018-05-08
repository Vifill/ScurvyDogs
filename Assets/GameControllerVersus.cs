using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerVersus : MonoBehaviour 
{
    public GameObject ScoreController;

    public void Initialize(GameObject pShip)
    {
        Instantiate(ScoreController);
        pShip.GetComponent<HealthSystem>().CmdSetSpawns(3);
    }
}
