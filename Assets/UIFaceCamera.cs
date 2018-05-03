using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour 
{
    public GameObject HealthBarCanvas;
    private Camera Camera;

    private void Start()
    {
        Camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    private void Update() 
	{
        HealthBarCanvas.transform.LookAt(Camera.transform);
	}
}
