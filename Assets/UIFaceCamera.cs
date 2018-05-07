using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour 
{
    public GameObject HealthBarCanvas;
    private Camera Camera;

    private void Start()
    {
        StartCoroutine(FindCamera());
    }

    // Update is called once per frame
    private void Update() 
	{
        if (Camera != null)
        {
            HealthBarCanvas.transform.LookAt(Camera.transform);
        }
    }

    private IEnumerator FindCamera()
    {
        while (Camera == null)
        {
            Camera = FindObjectOfType<Camera>();
            yield return null;
        }
    }
}
