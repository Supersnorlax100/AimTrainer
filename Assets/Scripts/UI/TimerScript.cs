using UnityEngine;
using TMPro;
using System;
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


    int bonusMinutes;
    int bonusSeconds;
    int bonusCentiseconds;

    public int countdownTimer;

    void Awake()
    {
        // EventHandeler.exitRoom += StoreTime;
        EventHandeler.enterRoom += Timer;

    }

    public void Timer()
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
        RoomType _curRoomType = MapManager.instance.currentRoomType;
        if (_curRoomType == RoomType.COMBAT || _curRoomType == RoomType.MINIBOSS || _curRoomType == RoomType.BOSS)
        {
            GameManager.instance.ForceLock(true);
            countdownTimer++; // if 3, becomes 4 so when starting it shows 3 since it subtracts on the start of the function
            InvokeRepeating("CountdownTimer", 0, 1);
        }

        // Sets bonus time values
        if(GameManager.instance.timeIncreaseStat > 0 || GameManager.instance.timeMultStat >0)
        {
            int bonusTime = (int)(GameManager.instance.timeIncreaseStat + (GameManager.instance.curTime * (1 + GameManager.instance.timeMultStat)));
            if (bonusTime >= 100)
            {
                bonusSeconds = Mathf.FloorToInt(bonusTime / 100);
                bonusCentiseconds = bonusTime - (bonusSeconds * 100);
            }
            if (bonusSeconds >= 60)
            {
                bonusMinutes = Mathf.FloorToInt(bonusSeconds / 60);
            }
            bonusSeconds = bonusSeconds - (bonusMinutes * 60);
        }
    }

    public void StoreTime()
    {
        // Debug.Log("what store, timescript time: " + ((minutes * 60) + seconds) * 100 + centiseconds);
        // Debug.Log("store, time: " + GameManager.instance.curTime);
        GameManager.instance.curTime = ((minutes * 60) + seconds) * 100 + centiseconds;
        // Debug.Log("after store: " + GameManager.instance.curTime);
    }

    private void UpdateTimer()
    {
        if (GameManager.instance.timeIncreaseStat > 0 || GameManager.instance.timeMultStat > 0)
        {
            if (bonusSeconds <= 0 && bonusMinutes <= 0 && bonusCentiseconds <= 0)
            {
                CancelInvoke("UpdateTimer");
                EventHandeler.onPlayerDeath?.Invoke();
            }
            if (bonusCentiseconds <= 0)
            {
                if (bonusSeconds <= 0)
                {
                    bonusMinutes--;
                    bonusSeconds = 59;
                    bonusCentiseconds = 99;
                }
                else
                {
                    bonusSeconds--;
                    bonusCentiseconds = 99;
                }
            }
            else
            {
                bonusCentiseconds--;
            }
            timerTextUI.text = string.Format("{0:00}:{1:00}:{2:00}", bonusMinutes, bonusSeconds, bonusCentiseconds);
        }
        else
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
            GameManager.instance.curTime--;
            timerTextUI.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
        }

    }

    public void CountdownTimer() 
    {
        countDownScreen.SetActive(true);
        crosshair.SetActive(true);
        ComboObj.SetActive(false);
        TimerValueObj.SetActive(false);
        ScoreObj.SetActive(false);
        
        countdownTimer--;
        if (countdownTimer >= 3)
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
            countdownTimer = 3 + 1;
            CancelInvoke("CountdownTimer");
        }
    }
}
