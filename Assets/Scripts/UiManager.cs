using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField] GameObject Timer;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;

    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject pauseMenu;

// Combo stuff
    [SerializeField] TMP_Text comboTextNum;
    [SerializeField] TMP_Text comboTextMult;
    [SerializeField] Slider comboSlider;
    [SerializeField] GameObject comboContainer;

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
        if (Input.GetKeyDown("escape") && !GameManager.instance.forceLock)
        {
            GameManager.instance.Invoke("Pause", 0);
            PauseMenu(GameManager.instance.isPaused);
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
        DeathScreen.SetActive(true); 
        finalScore.text = "Final " + scoreText.text;
        Timer.SetActive(false);
        scoreText.gameObject.SetActive(false);
       
    }

    #region Button Functions
    public void MainMenuOpen()
    {
        SceneManager.Instance.MainMenuOpen();
    }

    public void PauseMenuClose()
    {
        PauseMenu(false);
        GameManager.instance.Invoke("Pause", 0);
    }

    #endregion
}
