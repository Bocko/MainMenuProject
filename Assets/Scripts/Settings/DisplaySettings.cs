using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Assets.Scripts
{
    //modified the original function to only apply settings when the apply button is pressed
    public class DisplaySettings : MonoBehaviour
    {
        [SerializeField] private Volume PostProcessVolume;

        [SerializeField] private TextMeshProUGUI BrightnessTextValue;
        [SerializeField] private Slider BrightnessSlider;
        private ColorAdjustments ColorAdjustments;

        [SerializeField] private Toggle FullScreenModeCheckbox;

        [SerializeField] private Resolution[] Resolutions;
        [SerializeField] private TMP_Dropdown ResolutionDropdown;

        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        private UnityAction applyAction;
        private UnityAction resetAction;

        private void Awake()
        {
            //Brightness:
            BrightnessSlider.onValueChanged.AddListener(SetBrightness);
            PostProcessVolume.profile.TryGet<ColorAdjustments>(out ColorAdjustments); // e miatt toltodik be normalisan

            applyAction = new UnityAction(ApplySettings);
            resetAction = new UnityAction(ResetSettings);

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

            //Add UI valueinitializations here
            LoadPlayerPrefs();
            StartCoroutine(ApplyDisplaySettings());
        }

        private void OnDestroy()
        {
            BrightnessSlider.onValueChanged.RemoveAllListeners();
        }

        private void OnEnable()
        {
            applyButton.onClick.AddListener(applyAction);
            resetButton.onClick.AddListener(resetAction);
        }

        private void OnDisable()
        {
            CancelChanges();

            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);
        }

        private void SetBrightness(float value)
        {
            ColorAdjustments.postExposure.value = (value - 0.5f) * 4f; //min value -2 max value 2
            BrightnessTextValue.text = value.ToString("0.0");
        }

        private void LoadPlayerPrefs()
        {
            //loads playerprefs

            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.LoadIsFullscreen();
            BrightnessSlider.value = SettingsPlayerPrefs.LoadBrightness();
            ResolutionDropdown.value = SettingsPlayerPrefs.LoadResolution();
        }

        private void SavePlayerPrefs()
        {
            //saves playprefs

            SettingsPlayerPrefs.SaveResolution(ResolutionDropdown.value);
            SettingsPlayerPrefs.SaveBrightness(BrightnessSlider.value);
            SettingsPlayerPrefs.SaveIsFullScreen(FullScreenModeCheckbox.isOn);
        }

        private void ResetPlayerPrefs()
        {
            //resets playerprefs

            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.defaultIsFullscreen == 1;
            BrightnessSlider.value = SettingsPlayerPrefs.defaultBrightness;
            ResolutionDropdown.value = SettingsPlayerPrefs.defaultResIndex;
        }

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI AND sets the settings to match the UI
            SavePlayerPrefs();
            StartCoroutine(ApplyDisplaySettings());
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default
            ResetPlayerPrefs();
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
