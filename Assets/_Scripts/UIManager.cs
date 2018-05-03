using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameObject[] deathObjects;
    GameObject[] pauseObjects;

    public Image scurvyMeter;
    public Image healthMeter;
    public Text scurvyText;
    public Text healthText;
    public Text timerText;
    public Text deathTimeText;
    public Text deathText;

    private float healthPercent;
    private float scurvyPercent;
    private float timer;
    private string niceTime;

    private HealthSystem healthSys;

	// Use this for initialization
	void Start () {
        healthSys = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
        pauseObjects = GameObject.FindGameObjectsWithTag("PauseMenu");
        deathObjects = GameObject.FindGameObjectsWithTag("DeathMenu");
        hideDeath();
        hidePaused();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        healthText.text = "HP: " + healthSys.currentHP.ToString("#");
        scurvyText.text = "Scurvy " + healthSys.currentScurvy.ToString("#") + "%";
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.text = niceTime;

        ScurvyMeter();
        HPMeter();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                hidePaused();
            }
        }
    }

    private void ScurvyMeter()
    {
        scurvyPercent = healthSys.currentScurvy / 100;
        scurvyMeter.rectTransform.localScale = new Vector3(0.2f, scurvyPercent, 1f);
        if (scurvyPercent <= 0)
        {
            scurvyPercent = 0;
        }
    }

    private void HPMeter()
    {
        healthPercent = healthSys.currentHP / 100;
        healthMeter.rectTransform.localScale = new Vector3(healthPercent, 0.2f, 1f);
        if (healthPercent <= 0)
        {
            healthPercent = 0;
        }
    }

    public void showPaused()
    {
        Time.timeScale = 0;
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        Time.timeScale = 1;
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void deathMenu()
    {
        Time.timeScale = 0;
        deathTimeText.text = "You survived for " + niceTime + " minutes";
        foreach (GameObject g in deathObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideDeath()
    {
        Time.timeScale = 1;
        foreach (GameObject g in deathObjects)
        {
            g.SetActive(false);
        }
    }

    public void ReloadLevel(string level)
    {
        Scene scene = SceneManager.GetActiveScene();// SceneManager.LoadScene(scene.name);
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
