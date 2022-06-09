using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    enum SettingsPage { CONTROLS, AUDIO, DISPLAY, GRAPHICS}

    public GameObject[] buttonBackgrounds;
    public GameObject[] settingsPageHolders;

    SettingsPage currentSettingsPage;

    [SerializeField] private GameObject settingsHolder;

    void Start()
    {
        LoadPreferences();
        currentSettingsPage = SettingsPage.CONTROLS;
        SetCurrentButtonBackground();
        SetCurrentSettingsPage();
    }

    public void ChangeSettingsPage(int pageIndex)
    {
        currentSettingsPage = (SettingsPage)pageIndex;
        SetCurrentButtonBackground();
        SetCurrentSettingsPage();
    }

    void SetCurrentButtonBackground()
    {
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            buttonBackgrounds[i].SetActive(i == (int)currentSettingsPage);
        }
    }

    void SetCurrentSettingsPage() 
    {
        for (int i = 0; i < settingsPageHolders.Length; i++)
        {
            settingsPageHolders[i].SetActive(i == (int)currentSettingsPage);
        }
    }

    private void LoadPreferences()
    //this method is used to call each Settings' awake method on game start
    {
        settingsHolder.SetActive(true);
        for (int i = 0; i < settingsPageHolders.Length; i++)
        {
            settingsPageHolders[i].SetActive(true);
            settingsPageHolders[i].SetActive(false);
        }
        settingsHolder.SetActive(false);
    }
}
