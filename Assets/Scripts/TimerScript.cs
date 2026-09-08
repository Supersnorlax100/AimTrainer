using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] int minutesGiven;
    [SerializeField] int secondsGiven;
    [SerializeField] TMP_Text timerTextUI;

    int seconds;
    int minutes;
    int centiseconds =0 ;
    string[] timerText = new string[4];

    void Start()
    {
        seconds = secondsGiven;
        minutes = minutesGiven;
        centiseconds = 0;
        UpdateTimer();
        InvokeRepeating("UpdateTimer", 0.01f, 0.01f);
        UpdateText();
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
                --minutes;
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

        UpdateText();
    }

    void UpdateText()
    {
        timerText[0] = "Timer: ";
        timerText[1] = minutes.ToString("00");
        timerText[2] = seconds.ToString("00");
        timerText[3] = centiseconds.ToString("00");
        timerTextUI.text = timerText[0] + timerText[1] + ":" + timerText[2] + ":" + timerText[3];
    }
}
