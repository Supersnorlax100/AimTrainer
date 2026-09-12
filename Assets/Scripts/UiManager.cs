using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public GameObject activeMenu = null;
    [SerializeField] List<GameObject> menuOpenOrder;

    [SerializeField] GameObject Timer;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;

    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject pauseMenu;

    // Combo stuff
    [SerializeField] TMP_Text comboTextNum;
    [SerializeField] TMP_Text comboTextMult;
    [SerializeField] Slider comboSlider;
    [SerializeField] GameObject comboContainer;

    // Settings
    [SerializeField] Slider mouseSensSlider;
    [SerializeField] TMP_Text mouseSensSliderText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EventHandeler.onPlayerDeath += ActivateDeathScreen;
        EventHandeler.onTargetDeath += UpdateScore;
    }

    private void Update()
    {
        if (GameManager.instance.forceLock)
        {
            return;
        }

        if (Input.GetKeyDown("escape") || Input.GetKeyDown("tab"))
        {
           if (activeMenu == pauseMenu)
            {
                MenuOpen(pauseMenu, false, true);
                return;
            }
            else if (!activeMenu)
            {
                MenuOpen(pauseMenu, true, true);
            }
            // if active menu that is not pause menu
            else if (Input.GetKeyDown("escape")) Back();
        }
    }

    public void UpdateComboUI(float activeComboTimer, float comboValue)
    {
        if (activeComboTimer <= 0)
        {
            comboContainer.SetActive(false);
            return;
        }
        comboContainer.SetActive(true);
        comboSlider.value = activeComboTimer;
        comboTextNum.text = (comboValue).ToString();
        comboTextMult.text = (Math.Round(GameManager.instance.comboMult * 10)/10).ToString() + "x";
    }

    public void MenuOpen(GameObject menuToOpen, bool isOpening, bool addNewMenuOpenOrder)
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
            activeMenu = menuToOpen;
        }
        else
        {
            activeMenu = null;
            menuOpenOrder = new List<GameObject>();
            GameManager.instance.Pause(false);
        }

        menuToOpen.SetActive(isOpening);
    }

    void PauseMenu(bool enabled)
    {
        pauseMenu.SetActive(enabled);
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + (Mathf.Round(PlayerControler.instance.roomScore)).ToString();
    }

    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true); 
        activeMenu = deathScreen;
        finalScore.text = "Final " + scoreText.text;
        Timer.SetActive(false);
        scoreText.gameObject.SetActive(false);
       
    }

    public void SettingsUpdate()
    {
        // Round Values
        mouseSensSlider.value = Mathf.Round(mouseSensSlider.value * 100) / 100;

        // Change values
        PlayerControler.instance.mouseSens = mouseSensSlider.value;

        // Update Visual
        mouseSensSliderText.text = mouseSensSlider.value.ToString();
    }

    #region Button Functions
    public void MainMenuOpen()
    {
        SceneManager.Instance.MainMenuOpen();
    }

    public void PauseMenuClose()
    {
        MenuOpen(pauseMenu, false, false);
    }

    public void Back()
    {
        // ^1 is the same as -1 except for some reason it doesn't like -1 so I used ^1
        GameObject lastMenu = menuOpenOrder[^1];
        menuOpenOrder.Remove(lastMenu);
        MenuOpen(lastMenu, true, false);
    }

    public void SettingsMenu()
    {
        MenuOpen(settingsMenu, true, true);
    }

    #endregion
}
