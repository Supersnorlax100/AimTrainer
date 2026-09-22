using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    // Kinda a copy of UiManager but for main menu stuff
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenu;

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
}
