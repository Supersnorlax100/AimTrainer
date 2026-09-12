using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int minutesGiven;
    public int secondsGiven;

    public bool isPaused;
    public bool forceLock;

    public float maxComboTimer = 1;
    public float activeComboTimer;

    public int comboValue;
    public float comboMult;
    public float comboMultDivisor = 10;

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
        EventHandeler.onPlayerDeath += PlayerDie;
        EventHandeler.onTargetDeath += AddScore;
        EventHandeler.onTargetDeath += RefreshCombo;
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
            UiManager.instance.UpdateComboUI(activeComboTimer, comboValue);
        }
    }

    public void RefreshCombo()
    {
        comboValue += 1;
        activeComboTimer = maxComboTimer;
    }

    public void AddScore()
    {
        comboMult = 1 + comboValue/comboMultDivisor;
        PlayerControler.instance.roomScore += PlayerControler.instance.target.GetComponent<TargetScript>().scoreValue * comboMult;
        Debug.Log("room score: " + PlayerControler.instance.roomScore);
    }

    public void PlayerDie()
    {
        Destroy(PlayerControler.instance.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        forceLock = true;
    }

    public void Pause()
    {
        isPaused = !isPaused;
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
}
