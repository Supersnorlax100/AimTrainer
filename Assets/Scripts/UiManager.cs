using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] TMP_Text finalScore;
    private void Awake()
    {
        EventHandeler.onPlayerDeath += ActivateDeathScreen;
    }


    public void ActivateUi()
    {

    }

    public void ActivateDeathScreen()
    {
        DeathScreen.SetActive(true); 
        finalScore.text = "Final " + scoreText.text;
        Timer.SetActive(false);
        scoreText.gameObject.SetActive(false);
       
    }

}
