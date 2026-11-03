using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public Slider mouseSensSlider;
    [SerializeField] TMP_Text mouseSensSliderText;

    [SerializeField] Slider countdownSlider;
    [SerializeField] TMP_Text countdownSliderText;

    void Start()
    {
        mouseSensSlider.value = 1;
        countdownSlider.value = 3;

        SettingsUpdate();
    }

    public void SettingsUpdate()
    {
        // Round Values
        SettingsManager.instance.mouseSens = Mathf.Round(mouseSensSlider.value * 100) / 100;
        SettingsManager.instance.countdownTime = Mathf.RoundToInt(countdownSlider.value); // Should always be an int

        // Change values
        SettingsManager.instance.UpdateSettings();

        // Update Visual
        mouseSensSliderText.text = SettingsManager.instance.mouseSens.ToString();
        countdownSliderText.text = SettingsManager.instance.countdownTime.ToString();
    }

}
