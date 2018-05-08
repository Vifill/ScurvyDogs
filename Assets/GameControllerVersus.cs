using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerVersus : MonoBehaviour 
{
    public GameObject ScoreController;

	// Use this for initialization
	private void Start() 
	{

    }

    public void Initialize(GameObject pShip)
    {
        Instantiate(ScoreController);

        var healthSys = pShip.GetComponent<HealthSystem>();
        healthSys.SpawnsLeft = 3;
    }
}
