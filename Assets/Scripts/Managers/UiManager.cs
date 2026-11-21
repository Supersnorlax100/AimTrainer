using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;

// Intentionally useless on start menu
public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public TimerScript timerScript;

    public GameObject activeMenu = null;
    List<GameObject> menuOpenOrder = new List<GameObject>(1);

    [Header("Menus")]
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject inventoryMenu;
    public GameObject roomCompletionMenu; // Needs to be public since activeSelf is used on it for a ui check (like the map in a way)

    [Space]    
    [Space]    
    [Space]    

    #region CombatUI
    [SerializeField] GameObject combatUI;

    [SerializeField] GameObject Timer;

    [SerializeField] TMP_Text scoreAndAccuracyText;
   
    [Header("Combo Stuff")]
    [SerializeField] TMP_Text comboTextNum;
    [SerializeField] TMP_Text comboTextMult;
    [SerializeField] Slider comboSlider;
    [SerializeField] GameObject comboContainer;
    #endregion

    [Header("RoomCompletionMenu")]
    [SerializeField] TMP_Text roomScoreText;
    [SerializeField] TMP_Text accText; 
    [SerializeField] TMP_Text accScoreText;
    [SerializeField] TMP_Text currentScoreText;
    [SerializeField] TMP_Text totalScoreText;
    
    [Space]
    
    [SerializeField] TMP_Text startRoomTimeText;
    [SerializeField] TMP_Text roomTimeText;
    [SerializeField] TMP_Text totalTimeText;

    [Header("InfoPrompt")]

    [SerializeField] GameObject infoPrompt;
    [SerializeField] TMP_Text infoPromptText;
    [Tooltip("How long the info prompt stays on screen")] [SerializeField] float infoPromptTime;

    [Space]
    [Space]
    [Space]

    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text floorsClearedText;
    [SerializeField] TMP_Text finalLevelText;

    public MainMenuScript mainMenuScript; //Assigned by mainMenuScript
    int infoPromptTimerID;


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

        EventHandeler.onPlayerDeath += ActivateDeathScreen;
        EventHandeler.onTargetDeath += UpdateScoreAndAccuracyText;
        EventHandeler.onEnemyDeath += UpdateScoreAndAccuracyText;
        EventHandeler.onSubEnemyDeath += UpdateScoreAndAccuracyText;

        EventHandeler.exitRoom += CloseAllMenus;
        EventHandeler.exitRoom += RoomCompletionMenu;
        EventHandeler.enterRoom += EnterRoom;

        EventHandeler.restart += CloseAllMenus;
        EventHandeler.restart += CloseSpecialMenus;
    }

    void OnDisable()
    {
        EventHandeler.onPlayerDeath -= ActivateDeathScreen;
        EventHandeler.onTargetDeath -= UpdateScoreAndAccuracyText;
        EventHandeler.onEnemyDeath -= UpdateScoreAndAccuracyText;
        EventHandeler.onSubEnemyDeath -= UpdateScoreAndAccuracyText;

        EventHandeler.exitRoom -= CloseAllMenus;
        EventHandeler.exitRoom -= RoomCompletionMenu;
        EventHandeler.enterRoom -= EnterRoom;

        EventHandeler.restart -= CloseAllMenus;
        EventHandeler.restart -= CloseSpecialMenus;
    }

    void Start()
    {
        InventoryManager.instance.inventoryUi = inventoryMenu;
    }

    private void Update()
    {
        if (GameManager.instance.forceLock) return;
        
        // Only want Escape/Back functionality in main menu, otherwise, no other menus/inputs work
        // if (Input.GetKeyDown("escape") && (MapManager.instance.map.activeSelf || mainMenuSettingsMenu.activeSelf /* menus that can be open on main menu (also may want to abstract these if more) */)
        //  && MapManager.instance.currentRoomType == RoomType.HOME) Back();
        // else if (MapManager.instance.currentRoomType == RoomType.HOME) return;

        if (MapManager.instance.currentRoomType == RoomType.BOSS 
        || MapManager.instance.currentRoomType == RoomType.MINIBOSS 
        || MapManager.instance.currentRoomType == RoomType.COMBAT)
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("tab"))
            {
                if (!activeMenu && ! MapManager.instance.map.activeSelf)
                {
                    MenuOpen(pauseMenu, true, true);
                    return;
                }
                else if (Input.GetKeyDown("escape") || activeMenu == pauseMenu) Back();
            }
        }
        else if (Input.GetKeyDown("escape")) Back();

        if (Input.GetKeyDown("i"))
        {
            if (activeMenu == inventoryMenu)
            {
                Back();
                return;
            }
            MenuOpen(inventoryMenu, true, true);
        }
    }

    public void UpdateComboUI()
    {
        if (GameManager.instance.activeComboTimer <= 0)
        {
            comboContainer.SetActive(false);
            return;
        }
        comboSlider.maxValue = GameManager.instance.maxComboTimer;
        comboContainer.SetActive(true);
        comboSlider.value = GameManager.instance.activeComboTimer;
        comboTextNum.text = GameManager.instance.comboValue.ToString();
        comboTextMult.text = (Mathf.Round(GameManager.instance.comboMult * 100)/100).ToString() + "x";
    }

    public void MenuOpen(GameObject menu, bool isOpening, bool addNewMenuOpenOrder)
    {
        if (isOpening) AudioManager.instance.UiOpenSound();
        else AudioManager.instance.UiCloseSound();

        GameManager.instance.Pause(true);
        if (activeMenu && addNewMenuOpenOrder)
        {
            menuOpenOrder.Add(activeMenu);
            activeMenu.SetActive(false);
        } 
        else if (activeMenu)
        {
            activeMenu.SetActive(false);
        }

        if (isOpening)
        {
            activeMenu = menu;
        }
        else
        {
            activeMenu = null;
            menuOpenOrder = new List<GameObject>();
            GameManager.instance.Pause(false);
            menu.SetActive(isOpening);
            return;
        }
        menu.SetActive(isOpening);
        GameManager.instance.Pause(true);
    }

    // Hides extra ui for when in combat scene and when not (or for when map is open)
    public void CombatUI(bool isVisible)
    {
        combatUI.SetActive(isVisible);
    }

    // these use a connected string
    public void UpdateScoreAndAccuracyText()
    {
        if (MapManager.instance.IsUIRoom())
        {
            return;
        }
        scoreAndAccuracyText.text = "Score: " + Mathf.CeilToInt(GameManager.instance.roomScore).ToString() + " X "
        + Mathf.CeilToInt(GameManager.instance.accuracy * 100).ToString() + "%";
    }

    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true); 
        scoreAndAccuracyText.gameObject.SetActive(false);
        CombatUI(false);
        CloseAllMenus();
        GameManager.instance.Pause(true);
        GameManager.instance.ForceLock(true);
        finalScoreText.text = GameManager.instance.score.ToString();
        floorsClearedText.text = MapManager.instance.floorsCleared.ToString();
        finalLevelText.text = MapManager.instance.currnetLvl.ToString();
    }

    // This happens for exit room when not ui, and then has a button that the player can press to open the map, etc... for the rest of the functionality
    public void RoomCompletionMenu()
    {
        if (MapManager.instance.IsUIRoom())
        {
            MapManager.instance.ExitRoom(); //Button function, but called when in ui room since no score or time calc needed
            return;
        }
        bool isOpening = !roomCompletionMenu.activeSelf;
        roomCompletionMenu.SetActive(isOpening);
        GameManager.instance.ForceLock(isOpening);
        GameManager.instance.Pause(true);
        GameManager.instance.CursorLock(! isOpening);

        // The math for the display numbers can be a little odd due to the timing of the exit room event is called
        // Debug.Log("Mathf.CeilToInt(GameManager.instance.roomScore * GameManager.instance.accuracy): " + Mathf.CeilToInt(GameManager.instance.roomScore * GameManager.instance.accuracy));
        int accScore = Mathf.CeilToInt(GameManager.instance.roomScore * GameManager.instance.accuracy);
        // Debug.Log("GameManager.instance.roomScore.ToString(): " + GameManager.instance.roomScore.ToString());
        roomScoreText.text = GameManager.instance.roomScore.ToString();
        // Debug.Log("(GameManager.instance.accuracy * 100).ToString(): " + (GameManager.instance.accuracy * 100).ToString());
        accText.text = (GameManager.instance.accuracy * 100).ToString() + "%";
        // Debug.Log("accScore.ToString(): " + accScore.ToString());
        accScoreText.text = accScore.ToString();
        // Debug.Log("Mathf.CeilToInt(GameManager.instance.score - accScore).ToString(): " + Mathf.CeilToInt(GameManager.instance.score - accScore).ToString());
        currentScoreText.text = Mathf.CeilToInt(GameManager.instance.score).ToString();
        // Debug.Log("GameManager.instance.score.ToString(): " + GameManager.instance.score.ToString());
        totalScoreText.text = (GameManager.instance.score + accScore).ToString(); 

        startRoomTimeText.text = ConvertTime(GameManager.instance.curTime + timerScript.roomTime);
        roomTimeText.text = ConvertTime(timerScript.roomTime);
        totalTimeText.text = ConvertTime(GameManager.instance.curTime);
    }

    public void EnterRoom()
    {
        RoomType curRoomType = MapManager.instance.currentRoomType;
        if (curRoomType == RoomType.COMBAT || curRoomType == RoomType.MINIBOSS || curRoomType == RoomType.BOSS)
        {
            CombatUI(true);
        }
        else
        {
            CombatUI(false);
        }
    }

    public void CloseAllMenus()
    {
        menuOpenOrder.Clear();
        if (activeMenu) MenuOpen(activeMenu, false, false);
    }

    public void CloseSpecialMenus()
    {
        deathScreen.SetActive(false);
        infoPrompt.SetActive(false);
        roomCompletionMenu.SetActive(false);
    }

    public void InfoPrompt(bool isVisible, string infoText)
    {
        infoPrompt.SetActive(isVisible);
        infoPromptText.text = infoText;
        Debug.Log("info prompt, is visible: " + isVisible);
        if (isVisible) 
        {
            infoPromptTimerID++;
            _ = InfoPromptTimer(infoPromptTimerID);
        }
    }
    // Each infoPromptTimer call will add to the timer ID, reason: if the prompt is started shortly after another it will still be using the initial timer, instead of the newly started timer
    public async Task InfoPromptTimer(int timerID)
    {
        // had to use await, instead of invoke repeating since game is paused sometimes
        await Task.Delay((int)infoPromptTime * 1000);
        if (infoPromptTimerID == timerID)
        {
            InfoPrompt(false, "");
        }
    }


    #region Button Functions
    public void PauseMenuClose()
    {
        MenuOpen(pauseMenu, false, false);
    }

    public void LeaveRoom()
    {
        RoomCompletionMenu();
        MapManager.instance.ExitRoom();
    }

    public void Back()
    {
        Debug.Log("back");
        // Special Menus
        if (MapManager.instance.map.activeSelf)
        {
            MapManager.instance.MapVisibility(false);
            return;
        }
        if (mainMenuScript && mainMenuScript.settingsMenu.activeSelf)
        {
            mainMenuScript.Close(); // handles main menu menus
            return;
        }

        if (activeMenu == null)
        {
            return;
        }

        // if closing last menu game will now "resume"
        if (menuOpenOrder.Count == 0)
        {
            activeMenu.SetActive(false);
            AudioManager.instance.UiCloseSound();
            activeMenu = null;
            GameManager.instance.Pause(false);
            return;
        }

        // close last menu
        MenuOpen(menuOpenOrder[^1], true, false);
        menuOpenOrder.Remove(menuOpenOrder[^1]);
    }


    public void SettingsMenu()
    {
        MenuOpen(settingsMenu, true, true);
    }

    #endregion

    public string ConvertTime(int time)
    {
        int minutes = 0;
        int seconds = 0;
        int centiseconds = 0;
        {
            int _time = time;

            if (_time >= 100)
            {
                seconds = Mathf.FloorToInt(_time/100);
                centiseconds = _time - (seconds * 100);
            }
            if (seconds >= 60)
            {
                minutes = Mathf.FloorToInt(seconds/60);
            }
            seconds -= minutes * 60;
        }
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }
}
