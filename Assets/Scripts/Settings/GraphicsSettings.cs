using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Assets.Scripts
{
    //modified the original function to only apply settings when the apply button is pressed
    public class GraphicsSettings : MonoBehaviour
    {
        //https://docs.unity.cn/Packages/com.unity.render-pipelines.universal@13.1/api/UnityEngine.Rendering.Universal.html
        [SerializeField] private Volume PostProcessVolume;
        [SerializeField] private UniversalRenderPipelineAsset[] QualityPipeLines;
        private int CurrentPipeLineIndex;

        //URP implements the Ambient Occlusion effect as a Renderer Feature:
        //https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@10.0/manual/post-processing-ssao.html
        [SerializeField] private TMP_Dropdown AmbientOcclusionDorpdown;
        [SerializeField] private ForwardRendererData ForwardRendererData;
        private ScriptableRendererFeature AmbientOcclusion; //GraphicsSettings currently supports: SSAO: Off / Depth Normal     (available options for URP: //SSAO: off / low / medium / high / Depth Normals)
        //Cant change runtime due to its protection level: (ScreenSpaceAmbientOcclusionEditor / ScreenSpaceAmbientOcclusion)
        //To use all available options, use something like this: https://forum.unity.com/threads/change-ssao-render-feature-at-runtime.1163959/   or setup 3-4 forward render pipeline assets and chenge between those

        //Overrides the data in the current pipeline, and keeps the changes after each game reaload (most be loaded after each Quality change to override the loaded pipeline's MSAA)
        [SerializeField] private TMP_Dropdown AntiAliasingDropdown;//MSAA: off / 2x, 4x / 8x -- Render Pipeline => MSAA

        [SerializeField] private TMP_Dropdown MotionBlurDropDown;
        private MotionBlur MotionBlur; // off, low, medium, high (Post process volume)

        //set up each quality settings in project settings and for each URP asset (!!!GraphicsSettings currently overrides VSycn Count and Anti Aliasing Count)
        [SerializeField] private TMP_Dropdown QualityDropdown;

        [SerializeField] private TextMeshProUGUI TargetFrameRateValueText;
        [SerializeField] private Slider TargetFrameRateSlider;

        //Bacause it can be seen in the projectSettings => Quality , this setting resets (for each quality) after each game start (so it can be handeled independently from the Pipelines if it's loaded correctly)
        [SerializeField] private Toggle VSyncCheckbox;

        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        private UnityAction applyAction;
        private UnityAction resetAction;

        private void Awake()
        {
            TargetFrameRateSlider.onValueChanged.AddListener(SetTargetFrameRateUI);
            QualityDropdown.onValueChanged.AddListener(SetCurrentPipeLine);

            PostProcessVolume.profile.TryGet<MotionBlur>(out MotionBlur);

            applyAction = new UnityAction(ApplySettings);
            resetAction = new UnityAction(ResetSettings);

            var x = ForwardRendererData.rendererFeatures;
            foreach (var item in x)
            {
                if (item.name == "SSAO")
                {
                    AmbientOcclusion = item;
                }
            }

            LoadPlayerPrefs();
            ApplyGraphicsSettings();
        }

        private void OnDestroy()
        {
            TargetFrameRateSlider.onValueChanged.RemoveListener(SetTargetFrameRateUI);
            QualityDropdown.onValueChanged.RemoveListener(SetCurrentPipeLine);
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

        private void SetCurrentPipeLine(int index)
        {
            CurrentPipeLineIndex = index;
        }

        private void SetQuality(int indexFromDropdown)
        {
            QualitySettings.SetQualityLevel(indexFromDropdown);
        }

        private void SetTargetFrameRateUI(float volume)
        {
            TargetFrameRateValueText.text = volume > 200 ? "Unlimited" : volume.ToString();
        }

        private void SetTargetFrameRate(float volume)
        {
            Application.targetFrameRate = (int)(volume > 200 ? -1 : volume); //-1 equals unlimited | volume is always an int because Slider.WholeNumbers == true 
        }

        private void SetAnitAliasing(int index)
        {
            // Disabled = 1,
            // valid numbers: 1, 2, 4, 8
            var currentPipe = QualityPipeLines[CurrentPipeLineIndex];
            switch (index)
            {
                case 0:
                    currentPipe.msaaSampleCount = 1;
                    break;
                case 1:
                    currentPipe.msaaSampleCount = 2;
                    break;
                case 2:
                    currentPipe.msaaSampleCount = 4;
                    break;
                case 3:
                    currentPipe.msaaSampleCount = 8;
                    break;
            }
        }

        private void SetAmbientOcclusionQuality(int index)
        {
            //Can not be cahnged in a "Unity Supported Way" due to inaccessibility (only can be diasbaled)
            if (index == 0)
            {
                AmbientOcclusion.SetActive(false);
            }
            else
            {
                AmbientOcclusion.SetActive(true);
            }
        }

        private void SetMotionBlur(int index)
        {
            switch (index)
            {
                case 0:
                    MotionBlur.quality = new MotionBlurQualityParameter(MotionBlurQuality.Low, true);
                    break;
                case 1:
                    MotionBlur.quality = new MotionBlurQualityParameter(MotionBlurQuality.Medium, true);
                    break;
                case 2:
                    MotionBlur.quality = new MotionBlurQualityParameter(MotionBlurQuality.High, true);
                    break;
            }
        }

        private void SetVSync(bool on)
        {
            QualitySettings.vSyncCount = on ? 1 : 0;
        }

        private void LoadPlayerPrefs()
        {
            //loads graphics

            TargetFrameRateSlider.value = SettingsPlayerPrefs.LoadFramerate();
            QualityDropdown.value = SettingsPlayerPrefs.LoadQuality();
            AntiAliasingDropdown.value = SettingsPlayerPrefs.LoadAntiAliasing();
            AmbientOcclusionDorpdown.value = SettingsPlayerPrefs.LoadAmbientOcclusion();
            MotionBlurDropDown.value = SettingsPlayerPrefs.LoadMotionBlur();
            VSyncCheckbox.isOn = SettingsPlayerPrefs.LoadVSync();
        }

        private void SavePlayerPrefs()
        {
            //saves graphics

            SettingsPlayerPrefs.SaveFramerate((int)TargetFrameRateSlider.value);
            SettingsPlayerPrefs.SaveQuality(QualityDropdown.value);
            SettingsPlayerPrefs.SaveAntiAliasing(AntiAliasingDropdown.value);
            SettingsPlayerPrefs.SaveAmbientOcclusion(AmbientOcclusionDorpdown.value);
            SettingsPlayerPrefs.SaveMotionBlur(MotionBlurDropDown.value);
            SettingsPlayerPrefs.SaveVSync(VSyncCheckbox.isOn);
        }

        private void ResetPlayerPrefs()
        {
            //resets graphics

            TargetFrameRateSlider.value = SettingsPlayerPrefs.defaultFramerate;
            QualityDropdown.value = SettingsPlayerPrefs.defaultQualityIndex;
            AntiAliasingDropdown.value = SettingsPlayerPrefs.defaultAntiAliasingIndex;
            AmbientOcclusionDorpdown.value = SettingsPlayerPrefs.defaultAmbientOcclusionIndex;
            MotionBlurDropDown.value = SettingsPlayerPrefs.defaultMotionBlurIndex;
            VSyncCheckbox.isOn = SettingsPlayerPrefs.defaultVSync == 1;
        }

        private void ApplySettings()
        {
            //for graphic settings onclick methods should only save the values, then call the current set methods here

            SavePlayerPrefs();
            ApplyGraphicsSettings();
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default

            ResetPlayerPrefs();
            ApplySettings();
        }

        private void CancelChanges()
        {
            //set the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
            LoadPlayerPrefs();
        }

        private void ApplyGraphicsSettings()
        {
            SetTargetFrameRate(TargetFrameRateSlider.value);
            SetQuality(QualityDropdown.value);
            SetAnitAliasing(AntiAliasingDropdown.value);
            SetAmbientOcclusionQuality(AmbientOcclusionDorpdown.value);
            SetMotionBlur(MotionBlurDropDown.value);
            SetVSync(VSyncCheckbox.isOn);
        }
    }
}
