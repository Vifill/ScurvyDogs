using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerVersus : MonoBehaviour 
{
    public GameObject ScoreController;

	// Use this for initialization
	private void Start() 
	{
        Instantiate(ScoreController);
	}
	
	// Update is called once per frame
	private void Update() 
	{
		
	}
}
