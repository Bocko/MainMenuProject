using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts
{
    //modified the original function to only apply settings when the apply button is pressed
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

        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        private const string masterAudioMixerName = "Master";
        private const string musicAudioMixerName = "Music";
        private const string effectsAudioMixerName = "Effects";

        private UnityAction applyAction;
        private UnityAction resetAction;

        private void Awake()
        {
            applyAction = new UnityAction(ApplySettings);
            resetAction = new UnityAction(ResetSettings);
        }

        private void Start()
        {
            //UI valueinitializations here (should be form Default Stetting / Player Preferences / Currently applied options)
            LoadPlayerPrefs();
            ApplyAudioSettings();
        }

        private void OnEnable()
        {
            applyButton.onClick.AddListener(applyAction);
            resetButton.onClick.AddListener(resetAction);

            MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            SetMasterVolume(MasterVolumeSlider.value);

            MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            SetMusicVolume(MusicVolumeSlider.value);

            EffectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
            SetEffectsVolume(EffectsVolumeSlider.value);
        }

        private void OnDisable()
        {
            CancelChanges();

            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);

            MasterVolumeSlider.onValueChanged.RemoveAllListeners();
            MusicVolumeSlider.onValueChanged.RemoveAllListeners();
            EffectsVolumeSlider.onValueChanged.RemoveAllListeners();
        }

        private void SetMasterVolume(float volume)
        {
            //AudioListener.volume = volume;
            MasterVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void SetMusicVolume(float volume)
        {
            MusicVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void SetEffectsVolume(float volume)
        {
            EffectsVolumeTextValue.text = (volume * 100).ToString("0");
        }

        private void LoadPlayerPrefs()
        {
            //loads audio

            MasterVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.MASTER);
            EffectsVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.EFFECT);
            MusicVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.MUSIC);
        }

        private void SavePlayerPrefs()
        {
            //saves audio

            SettingsPlayerPrefs.SaveVolume(MasterVolumeSlider.value, SettingsPlayerPrefs.Volumes.MASTER);
            SettingsPlayerPrefs.SaveVolume(EffectsVolumeSlider.value, SettingsPlayerPrefs.Volumes.EFFECT);
            SettingsPlayerPrefs.SaveVolume(MusicVolumeSlider.value, SettingsPlayerPrefs.Volumes.MUSIC);
        }

        private void ResetPlayerPrefs()
        {
            //resets audio

            MasterVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
            EffectsVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
            MusicVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
        }

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI and applies them
            SavePlayerPrefs();
            ApplyAudioSettings();
        }

        private void ResetSettings()
        {
            //resets audio settings to the defaults in SettingsPlayerPrefs and saves them
            ResetPlayerPrefs();
            ApplySettings();
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //should use this or ApplySettings() on menuchange
            LoadPlayerPrefs();
        }

        private void ApplyAudioSettings()
        {
            //applies the values show on UI
            AudioMixer.SetFloat(masterAudioMixerName, Mathf.Log10(MasterVolumeSlider.value) * 30);
            AudioMixer.SetFloat(musicAudioMixerName, Mathf.Log10(MusicVolumeSlider.value) * 30);
            AudioMixer.SetFloat(effectsAudioMixerName, Mathf.Log10(EffectsVolumeSlider.value) * 30);
        }
    }


}
