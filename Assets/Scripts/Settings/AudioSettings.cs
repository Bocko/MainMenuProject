using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

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
            //UI valueinitializations here (should be form Default Stetting / Player Preferences / Currently applied options)
            LoadPlayerPrefs();
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

        private void LoadPlayerPrefs()
        {
            //loads audio
            print("load audio");

            MasterVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.MASTER);
            EffectsVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.EFFECT);
            MusicVolumeSlider.value = SettingsPlayerPrefs.LoadVolume(SettingsPlayerPrefs.Volumes.MUSIC);
        }

        private void SavePlayerPrefs()
        {
            //saves audio
            print("save audio");

            SettingsPlayerPrefs.SaveVolume(MasterVolumeSlider.value, SettingsPlayerPrefs.Volumes.MASTER);
            SettingsPlayerPrefs.SaveVolume(EffectsVolumeSlider.value, SettingsPlayerPrefs.Volumes.EFFECT);
            SettingsPlayerPrefs.SaveVolume(MusicVolumeSlider.value, SettingsPlayerPrefs.Volumes.MUSIC);
        }

        private void ResetPlayerPrefs()
        {
            //resets audio
            print("reset audio");

            MasterVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
            EffectsVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
            MusicVolumeSlider.value = SettingsPlayerPrefs.defaultVolume;
        }

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI
            SavePlayerPrefs();
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
            //shoud use this or ApplySettings() on menuchange
            LoadPlayerPrefs();
        }
    }


}
