using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [SerializeField] Slider mouseSensSlider;
    [SerializeField] TMP_Text mouseSensSliderText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

}
