using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class UIManager : NetworkBehaviour
{
    GameObject[] DeathObjects;
    GameObject[] PauseObjects;

    public Image ScurvyMeterImage;
    public Image HealthMeterImage;

    public Text ScurvyText;
    public Text HealthText;
    public Text TimerText;
    public Text DeathTimeText;
    public Text DeathText;

    private float HealthPercent;
    private float ScurvyPercent;
    private float Timer;
    private string NiceTime;

    private HealthSystem HealthSys;

	// Use this for initialization
	void Start ()
	{
        
    }

    internal void Initialize(GameObject pGameObject)
    {
        HealthSys = pGameObject.GetComponent<HealthSystem>();
        PauseObjects = GameObject.FindGameObjectsWithTag("PauseMenu");
        DeathObjects = GameObject.FindGameObjectsWithTag("DeathMenu");
        HideDeath();
        HidePaused();
    }
    

    // Update is called once per frame
    void Update ()
    {
        Timer += Time.deltaTime;
        HealthText.text = "HP: " + HealthSys.currentHP.ToString("#");
        ScurvyText.text = "Scurvy " + HealthSys.currentScurvy.ToString("#") + "%";
        int minutes = Mathf.FloorToInt(Timer / 60F);
        int seconds = Mathf.FloorToInt(Timer - minutes * 60);
        NiceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        TimerText.text = NiceTime;

        ScurvyMeter();
        HpMeter();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                ShowPaused();
            }
            else if (Time.timeScale == 0)
            {
                HidePaused();
            }
        }
    }

    private void ScurvyMeter()
    {
        ScurvyPercent = HealthSys.currentScurvy / 100;
        ScurvyMeterImage.rectTransform.localScale = new Vector3(0.2f, ScurvyPercent, 1f);
        if (ScurvyPercent <= 0)
        {
            ScurvyPercent = 0;
        }
    }

    private void HpMeter()
    {
        HealthPercent = HealthSys.currentHP / 100;
        HealthMeterImage.rectTransform.localScale = new Vector3(HealthPercent, 0.2f, 1f);
        if (HealthPercent <= 0)
        {
            HealthPercent = 0;
        }
    }

    public void ShowPaused()
    {
        Time.timeScale = 0;
        foreach (GameObject g in PauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void HidePaused()
    {
        Time.timeScale = 1;
        foreach (GameObject g in PauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void DeathMenu()
    {
        Time.timeScale = 0;
        DeathTimeText.text = "You survived for " + NiceTime + " minutes";
        foreach (GameObject g in DeathObjects)
        {
            g.SetActive(true);
        }
    }

    public void HideDeath()
    {
        Time.timeScale = 1;
        foreach (GameObject g in DeathObjects)
        {
            g.SetActive(false);
        }
    }

    public void ReloadLevel(string pLevel)
    {
        Scene scene = SceneManager.GetActiveScene();// SceneManager.LoadScene(scene.name);
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
