using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] TMP_Text timerTextUI;

    int minutes;
    int seconds;
    int centiseconds;

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
        InvokeRepeating("UpdateTimer", 0.01f, 0.01f);
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
}
