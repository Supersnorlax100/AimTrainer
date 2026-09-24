using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TargetType stageType;

    public static GameManager instance;

    public int targetCount;

    public int minutesGiven;
    public int secondsGiven;

    public bool isPaused;
    public bool forceLock;

    public float maxComboTimer = 1;
    public float activeComboTimer;

    public int comboValue;
    public float comboMult;
    public float comboMultDivisor = 10;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        EventHandeler.onPlayerDeath += PlayerDie;

        EventHandeler.onTargetDeath += RefreshCombo;
        EventHandeler.onTargetDeath += AddScore;

        EventHandeler.onEnemyDeath += AddScore;
        EventHandeler.onEnemyDeath += RefreshCombo;

        EventHandeler.onPetalDeath += AddScore;
        EventHandeler.onPetalDeath += RefreshCombo;
    }

    void Start()
    {
        if (stageType == TargetType.TRACKING)
        {
            maxComboTimer = maxComboTimer/10;
            comboMultDivisor = comboMultDivisor*10;
        }
    }

    private void Update()
    {
        // Handle Combo Logic
        if (!isPaused)
        {
            activeComboTimer -= Time.deltaTime;
            if (activeComboTimer <= 0)
            {
                comboValue = 0;
            }
            UiManager.instance?.UpdateComboUI(activeComboTimer, comboValue);
        }
    }

    public void RefreshCombo()
    {
        comboValue += 1;
        activeComboTimer = Mathf.Clamp(maxComboTimer / Mathf.Clamp(comboValue * 0.05f, 1, 10), 0.4f, 5);
    }

    public void AddScore()
    {
        comboMult = 1 + comboValue/comboMultDivisor;
        if (stageType == TargetType.TRACKING)
        {
            PlayerControler.instance.roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue / 10 * comboMult;
        }
        else
        {
            PlayerControler.instance.roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue * comboMult;
        }
    }

    public void PlayerDie()
    {
        Destroy(PlayerControler.instance.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        forceLock = true;
    }

    public void Pause(bool isPausing)
    {
        isPaused = isPausing;
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
