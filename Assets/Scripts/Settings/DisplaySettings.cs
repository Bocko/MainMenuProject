using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DisplaySettings : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI BrightnessTextValue;
        [SerializeField] private Slider BrightnessSlider;

        [SerializeField] private Toggle FullScreenModeCheckbox;

        [SerializeField] private Resolution[] Resolutions;
        [SerializeField] private TMP_Dropdown ResolutionDropdown;

        private void Start()
        {
            //Resolution:
            Resolutions = Screen.resolutions;

            ResolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < Resolutions.Length; i++)
            {
                string option = Resolutions[i].width + " x " + Resolutions[i].height;
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
        }

        private void OnEnable()
        {
            ResolutionDropdown.onValueChanged.AddListener(SetResolution);
            BrightnessSlider.onValueChanged.AddListener(SetBrightness);
            FullScreenModeCheckbox.onValueChanged.AddListener(SetFullscreenMode);
        }

        private void OnDisable()
        {
            ResolutionDropdown.onValueChanged.RemoveListener(SetResolution);
            BrightnessSlider.onValueChanged.RemoveListener(SetBrightness);
            FullScreenModeCheckbox.onValueChanged.RemoveListener(SetFullscreenMode);
        }

        private void SetResolution(int resolutionIdenx)
        {
            Resolution currentResoulution = Resolutions[resolutionIdenx];
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

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }
}
