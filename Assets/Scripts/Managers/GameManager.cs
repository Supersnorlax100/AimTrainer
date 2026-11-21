
using System.Collections.Generic;
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

    [HideInInspector] public bool isSceneChanging; // On destroy functionality occurs on enemies even on scene change which throws errors

    public float[] playerStats = new float[11];
    [HideInInspector] public float damageIncreaseStat;
    [HideInInspector] public float damageMultStat;
    [HideInInspector] public float comboTimeStat;
    [HideInInspector] public float comboIncreaseStat;
    [HideInInspector] public float comboMultStat;
    [HideInInspector] public float critMultStat;
    [HideInInspector] public float critSizeStat;
    [HideInInspector] public float timeIncreaseStat;
    [HideInInspector] public float timeMultStat;
    [HideInInspector] public float bulletSizeStat;
    



    [HideInInspector] public bool isPaused { get; private set; }
    [HideInInspector] public bool forceLock { get; private set; }
    [HideInInspector] public bool cameraLock { get; private set; }
    
    [HideInInspector] public TargetType stageType;

    public Attachment[] attachmentPool;

    [SerializeField] int secondsGiven;
    [HideInInspector] public int curTime; // in centiseconds
    [HideInInspector] public int timeToStore; // in centiseconds, save time on every room clear


    [SerializeField] float defaultMaxComboTimer = 1;
    public float maxComboTimer;
    public float activeComboTimer;

    public int comboValue;
    [SerializeField] float defaultComboIncrease = 1;
    [HideInInspector] public float comboIncrease;
    [HideInInspector] public float comboMult;
    [HideInInspector] public float comboMultDivisor = 10;
    [HideInInspector] public float comboMultAddition = 0;

    float defaultCritSize = 0.4f; // based of initial crit size may need to change later.
    [HideInInspector] public float critSize;
    [SerializeField] float defaultCritMult = 1.5f; 
    [HideInInspector] public float critMult;

    [SerializeField] float baseDamage = 1;
    [HideInInspector] public float damage;
    
    [HideInInspector] public float roomScore;
    public int score;

    [SerializeField] List<MonoBehaviour> modScripts;
    public List<MonoBehaviour> modScriptsUnlocked;
    public List<MonoBehaviour> modScriptsToLock;

    public float accuracy; //Calculated by player

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

        EventHandeler.exitRoom += FinalizeScore;
        // EventHandeler.exitRoom += StopCombo;
        EventHandeler.exitRoom += GetRidOfTempMods;

        EventHandeler.enterRoom += StopCombo;
        EventHandeler.enterRoom += EnterRoom;

        EventHandeler.levelClear += LevelClear;

        EventHandeler.restart += Restart;
        
        curTime = secondsGiven * 100;

        playerStats[0] = damageIncreaseStat;
        playerStats[1] = damageMultStat;
        playerStats[2] = comboIncreaseStat;
        playerStats[3] = comboTimeStat;
        playerStats[4] = comboMultStat;
        playerStats[5] = critMultStat;
        playerStats[6] = critSizeStat;
        playerStats[7] = timeIncreaseStat;
        playerStats[8] = timeMultStat;
        playerStats[9] = bulletSizeStat;
    }

    void OnDisable()
    {
        EventHandeler.onPlayerDeath -= PlayerDie;

        EventHandeler.onTargetDeath -= RefreshCombo;
        EventHandeler.onTargetDeath -= AddScore;

        EventHandeler.onEnemyDeath -= AddScore;
        EventHandeler.onEnemyDeath -= RefreshCombo;

        EventHandeler.onSubEnemyDeath -= RefreshCombo;

        EventHandeler.exitRoom -= FinalizeScore;
        // EventHandeler.exitRoom -= StopCombo;
        EventHandeler.exitRoom -= GetRidOfTempMods;

        EventHandeler.enterRoom -= StopCombo;
        EventHandeler.enterRoom -= EnterRoom;

        EventHandeler.levelClear -= LevelClear;

        EventHandeler.restart -= Restart;
    }


    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            EventHandeler.levelClear?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Debug.Log("restart");
            EventHandeler.restart?.Invoke();
        }
        #endif
    }

    void Start()
    {
        Pause(true);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AutoHide"))
        {
            obj.SetActive(false);
        }

        UpdateStats();
    }

    public void Restart()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            playerStats[i] = 0;
        }
        StopCombo();
        GetRidOfTempMods();
        isPaused = true;
        forceLock = false;
        cameraLock = false;
        score = 0;
        roomScore = 0;
        comboValue = 0;
        curTime = secondsGiven * 100;
        UpdateStats();
    }

    public void LevelClear()
    {
        curTime = secondsGiven * 100;
    }

    public void EnterRoom()
    {
        Debug.Log("enter room");
        roomScore = 0;

        // EventHandeler.updateUI?.Invoke();
        
        DetermineTargetType();
        if (stageType == TargetType.TRACKING)
        {
            maxComboTimer = maxComboTimer / 10;
            comboMultDivisor = comboMultDivisor * 10;
        }

        #region ModStuff
        
        switch (MapManager.instance.currentRoomType)
        {
            case RoomType.COMBAT:
                break;
            case RoomType.MINIBOSS:
                AddTempMod();
                break;
            case RoomType.BOSS:
                AddPermanentMod();
                AddTempMod(); 
                break;
            case RoomType.EVENT:
                break;
            case RoomType.SHOP:
                break;
            case RoomType.HOME:
                break;
            default:
                Debug.LogError("Room type out of range GameManger EnterRoom");
                break;
        }
        #endregion
    }

    
    void AddPermanentMod()
    {
        if (modScripts.Count == 0)
        {
            Debug.Log("obtained all mods");
            return;
        }
        int randomNum = UnityEngine.Random.Range(0, modScripts.Count);
        modScriptsUnlocked.Add(modScripts[randomNum]);
        modScripts.Remove(modScripts[randomNum]);
    }
    void AddTempMod()
    {
        if (modScripts.Count == 0)
        {
            Debug.Log("obtained all mods");
            return;
        }
        int randomNum = Random.Range(0, modScripts.Count);
        if (! IsModUnlocked(modScripts[randomNum]))
        {
            modScriptsUnlocked.Add(modScripts[randomNum]);
            modScriptsToLock.Add(modScripts[randomNum]);
        }
    }

    bool IsModUnlocked(MonoBehaviour script)
    {
        foreach (MonoBehaviour _script in modScriptsUnlocked)
        {
            if (_script == script)
            {
                return true;
            }
        }
        return false;    
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
                int randomnum = Random.Range(0, 2);
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
                int randomnum2 = Random.Range(0, 2);
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
                Debug.LogError("run type out of range");
                break;
        }
    }

    public void GetRidOfTempMods()
    {
        foreach (MonoBehaviour script in modScriptsToLock)
        {
            if (modScriptsUnlocked.Contains(script))
            {
                modScriptsUnlocked.Remove(script);
            }
        }
        modScriptsToLock.Clear();
    }
   
    #region Combo Stuff
    public void RefreshCombo()
    {
        // Debug.Log("combo value" + comboValue);
        // Debug.Log("max timer: " + maxComboTimer);
        comboValue += (int)comboIncrease;
        // Debug.Log("active Combo Timer: " + Mathf.Clamp(maxComboTimer / Mathf.Clamp(comboValue * 0.05f, 1, 10), 0.4f, maxComboTimer));
        activeComboTimer = Mathf.Clamp(maxComboTimer / Mathf.Clamp(comboValue * 0.05f, 1, 10), 0.4f, maxComboTimer);
    }

    public void ComboTimer()
    {
        if (isPaused) CancelInvoke("ComboTimer");
        activeComboTimer -= .01f;
        if (activeComboTimer <= 0)   comboValue = 0;
        if (UiManager.instance)  UiManager.instance.UpdateComboUI();
    }

    public void StopCombo()
    {
        comboValue = 0;
        activeComboTimer = 0;
        UiManager.instance.UpdateComboUI();
        CancelInvoke("ComboTimer");
    }
    #endregion

    #region Score
    public void AddScore()
    {
        if (! PlayerControler.instance.target)
        {
            // Debug.Log("no target");
            return;
        }
        comboMult = (1 + comboValue/comboMultDivisor + comboMultAddition) * (1 + comboMultStat);
        if (stageType == TargetType.TRACKING)
        {
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue / 10 * comboMult;
        }
        else
        {
            roomScore += PlayerControler.instance.target.GetComponent<EnemyScript>().scoreValue * comboMult;
        }
        UiManager.instance.UpdateScoreAndAccuracyText();
    }

    public void FinalizeScore()
    {
        score += Mathf.CeilToInt(roomScore * accuracy);
        UiManager.instance.UpdateScoreAndAccuracyText();
    }
    #endregion
    public void PlayerDie()
    {
        Destroy(PlayerControler.instance.gameObject);
        CursorLock(false);
        Time.timeScale = 0;
        forceLock = true;
    }

    public void Pause(bool isPausing)
    {
        // Debug.Log("attempt pause");
        // Reqs to unpause
        if (! isPausing && ! IsActiveUI() && ! MapManager.instance.canLeaveRoom)
        {
            // Debug.Log("unpause");
            isPaused = false;
            CursorLock(true);
            Time.timeScale = 1;
            InvokeRepeating("ComboTimer", 0.01f, 0.01f);
            return;
        }
        // else if (! isPausing)
        // {
        //     // Debug.Log("issue, active ui need false: " + IsActiveUI() + "can leave need false: " + MapManager.instance.canLeaveRoom);
        //     return;
        // }
        // Debug.Log("pause, was trying: " + isPausing);
        isPaused = true;
        CancelInvoke("ComboTimer");
        if (IsActiveUI()) {CursorLock(false);}
        else {CursorLock(true);}
        Time.timeScale = 0;
    }

    // Checks if in a UI scene or if UI is active
    public bool IsActiveUI()
    {
        if (MapManager.instance.map.activeSelf || MapManager.instance.IsUIRoom() || 
        ( UiManager.instance && (UiManager.instance.roomCompletionMenu.activeSelf || UiManager.instance.activeMenu)))
        {return true;}
        else return false;
    }

    // Mainly created for when force lock is used and whatnot
    public void CursorLock(bool isLocking)
    {
        if (isLocking)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ForceLock(bool isLocking)
    {
        forceLock = isLocking;
    }

    public void CameraLock(bool isLocking)
    {
        cameraLock = isLocking;
    }

    // For button
    public void GameStart()
    {
        MapManager.instance.canLeaveRoom = true;
        MapManager.instance.OpenMap();
    }

    // For Button
    public void ButtonRestart()
    {
        EventHandeler.restart?.Invoke();
    }

    public void UpdateStats()
    {
        damageIncreaseStat = playerStats[0];
        damageMultStat = playerStats[1];
        comboTimeStat = playerStats[2];
        comboIncreaseStat = playerStats[3];
        comboMultStat = playerStats[4];
        critMultStat = playerStats[5];
        critSizeStat = playerStats[6];
        timeIncreaseStat = playerStats[7];
        timeMultStat = playerStats[8];
        bulletSizeStat = playerStats[9];

        Debug.Log("combo time stat: " + comboTimeStat);
        maxComboTimer = defaultMaxComboTimer + comboTimeStat;
        comboIncrease = defaultComboIncrease + comboIncreaseStat;
        critMult = defaultCritMult + critMultStat;
        critSize = defaultCritSize + critSizeStat;
        damage = (baseDamage + damageIncreaseStat) * (1 + damageMultStat);
    }
}
