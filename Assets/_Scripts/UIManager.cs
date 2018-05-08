using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class UIManager : NetworkBehaviour
{
    public Image HealthMeterImage;

    public Text HealthText;
    public Text TimerText;
    public Text DeathTimeText;
    public Text DeathText;
    public GameObject PauseMenu;
    public GameObject DeathMenu;

    private float HealthPercent;
    private float Timer;
    public string NiceTime;
    private bool IsVersusMode = false;

    private HealthSystem HealthSys;
    private GameControllerVersus GameControllerVersus;

    internal void Initialize(GameObject pGameObject)
    {
        GameControllerVersus = FindObjectOfType<GameControllerVersus>();
        if (GameControllerVersus != null)
        {
            IsVersusMode = true;
        }

        HealthSys = pGameObject.GetComponent<HealthSystem>();
        HideDeath();
        if (IsVersusMode)
        {
            GameControllerVersus.Initialize(pGameObject);
            
            FindObjectOfType<ScoreController>().Initialize();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (!IsVersusMode)
        {
            Timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(Timer / 60F);
            int seconds = Mathf.FloorToInt(Timer - minutes * 60);
            NiceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            TimerText.text = NiceTime;
        }

        HealthText.text = "HP: " + HealthSys.CurrentHp.ToString("#");
        HpMeter();
    }
    
    public void ShowPauseMenu(bool pValue, float pTimeScale)
    {
        PauseMenu.SetActive(pValue);
        Time.timeScale = pTimeScale;
    }

    private void HpMeter()
    {
        HealthPercent = HealthSys.CurrentHp / 100;
        HealthMeterImage.rectTransform.localScale = new Vector3(HealthPercent, 0.2f, 1f);
        if (HealthPercent <= 0)
        {
            HealthPercent = 0;
        }
    }

    public void ShowDeathMenu(string pString)
    {
        if (!IsVersusMode)
        {
            Time.timeScale = 0;
            DeathTimeText.text = "You survived for " + pString + " minutes";
            DeathMenu.SetActive(true);
        }
    }

    public void HideDeath()
    {
        Time.timeScale = 1;
        DeathMenu.SetActive(false);
    }

    public void ReloadLevel(string pLevel)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LeaveGame()
    {
        if (!isClient)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
