using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPlayerPrefs : MonoBehaviour
{
    //controls
    private Dictionary<string, string> keys = new Dictionary<string, string>();

    //audio
    public enum Volumes { MASTER, EFFECT, MUSIC }
    public static float defaultVolume = 1f;
    private const string masterVolumeKey = "masterVolume";
    private const string effectVolumeKey = "effectVolume";
    private const string musicVolumeKey = "musicVolume";

    //display
    private const string resolutionIndexKey = "resIndex";
    public static int defaultResIndex = Screen.resolutions.Length - 1;
    private const string brightnessKey = "brightness";
    public static float defaultBrightness = 0.5f;
    private const string isFullscreenKey = "isFullscreen";
    public static int defaultIsFullscreen = 1;//0 = false, 1 = true

    #region AUDIO

    public static void SaveVolume(float value, Volumes volume)
    {
        switch (volume)
        {
            case Volumes.MASTER:
                PlayerPrefs.SetFloat(masterVolumeKey, value);
                break;
            case Volumes.EFFECT:
                PlayerPrefs.SetFloat(effectVolumeKey, value);
                break;
            case Volumes.MUSIC:
                PlayerPrefs.SetFloat(musicVolumeKey, value);
                break;
        }
        PlayerPrefs.Save();
    }

    public static float LoadVolume(Volumes volume)
    {
        switch (volume)
        {
            case Volumes.MASTER:
                return PlayerPrefs.GetFloat(masterVolumeKey, defaultVolume);
            case Volumes.EFFECT:
                return PlayerPrefs.GetFloat(effectVolumeKey, defaultVolume);
            case Volumes.MUSIC:
                return PlayerPrefs.GetFloat(musicVolumeKey, defaultVolume);
        }
        return 0;// nOt AlL cOdE pAtHs rEtUrN VaLuE
    }

    #endregion

    #region DISPLAY

    public static void SaveResolution(int resIndex)
    {
        PlayerPrefs.SetInt(resolutionIndexKey, resIndex);
        PlayerPrefs.Save();
    }

    public static int LoadResolution()
    {
        return PlayerPrefs.GetInt(resolutionIndexKey, defaultResIndex);
    }

    public static void SaveBrightness(float brightness)
    {
        PlayerPrefs.SetFloat(brightnessKey, brightness);
        PlayerPrefs.Save();
    }

    public static float LoadBrightness()
    {
        return PlayerPrefs.GetFloat(brightnessKey, defaultBrightness);
    }

    public static void SaveIsFullScreen(int isFullscreen)
    {
        PlayerPrefs.SetInt(isFullscreenKey, isFullscreen);
        PlayerPrefs.Save();
    }

    public static int LoadIsFullscreen()
    {
        return PlayerPrefs.GetInt(isFullscreenKey, defaultIsFullscreen);
    }

    #endregion
}
