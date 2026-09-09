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
            comboSlider.value = activeComboTimer;
            activeComboTimer -= Time.deltaTime;
            if (activeComboTimer > 0) 
            { 
                comboTextUI.text = "Combo: " + (comboValue).ToString(); 
            }
            else 
            { 
                comboTextUI.text = "";
                comboValue = 0;
                sliderImage.enabled = false; 
            }
        }
    }

    public void RefreshCombo()
    {
        comboValue += 1;
        activeComboTimer = maxComboTimer;
        sliderImage.enabled = true;
    }

}
