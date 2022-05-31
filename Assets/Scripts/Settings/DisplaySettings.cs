using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Assets.Scripts
{
    //modified the original function to only apply settings when the apply button is pressed
    public class DisplaySettings : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI BrightnessTextValue;
        [SerializeField] private Slider BrightnessSlider;

        [SerializeField] private Toggle FullScreenModeCheckbox;

        [SerializeField] private Resolution[] Resolutions;
        [SerializeField] private TMP_Dropdown ResolutionDropdown;

        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        private UnityAction applyAction;
        private UnityAction resetAction;

        private void Awake()
        {
            applyAction = new UnityAction(ApplySettings);
            resetAction = new UnityAction(ResetSettings);
        }

        private void Start()
        {
            //Resolution:
            Resolutions = Screen.resolutions;

            ResolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < Resolutions.Length; i++)
            {
                string option = Resolutions[i].width + " x " + Resolutions[i].height + " - " + Resolutions[i].refreshRate + "hz";
                options.Add(option);

                if (Resolutions[i].width == Screen.currentResolution.width &&
                    Resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            ResolutionDropdown.AddOptions(options);
            ResolutionDropdown.value = currentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();

            //Add UI valueinitializations here (should be form Default Stetting / Player Preferences / Currently applied options)
            LoadPlayerPrefs();
        }

        private void OnEnable()
        {
            applyButton.onClick.AddListener(applyAction);
            resetButton.onClick.AddListener(resetAction);

            BrightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        private void OnDisable()
        {
            CancelChanges();

            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);

            BrightnessSlider.onValueChanged.RemoveAllListeners();
        }

        private void SetBrightness(float value)
        {
            BrightnessTextValue.text = value.ToString("0.0");
        }

        private void LoadPlayerPrefs()
        {
            //loads playerprefs and applys them

            print("load display");

            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.LoadIsFullscreen() == 1;
            BrightnessSlider.value = SettingsPlayerPrefs.LoadBrightness();
            ResolutionDropdown.value = SettingsPlayerPrefs.LoadResolution();

            StartCoroutine(ApplyDisplaySettings());
        }

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI AND sets the settings to match the UI

            print("apply display");

            StartCoroutine(ApplyDisplaySettings());

            SettingsPlayerPrefs.SaveResolution(ResolutionDropdown.value);
            SettingsPlayerPrefs.SaveBrightness(BrightnessSlider.value);
            SettingsPlayerPrefs.SaveIsFullScreen(FullScreenModeCheckbox.isOn ? 1 : 0);
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default

            print("reset display");
            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.defaultIsFullscreen == 1;
            BrightnessSlider.value = SettingsPlayerPrefs.defaultBrightness;
            ResolutionDropdown.value = SettingsPlayerPrefs.defaultResIndex;

            ApplySettings();
        }

        private void CancelChanges()
        {
            //canceling when switching tabs
            LoadPlayerPrefs();
        }

        IEnumerator ApplyDisplaySettings()//changing settings based on ui
        {
            //this coroutine is needed because for some reason you cant change the fullscreen variable and resolution in the same frame
            //https://answers.unity.com/questions/765780/screensetresolution-and-full-screen-not-behaving-c.html

            Screen.fullScreen = FullScreenModeCheckbox.isOn;
            Screen.brightness = BrightnessSlider.value;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Resolution currentResoulution = Resolutions[ResolutionDropdown.value];
            Screen.SetResolution(currentResoulution.width, currentResoulution.height, Screen.fullScreen, currentResoulution.refreshRate);
        }
    }
}
