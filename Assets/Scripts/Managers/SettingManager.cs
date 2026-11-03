using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public float mouseSens = 1;
    public int countdownTime = 3;

    void Awake()
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
    }

    void Start()
    {
        UpdateSettings();
    }

    public void UpdateSettings()
    {
        if (PlayerControler.instance) PlayerControler.instance.mouseSens = mouseSens;
        UiManager.instance.timerScript.countdownTime = countdownTime;
    }
}
