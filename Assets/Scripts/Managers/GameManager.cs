using System;
using NUnit.Framework;
using Unity.VisualScripting;
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

    public static GameManager instance;

    public float damageIncrease = 0;
    public float damageMult = 1;
    public float critMult = 1.5f;

    public TargetType stageType;
    
    public Attachment[] attachmentPool;

    public int targetCount;

    public int secondsGiven;
    public int curTime; // in centiseconds
    public int bonusTime = 0; // in centiseconds
    public float timeMult;

    public bool isPaused {get; private set;}
    public bool forceLock { get; private set; }

    public float maxComboTimer = 1;
    public float activeComboTimer;

    public int comboValue;
    public float comboIncrease = 1;
    public float comboMult;
    public float comboMultDivisor = 10;
    public float comboMultAddition = 0;

    float roomScore;
    public int score = 1000000;

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

        EventHandeler.onSubEnemyDeath += RefreshCombo;

        SceneManager.sceneLoaded += OnSceneLoaded;

        EventHandeler.exitRoom += FinalizeScore;

        EventHandeler.enterRoom += EnterRoom;
        
        curTime = secondsGiven * 100;
    }

    void Start()
    {
        Pause(true);

    }
    public void EnterRoom()
    {
        
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

    #region Combo Stuff
    public void RefreshCombo()
    {
        comboValue += (int)comboIncrease;
        activeComboTimer = Mathf.Clamp(maxComboTimer / Mathf.Clamp(comboValue * 0.05f, 1, 10), 0.4f, maxComboTimer);
    }

    public void ComboTimer()
    {
        if (isPaused) CancelInvoke("ComboTimer");
        activeComboTimer -= .01f;
        if (activeComboTimer <= 0)   comboValue = 0;
        if (UiManager.instance)  UiManager.instance?.UpdateComboUI(activeComboTimer, comboValue);
    }
    #endregion

    #region Score
    public void AddScore()
    {
        comboMult = (1 + comboValue/comboMultDivisor) + comboMultAddition;
        if (stageType == TargetType.TRACKING)
        {
            Debug.Log("tracking");
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue / 10 * comboMult;
        }
        else
        {
            Debug.Log("not tracking");
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue * comboMult;
        }
    }

    public void FinalizeScore()
    {
        score += (int)roomScore;
        roomScore = 0;
    }
    #endregion
    public void PlayerDie()
    {
        Destroy(PlayerControler.instance.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        forceLock = true;
    }

    public void Pause(bool isPausing)
    {
        if (! isPausing && ! MapManager.instance.map.activeSelf && MapManager.instance.currentRoomType != RoomType.SHOP &&
        (!UiManager.instance || (UiManager.instance && ! UiManager.instance.activeMenu)))
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            Invoke("ComboTimer", 0.01f);
            return;
        }
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ForceLock(bool isLocking)
    {
        forceLock = isLocking;
    }

    // For button
    public void GameStart()
    {
        MapManager.instance.canLeaveRoom = true;
        MapManager.instance.OpenMap();
    }
}
