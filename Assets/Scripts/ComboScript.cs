using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboScript : MonoBehaviour
{
    [SerializeField] TMP_Text comboTextUI;
    [SerializeField] Slider comboSlider;
    [SerializeField] GameObject comboContainer;

    public float maxComboTimer = 1;
    float activeComboTimer;

    int comboValue;


    private void Awake()
    {
        EventHandeler.onTargetDeath += RefreshCombo;
    }

    private void Update()
    {
        if (! UiManager.instance.isPaused)
        {
            comboSlider.value = activeComboTimer;
            activeComboTimer -= Time.deltaTime;
            if (activeComboTimer > 0) 
            {
                comboTextUI.text = (comboValue).ToString(); 
            }
            else 
            {
                comboContainer.SetActive(false);
                comboValue = 0;
            }
        }
    }

    public void RefreshCombo()
    {
        comboValue += 1;
        activeComboTimer = maxComboTimer;
        comboContainer.SetActive(true);
    }

}
