﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public float maxHP;
    public float currentHP;
    public float maxScurvy;
    public float currentScurvy;
    public AudioClip DamageSFX;
    public GameObject HitParticle;
    public GameObject DeathParticle;

    private UIManager uiManager;
    private float AudioTimer = 0;
    private bool AudioTimerStart = false;
    private bool HasDied = false;

    // Use this for initialization
    void Start()
    {
        currentScurvy = 0;
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ScurvyMeter();
        CheckIfDead();
        if (AudioTimerStart)
        {
            AudioTimer += Time.deltaTime;
            if (AudioTimer >= 1)
            {
                AudioTimerStart = false;
                AudioTimer = 0;
            }
        }
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
        if (currentScurvy <= 0f)
        {
            currentScurvy = 0f;
        }
    }

    private void ScurvyMeter()
    {
        currentScurvy += Time.deltaTime;
    }

    private void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            if (!HasDied)
            {
                HasDied = true;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.Find("TrailHolder").gameObject.SetActive(false);
                transform.Find("Camera").parent = null;
                var particle = Instantiate(DeathParticle, transform.position, transform.rotation, null);
                Invoke("Death", 2);
            }

        }
        else if (currentScurvy >= 100)
        {
            uiManager.deathText.text = "Until you died from scurvy";
            uiManager.deathMenu();
        }
    }

    public void GotHit(int pDamage)
    {
        currentHP -= pDamage;
        if (!AudioTimerStart)
        {
            var particle = Instantiate(HitParticle, transform.position, transform.rotation, transform);
            gameObject.GetComponent<AudioSource>().PlayOneShot(DamageSFX, 0.7f);
            AudioTimerStart = true;

            Destroy(particle, 3);
        }
    }

    public void Death()
    {
        uiManager.deathText.text = "Until you died";
        uiManager.deathMenu();
    }
}