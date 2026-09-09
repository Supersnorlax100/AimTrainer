using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public bool isPaused;
    public bool forceLock;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject Combo;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] GameObject pauseMenu;

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
        if (Input.GetKeyDown("escape") && !forceLock)
        {
            Pause(!isPaused);
            PauseMenu(isPaused);
        }
    }

    public void Pause(bool pause)
    {
        isPaused = pause;
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

    void PauseMenu(bool enabled)
    {
        pauseMenu.SetActive(enabled);
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + PlayerControler.instance.score.ToString();
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
        Pause(false);
    }

    #endregion
}
