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

    public TargetType stageType;
    
    public Attachment[] attachmentPool;

    public int targetCount;

    public int secondsGiven;
    public int curTime; // in centiseconds

    public bool isPaused {get; private set;}
    public bool forceLock { get; private set; }

    public float maxComboTimer = 1;
    public float activeComboTimer;

    public int comboValue;
    public float comboMult;
    public float comboMultDivisor = 10;

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

    private void Update()
    {
        //#TODO replace with invoke repeating and in pause function
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

    public void EnterRoom()
    {
        
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
            Debug.Log("tracking");
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue / 10 * comboMult;
        }
        else
        {
            Debug.Log("not tracking");
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
        if (! isPausing && ! MapManager.instance.map.activeSelf && MapManager.instance.currentRoomType != RoomType.SHOP &&
        (!UiManager.instance || (UiManager.instance && ! UiManager.instance.activeMenu)))
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            // #TODO: Consider if in the shop the game should be "paused"
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
