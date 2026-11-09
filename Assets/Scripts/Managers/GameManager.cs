
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

    [HideInInspector] public float[] playerStats = new float[11];
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
    
    [HideInInspector] public TargetType stageType;

    public Attachment[] attachmentPool;

    [SerializeField] int secondsGiven;
    [HideInInspector] public int curTime; // in centiseconds
    [HideInInspector] public int timeToStore; // in centiseconds, save time on every room clear


    [SerializeField] float defaultMaxComboTimer = 1;
    [HideInInspector] public float maxComboTimer;
    [HideInInspector] public float activeComboTimer;

    [HideInInspector] public int comboValue;
    [SerializeField] float defaultComboIncrease = 1;
    [HideInInspector] public float comboIncrease;
    [HideInInspector] public float comboMult;
    [HideInInspector] public float comboMultDivisor = 10;
    [HideInInspector] public float comboMultAddition = 0;

    float defaultCritSize = 0.4f; // based of initial crit size may need to change later.
   [HideInInspector]  public float critSize;
    [SerializeField] float defaultCritMult = 1.5f; 
    [HideInInspector] public float critMult;

    [SerializeField] float baseDamage = 1;
    [HideInInspector] public float damage;
    
    [HideInInspector] public float roomScore;
    [HideInInspector] public int score;
    
    [SerializeField] List<Component> modScripts;
    public List<Component> modScriptsUnlocked;
    public List<Component> modScriptsToLock;



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
        EventHandeler.exitRoom += GetRidOfTempMods;

        EventHandeler.enterRoom += EnterRoom;
        
        curTime = secondsGiven * 100;

        //#TODO: cleaner integration and proper display of player stats
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

    void Start()
    {
        Pause(true);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AutoHide"))
        {
            obj.SetActive(false);
        }

        UpdateStats();
    }
    public void EnterRoom()
    {
        Debug.Log(MapManager.instance.currentRoomType);
        bool loop = true;
        switch (MapManager.instance.currentRoomType)
        {
            case RoomType.COMBAT:
                break;
            case RoomType.MINIBOSS:
                int iterations = 0;
                while (loop)
                {
                    if (iterations > 1000)
                    {
                        Debug.Log("Error: GameManager EnterRoom: Could not find a mod to unlock");
                        loop = false;
                        break;
                    }
                    if (modScriptsUnlocked.Count == modScripts.Count)
                    {
                        loop = false;
                        break;
                    }
                    int randomNum = UnityEngine.Random.Range(0, modScripts.Count);
                    bool modNotUnlocked = true;
                    foreach (Component script in modScriptsUnlocked)
                    {
                        if (script == modScripts[randomNum])
                        {
                            modNotUnlocked = false;
                            break;
                        }
                    }
                    if (modNotUnlocked)
                    {
                        modScriptsUnlocked.Add(modScripts[randomNum]);
                        modScriptsToLock.Add(modScripts[randomNum]);
                        loop = false;
                    }
                    iterations++;
                }
                break;
            case RoomType.BOSS:
                while (loop)
                {
                    if (modScriptsUnlocked.Count == modScripts.Count)
                    {
                        loop = false;
                        break;
                    }
                    int randomNum = UnityEngine.Random.Range(0, modScripts.Count);
                    bool modNotUnlocked = true;
                    foreach (Component script in modScriptsUnlocked)
                    {
                        if (script == modScripts[randomNum])
                        {
                            modNotUnlocked = false;
                            break;
                        }
                    }
                    if (modNotUnlocked)
                    {
                        modScriptsUnlocked.Add(modScripts[randomNum]);
                        modScripts.Remove(modScripts[randomNum]);
                        loop = false;
                    }
                    else
                    {
                        Debug.LogError("Error: Mod:" + modScripts[randomNum].name + " already unlocked but is not removed from ModScripts");
                    }
                }
                break;
            case RoomType.EVENT:
                break;
            case RoomType.SHOP:
                break;
            default:
                Debug.Log("Room type out of range GameManger EnterRoom");
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

    public void GetRidOfTempMods()
    {
        foreach (Component script in modScriptsToLock)
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
        comboMult = (1 + comboValue/comboMultDivisor + comboMultAddition) * (1 + comboMultStat);
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
        UiManager.instance.UpdateScore();
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
        if (! isPausing && ! MapManager.instance.map.activeSelf && MapManager.instance.currentRoomType != RoomType.SHOP
        && MapManager.instance.currentRoomType != RoomType.EVENT && (!UiManager.instance || (UiManager.instance && ! UiManager.instance.activeMenu)))
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            InvokeRepeating("ComboTimer", 0.01f, 0.01f);
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

    public void UpdateStats()
    {
        maxComboTimer = defaultMaxComboTimer + comboTimeStat;
        comboIncrease = defaultComboIncrease + comboIncreaseStat;
        critMult = defaultCritMult + critMultStat;
        critSize = defaultCritSize + critSizeStat;
        damage = (baseDamage + damageIncreaseStat) * (1 + damageMultStat);
    }
}
