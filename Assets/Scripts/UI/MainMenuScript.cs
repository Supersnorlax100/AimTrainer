using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    // Kinda a copy of UiManager but for main menu stuff
    public GameObject settingsMenu;
    public GameObject mainMenu;
    [SerializeField] Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(GameManager.instance.GameStart);
        UiManager.instance.mainMenuScript = GetComponent<MainMenuScript>();
    }

    public void Close()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
