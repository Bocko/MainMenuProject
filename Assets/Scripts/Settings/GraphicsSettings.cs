using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

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

        private void Start()
        {
            PostProcessVolume.profile.TryGetSettings(out AmbientOcclusion);
            PostProcessVolume.profile.TryGetSettings(out MotionBlur);

            //UI valueinitializations here (should be form Default Stetting / Player Preferences / autodetect / Currently applied options)
        }

        private void OnEnable()
        {
            AmbientOcclusionDorpdown.onValueChanged.AddListener(SetAmbientOcclusionQuality);
            AntiAliasingDropdown.onValueChanged.AddListener(SetAnitAliasing);
            MotionBlurCheckbox.onValueChanged.AddListener(SetMotionBlur);
            QualityDropdown.onValueChanged.AddListener(SetQuality);
            TargetFrameRateSlider.onValueChanged.AddListener(SetTargetFrameRate);
        }

        private void OnDisable()
        {
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
                    AmbientOcclusion.quality.value = UnityEngine.Rendering.PostProcessing.AmbientOcclusionQuality.Lowest;
                    break;
                case 2:
                    AmbientOcclusion.quality.value = UnityEngine.Rendering.PostProcessing.AmbientOcclusionQuality.Low;
                    break;
                case 3:
                    AmbientOcclusion.quality.value = UnityEngine.Rendering.PostProcessing.AmbientOcclusionQuality.Medium;
                    break;
                case 4:
                    AmbientOcclusion.quality.value = UnityEngine.Rendering.PostProcessing.AmbientOcclusionQuality.High;
                    break;
                case 5:
                    AmbientOcclusion.quality.value = UnityEngine.Rendering.PostProcessing.AmbientOcclusionQuality.Ultra;
                    break;
            }
        }

        private void SetMotionBlur(bool on)
        {
            MotionBlur.active = on;
        }

        private void ApplySettings()
        {
            //for graphic settings onclick methods should only save the values, then call the current set methods here
        }

        private void ResetSettings()
        {
            //ResetSettings settings back to default
        }

        private void CancelChanges()
        {
            //set the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }
}
