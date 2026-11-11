using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Intentionally useless on start menu
public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public TimerScript timerScript;

    public GameObject activeMenu = null;
    [SerializeField] List<GameObject> menuOpenOrder;

    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject inventoryMenu;

    [SerializeField] GameObject roomCompletionMenu;
    
    #region CombatUI
    [SerializeField] GameObject combatUI;

    [SerializeField] GameObject Timer;

    [SerializeField] TMP_Text scoreAndAccuracyText;
    [SerializeField] TMP_Text finalScore;
   
    // Combo stuff
    [SerializeField] TMP_Text comboTextNum;
    [SerializeField] TMP_Text comboTextMult;
    [SerializeField] Slider comboSlider;
    [SerializeField] GameObject comboContainer;
    #endregion


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
    }

    void Start()
    {
        InventoryManager.instance.inventoryUi = inventoryMenu;
    }

    private void Update()
    {
        if (GameManager.instance.forceLock || MapManager.instance.currentRoomType == RoomType.HOME)
        {
            return;
        }

        if (MapManager.instance.currentRoomType == RoomType.BOSS 
        || MapManager.instance.currentRoomType == RoomType.MINIBOSS 
        || MapManager.instance.currentRoomType == RoomType.COMBAT)
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("tab"))
            {
                if (!activeMenu)
                {
                    MenuOpen(pauseMenu, true, true);
                    return;
                }
                else if (Input.GetKeyDown("escape") || activeMenu == pauseMenu) Back();
            }
        }
        // else if (Input.GetKeyDown("escape")) Back();

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

    public void UpdateComboUI(float activeComboTimer, float comboValue)
    {
        if (activeComboTimer <= 0)
        {
            comboContainer.SetActive(false);
            return;
        }
        comboSlider.maxValue = GameManager.instance.maxComboTimer;
        comboContainer.SetActive(true);
        comboSlider.value = activeComboTimer;
        comboTextNum.text = comboValue.ToString();
        comboTextMult.text = (Math.Round(GameManager.instance.comboMult * 100)/100).ToString() + "x";
    }

    public void MenuOpen(GameObject menu, bool isOpening, bool addNewMenuOpenOrder)
    {
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

    void PauseMenu(bool enabled)
    {
        pauseMenu.SetActive(enabled);
    }

    // these use a connected string
    public void UpdateScoreAndAccuracyText()
    {
        scoreAndAccuracyText.text = "Score: " + Mathf.Round(GameManager.instance.roomScore).ToString() + " X "
        + Mathf.Round(GameManager.instance.accuracy * 100).ToString() + "%";
    }

    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true); 
        activeMenu = deathScreen;
        finalScore.text = "Final " + scoreAndAccuracyText.text;
        Timer.SetActive(false);
        scoreAndAccuracyText.gameObject.SetActive(false);
    }

    public void RoomCompletionMenu()
    {
        bool isOpening = !roomCompletionMenu.activeSelf;
        roomCompletionMenu.SetActive(isOpening);
        GameManager.instance.ForceLock(isOpening);
        GameManager.instance.Pause(true);
        GameManager.instance.CursorLock(! isOpening);
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

    #region Button Functions
    public void MainMenuOpen()
    {
        //#TODO
    }

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
        if (menuOpenOrder.Count == 0)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
            GameManager.instance.Pause(false);
            return;
        }
        // ^1 is the same as -1 except for some reason it doesn't like -1 so I used ^1
        MenuOpen(menuOpenOrder[^1], true, false);
        menuOpenOrder.Remove(menuOpenOrder[^1]);
    }

    public void CloseAllMenus()
    {
        menuOpenOrder.Clear();
        if (activeMenu) MenuOpen(activeMenu, false, false);
    }

    public void SettingsMenu()
    {
        MenuOpen(settingsMenu, true, true);
    }

    #endregion
}
