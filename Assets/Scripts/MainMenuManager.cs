using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuPage { MAIN, SETTINGS, CREDITS, CONFIRMEXIT }

    public TextMeshProUGUI titleText;
    public GameObject mainMenuHolder;
    public GameObject confirmExitHolder;
    public GameObject settingsHolder;

    public string mainMenuTitle = "MAIN MENU";
    public string settingsTitle = "SETTINGS";
    string confirmExitTitle = "YOU SURE???";

    MenuPage currentPage;

    void Start()
    {
        currentPage = MenuPage.MAIN;
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentPage)
            {
                case MenuPage.MAIN:
                {
                    ShowConfirmExit();
                    break;
                }
                case MenuPage.SETTINGS:
                {
                    SettingsToMenu();
                    break;
                }
                case MenuPage.CONFIRMEXIT:
                {
                    HideConfirmExit();
                    break;
                }
            }
        }
    }

    public void MenuToSettings()
    {
        titleText.text = settingsTitle;
        currentPage = MenuPage.SETTINGS;
        mainMenuHolder.SetActive(false);
        settingsHolder.SetActive(true);
    }

    public void SettingsToMenu()
    {
        titleText.text = mainMenuTitle;
        currentPage = MenuPage.MAIN;
        settingsHolder.SetActive(false);
        mainMenuHolder.SetActive(true);
    }

    public void ShowConfirmExit()
    {
        titleText.text = confirmExitTitle;
        currentPage = MenuPage.CONFIRMEXIT;
        confirmExitHolder.SetActive(true);
    }

    public void HideConfirmExit()
    {
        titleText.text = mainMenuTitle;
        currentPage = MenuPage.MAIN;
        confirmExitHolder.SetActive(false);
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }
}
