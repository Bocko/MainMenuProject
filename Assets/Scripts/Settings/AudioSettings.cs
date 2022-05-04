using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AudioSettings : MonoBehaviour
    {
        //Add audiomixer form the game
        [SerializeField] private AudioMixer AudioMixer = null;

        //assign audio sources' outputs to audio mixer groups
        [SerializeField] private TextMeshProUGUI MasterVolumeTextValue;
        [SerializeField] private Slider MasterVolumeSlider;

        [SerializeField] private TextMeshProUGUI MusicVolumeTextValue;
        [SerializeField] private Slider MusicVolumeSlider;

        [SerializeField] private TextMeshProUGUI EffectsVolumeTextValue;
        [SerializeField] private Slider EffectsVolumeSlider;

        private void Start()
        {
            //UI valueinitializations here (should be form Default Stetting / Player Preferences / Currently applied options)
            MasterVolumeTextValue.text = "100";
            MusicVolumeTextValue.text = "100";
            EffectsVolumeTextValue.text = "100";
        }

        private void OnEnable()
        {
            MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            SetMasterVolume(MasterVolumeSlider.value);

            MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            SetMasterVolume(MusicVolumeSlider.value);

            EffectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
            SetMasterVolume(EffectsVolumeSlider.value);
        }

        private void OnDisable()
        {
            MasterVolumeSlider.onValueChanged.RemoveAllListeners();
            MusicVolumeSlider.onValueChanged.RemoveAllListeners();
            EffectsVolumeSlider.onValueChanged.RemoveAllListeners();
        }

        private void SetMasterVolume(float volume)
        {
            //AudioListener.volume = volume;
            AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 30);
            MasterVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void SetMusicVolume(float volume)
        {
            AudioMixer.SetFloat("Music", Mathf.Log10(volume) * 30);
            MusicVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void SetEffectsVolume(float volume)
        {
            AudioMixer.SetFloat("Effects", Mathf.Log10(volume) * 30);
            EffectsVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI
        }

        private void ResetSettings()
        {
            //Reset settings back to default
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }


}
