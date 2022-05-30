using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts
{
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

            ResolutionDropdown.onValueChanged.AddListener(SetResolution);
            BrightnessSlider.onValueChanged.AddListener(SetBrightness);
            FullScreenModeCheckbox.onValueChanged.AddListener(SetFullscreenMode);
        }

        private void OnDisable()
        {
            CancelChanges();

            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);

            ResolutionDropdown.onValueChanged.RemoveAllListeners();
            BrightnessSlider.onValueChanged.RemoveAllListeners();
            FullScreenModeCheckbox.onValueChanged.RemoveAllListeners();
        }

        private void SetResolution(int resolutionIndex)
        {
            Resolution currentResoulution = Resolutions[resolutionIndex];
            Screen.SetResolution(currentResoulution.width, currentResoulution.height, Screen.fullScreen);
        }

        private void SetFullscreenMode(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        private void SetBrightness(float value)
        {
            Screen.brightness = value;
            BrightnessTextValue.text = value.ToString("0.0");
        }

        private void LoadPlayerPrefs()
        {
            print("load display");
            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.LoadIsFullscreen() == 1;
            ResolutionDropdown.value = SettingsPlayerPrefs.LoadResolution();
            BrightnessSlider.value = SettingsPlayerPrefs.LoadBrightness();
        }

        private void ApplySettings()
        {
            print("apply display");
            //set playerperfs to values shown on UI
            SettingsPlayerPrefs.SaveResolution(ResolutionDropdown.value);
            SettingsPlayerPrefs.SaveBrightness(BrightnessSlider.value);
            SettingsPlayerPrefs.SaveIsFullScreen(FullScreenModeCheckbox.isOn ? 1 : 0);
        }

        private void ResetSettings()
        {
            print("reset display");
            //ResetSettings settings back to default
            FullScreenModeCheckbox.isOn = SettingsPlayerPrefs.defaultIsFullscreen == 1;
            ResolutionDropdown.value = SettingsPlayerPrefs.defaultResIndex;
            BrightnessSlider.value = SettingsPlayerPrefs.defaultBrightness;
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
            LoadPlayerPrefs();
        }
    }
}
