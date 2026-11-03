using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public float mouseSens;
    public int countdownTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateSettings()
    {
        if (PlayerControler.instance) PlayerControler.instance.mouseSens = mouseSens;
        UiManager.instance.timerScript.countdownTimer = countdownTime;
    }
}
