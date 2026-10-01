using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RunType
{
    CLICKING,
    SWITCHING,
    TRACKING,
    CLICK_TRACK,
    TRACK_SWITCH

}
public class GameManager : MonoBehaviour
{
    public TargetType stageType;

    public static GameManager instance;

    public Attatchment[] attatchmentPool;

    public int targetCount;

    public int minutesGiven;
    public int secondsGiven;

    public bool isPaused {get; private set;}
    public bool forceLock { get; private set; }

    public float maxComboTimer = 1;
    public float activeComboTimer;

    public int comboValue;
    public float comboMult;
    public float comboMultDivisor = 10;

    float roomScore;
    public int score;

    public RunType runType;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

        EventHandeler.exitRoom += FinalizeScore;
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
            if (UiManager.instance)
            {
                UiManager.instance?.UpdateComboUI(activeComboTimer, comboValue);
            }
        }
    }

    void DetermineTargetType()
    {
        switch(runType)
        {
            case RunType.CLICKING:
                stageType = TargetType.CLICKING;
                break;
            case RunType.SWITCHING:
                stageType = TargetType.SWITCHING;
                break;
            case RunType.TRACKING:
                stageType = TargetType.TRACKING;
                break;
            case RunType.CLICK_TRACK:
                int randomnum = UnityEngine.Random.Range(0, 2);
                if (randomnum == 0)
                {
                    stageType = TargetType.CLICKING;
                }
                else
                {
                    stageType = TargetType.TRACKING;
                }
                break;
            case RunType.TRACK_SWITCH:
                int randomnum2 = UnityEngine.Random.Range(0, 2);
                if (randomnum2 == 0)
                {
                    stageType = TargetType.TRACKING;
                }
                else
                {
                    stageType = TargetType.SWITCHING;
                }
                break;
            default:
                Debug.Log("run type out of range");
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventHandeler.updateUI?.Invoke();
        DetermineTargetType();
        if (stageType == TargetType.TRACKING)
        {
            maxComboTimer = maxComboTimer / 10;
            comboMultDivisor = comboMultDivisor * 10;
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
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue / 10 * comboMult;
        }
        else
        {
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue * comboMult;
        }
    }

    public void PlayerDie()
    {
        Destroy(PlayerControler.instance.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        forceLock = true;
    }

    public void FinalizeScore()
    {
        score += (int)roomScore;
        roomScore = 0;
    }

    public void Pause(bool isPausing)
    {
        if (! isPausing && ! MapManager.instance.map.activeSelf && 
        (!UiManager.instance || (UiManager.instance && ! UiManager.instance.activeMenu)) && ! MapManager.instance.canLeaveRoom) 
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            return;
        }
        isPaused = true;
        
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        if (MapManager.instance.canLeaveRoom && ! MapManager.instance.map.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // For button
    public void GameStart()
    {
        MapManager.instance.OpenMap();
        MapManager.instance.canLeaveRoom = true;
    }
}
