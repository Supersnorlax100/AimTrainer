using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboScript : MonoBehaviour
{
    [SerializeField] TMP_Text comboTextUI;
    [SerializeField] Slider comboSlider;
    [SerializeField] Image sliderImage;

    bool isPaused;

    public float maxComboTimer = 1;
    float activeComboTimer;

    int comboValue;


    private void Awake()
    {
        EventHandeler.onTargetDeath += RefreshCombo;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isPaused)
        {
            activeComboTimer -= Time.deltaTime;
            if (activeComboTimer > 0) { comboTextUI.text = "Combo: " + (Mathf.Round(activeComboTimer * 100)/100).ToString(); }
            else { comboTextUI.text = "Combo: 0"; sliderImage.enabled = false; }
            comboSlider.value = activeComboTimer;
        }
    }

    public void RefreshCombo()
    {
        activeComboTimer = maxComboTimer;
        sliderImage.enabled = true;
    }

}
