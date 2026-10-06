using UnityEngine;
using TMPro;
using System.Threading.Tasks;
public class TimerScript : MonoBehaviour
{
    [SerializeField] TMP_Text timerTextUI;
    [SerializeField] TMP_Text countdownTimerText;
    [SerializeField] GameObject countDownScreen;

    // Combat UI
    [SerializeField] GameObject crosshair; // Want to keep this part of combat UI
    [SerializeField] GameObject TimerValueObj;
    [SerializeField] GameObject ScoreObj;
    [SerializeField] GameObject ComboObj;

    int minutes;
    int seconds;
    int centiseconds;

    int countdownTimer = 4;

    void Awake()
    {
        EventHandeler.exitRoom += StoreTime;
    }

    void Start()
    {
        int time = GameManager.instance.curTime;

        if (time >= 100)
        {
            seconds = Mathf.FloorToInt(time/100);
            centiseconds = time - (seconds * 100);
        }
        if (seconds >= 60)
        {
             minutes = Mathf.FloorToInt(seconds/60);
        }
        seconds = seconds - (minutes * 60);
        UpdateTimer();

        GameManager.instance.ForceLock(true);
        InvokeRepeating("CountdownTimer", 0, 1);
    }

    public void StoreTime()
    {
        GameManager.instance.curTime = ((minutes * 60) + seconds) * 100 + centiseconds;
    }


    private void UpdateTimer()
    {

        if (seconds <= 0 && minutes <= 0 && centiseconds <= 0)
        {
            CancelInvoke("UpdateTimer");
            EventHandeler.onPlayerDeath?.Invoke();
        }
        if (centiseconds <= 0)
        {
            if (seconds <= 0)
            {
                minutes--;
                seconds = 59;
                centiseconds = 99;
            }
            else
            {
                seconds--;
                centiseconds = 99;
            }
        }
        else
        {
            centiseconds--;
        }

        // UpdateText();
        timerTextUI.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }

    public void CountdownTimer()
    {
        countDownScreen.SetActive(true);
        crosshair.SetActive(true);
        ComboObj.SetActive(false);
        TimerValueObj.SetActive(false);
        ScoreObj.SetActive(false);
        
        countdownTimer--;
        if (countdownTimer == 3)
        {
            countdownTimerText.color = Color.red;
        }
        if (countdownTimer == 2)
        {
            countdownTimerText.color = Color.yellow;
        }
        if (countdownTimer == 1)
        {
            countdownTimerText.color = Color.green;
        }
        countdownTimerText.text = countdownTimer.ToString();
        if (countdownTimer <= 0)
        {
            GameManager.instance.ForceLock(false);
            countDownScreen.SetActive(false);
            ComboObj.SetActive(true);
            TimerValueObj.SetActive(true);
            ScoreObj.SetActive(true);
            InvokeRepeating("UpdateTimer", 0.01f, 0.01f);
            CancelInvoke("CountdownTimer");
        }
    }
}
