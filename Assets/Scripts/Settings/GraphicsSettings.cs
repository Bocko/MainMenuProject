using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GraphicsSettings : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume PostProcessVolume; //should be a global / the game's prost process volume

        [SerializeField] private TMP_Dropdown AmbientOcclusionDorpdown;
        private AmbientOcclusion AmbientOcclusion; //off / low / medium / high

        [SerializeField] private TMP_Dropdown AntiAliasingDropdown;
        //private int AntiAliasing; //MSAA: off / 2x, 4x / 8x -- QualitySettings.antiAliasing

        [SerializeField] private Toggle MotionBlurCheckbox;
        private MotionBlur MotionBlur; //on / off

        [SerializeField] private TMP_Dropdown QualityDropdown;
        private int Quality; // form project settings or
        //private int TextureQuality; // if quality is from project settings this should be removed

        [SerializeField] private TextMeshProUGUI TargetFrameRateValueText;
        [SerializeField] private Slider TargetFrameRateSlider;

        //egyebb lehet: pl shadows / QualitySettings.anisotropicFiltering

        /* Overall Quaility should be set like:

            QualitySettings.SetQualityLevel(index)
            Should be the same as: Edit < Project Settings < Quality values

        OR

            Set playerPreferences on quality change manually

        */

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
            PostProcessVolume.profile.TryGetSettings(out AmbientOcclusion);
            PostProcessVolume.profile.TryGetSettings(out MotionBlur);

            //UI valueinitializations here (should be form Default Stetting / Player Preferences / autodetect / Currently applied options)
            LoadPlayerPrefs();
        }

        private void OnEnable()
        {
            applyButton.onClick.AddListener(applyAction);
            resetButton.onClick.AddListener(resetAction);

            AmbientOcclusionDorpdown.onValueChanged.AddListener(SetAmbientOcclusionQuality);
            AntiAliasingDropdown.onValueChanged.AddListener(SetAnitAliasing);
            MotionBlurCheckbox.onValueChanged.AddListener(SetMotionBlur);
            QualityDropdown.onValueChanged.AddListener(SetQuality);
            TargetFrameRateSlider.onValueChanged.AddListener(SetTargetFrameRate);
        }

        private void OnDisable()
        {
            CancelChanges();

            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);

            AmbientOcclusionDorpdown.onValueChanged.RemoveListener(SetAmbientOcclusionQuality);
            AntiAliasingDropdown.onValueChanged.RemoveListener(SetAnitAliasing);
            MotionBlurCheckbox.onValueChanged.RemoveListener(SetMotionBlur);
            QualityDropdown.onValueChanged.RemoveListener(SetQuality);
            TargetFrameRateSlider.onValueChanged.RemoveListener(SetTargetFrameRate);
        }

        private void SetQuality(int indexFromDropdown)
        {
            QualitySettings.SetQualityLevel(indexFromDropdown);
        }

        private void SetTargetFrameRate(float volume)
        {
            if (volume > 200)
            {
                Application.targetFrameRate = -1;    //-1 equals unlimited
                TargetFrameRateValueText.text = "Unlimited";
                return;
            }
            Application.targetFrameRate = (int)volume; //volume is always an int because Slider.WholeNumbers == true 
            TargetFrameRateValueText.text = volume.ToString();
        }

        private void SetAnitAliasing(int index)
        {
            //valid numbers: 0, 2, 4, 8
            switch (index)
            {
                case 0:
                    QualitySettings.antiAliasing = 0;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 2;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 3:
                    QualitySettings.antiAliasing = 8;
                    break;
            }
        }

        private void SetAmbientOcclusionQuality(int index)
        {
            if (index != 0)
            {
                AmbientOcclusion.active = true;
            }
            switch (index)
            {
                case 0:
                    AmbientOcclusion.active = false;
                    break;
                case 1:
                    AmbientOcclusion.quality.value = AmbientOcclusionQuality.Lowest;
                    break;
                case 2:
                    AmbientOcclusion.quality.value = AmbientOcclusionQuality.Low;
                    break;
                case 3:
                    AmbientOcclusion.quality.value = AmbientOcclusionQuality.Medium;
                    break;
                case 4:
                    AmbientOcclusion.quality.value = AmbientOcclusionQuality.High;
                    break;
                case 5:
                    AmbientOcclusion.quality.value = AmbientOcclusionQuality.Ultra;
                    break;
            }
        }

        private void SetMotionBlur(bool on)
        {
            MotionBlur.active = on;
        }

        private void LoadPlayerPrefs()
        {
            TargetFrameRateSlider.value = SettingsPlayerPrefs.LoadFramerate();
            QualityDropdown.value = SettingsPlayerPrefs.LoadQuality();
            AntiAliasingDropdown.value = SettingsPlayerPrefs.LoadAntiAliasing();
            AmbientOcclusionDorpdown.value = SettingsPlayerPrefs.LoadAmbientOcclusion();
            MotionBlurCheckbox.isOn = SettingsPlayerPrefs.LoadMotionBlur();
        }

        private void ApplySettings()
        {
            //for graphic settings onclick methods should only save the values, then call the current set methods here
            print("graphics apply");

            SettingsPlayerPrefs.SaveFramerate((int)TargetFrameRateSlider.value);
            SettingsPlayerPrefs.SaveQuality(QualityDropdown.value);
            SettingsPlayerPrefs.SaveAntiAliasing(AntiAliasingDropdown.value);
            SettingsPlayerPrefs.SaveAmbientOcclusion(AmbientOcclusionDorpdown.value);
            SettingsPlayerPrefs.SaveMotionBlur(MotionBlurCheckbox.isOn);
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default
            print("graphics reset");

            TargetFrameRateSlider.value = SettingsPlayerPrefs.defaultFramerate;
            QualityDropdown.value = SettingsPlayerPrefs.defaultQualityIndex;
            AntiAliasingDropdown.value = SettingsPlayerPrefs.defaultAntiAliasingIndex;
            AmbientOcclusionDorpdown.value = SettingsPlayerPrefs.defaultAmbientOcclusionIndex;
            MotionBlurCheckbox.isOn = SettingsPlayerPrefs.defaultMotionBlur == 1;
        }

        private void CancelChanges()
        {
            //set the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
            LoadPlayerPrefs();
        }
    }
}
