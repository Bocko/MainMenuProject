using System.Collections.Generic;
using UnityEngine;

public class SettingsPlayerPrefs
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

    //graphics
    private const string framerateKey = "framerate";
    public static int defaultFramerate = 200;//40-200 and 201 is unlimited
    private const string qualityIndexKey = "qualityIndex";
    public static int defaultQualityIndex = 1;
    private const string antiAliasingIndexKey = "antiAliasingIndex";
    public static int defaultAntiAliasingIndex = 2;
    private const string ambientOcclusionIndexKey = "ambientOcclusionIndex";
    public static int defaultAmbientOcclusionIndex = 3;
    private const string motionBlurKey = "motionBlur";
    public static int defaultMotionBlur = 0;//0 = false, 1 = true

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

    public static void SaveIsFullScreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt(isFullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool LoadIsFullscreen()
    {
        return PlayerPrefs.GetInt(isFullscreenKey, defaultIsFullscreen) == 1;
    }

    #endregion

    #region GRAPHICS

    public static void SaveFramerate(int framerate)
    {
        PlayerPrefs.SetInt(framerateKey, framerate);
        PlayerPrefs.Save();
    }

    public static int LoadFramerate()
    {
        return PlayerPrefs.GetInt(framerateKey, defaultFramerate);
    }

    public static void SaveQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt(qualityIndexKey, qualityIndex);
        PlayerPrefs.Save();
    }

    public static int LoadQuality()
    {
        return PlayerPrefs.GetInt(qualityIndexKey, defaultQualityIndex);
    }

    public static void SaveAntiAliasing(int antiAliasingIndex)
    {
        PlayerPrefs.SetInt(antiAliasingIndexKey, antiAliasingIndex);
        PlayerPrefs.Save();
    }

    public static int LoadAntiAliasing()
    {
        return PlayerPrefs.GetInt(antiAliasingIndexKey, defaultAntiAliasingIndex);
    }

    public static void SaveAmbientOcclusion(int ambientOcclusionIndex)
    {
        PlayerPrefs.SetInt(ambientOcclusionIndexKey, ambientOcclusionIndex);
        PlayerPrefs.Save();
    }

    public static int LoadAmbientOcclusion()
    {
        return PlayerPrefs.GetInt(ambientOcclusionIndexKey, defaultAmbientOcclusionIndex);
    }

    public static void SaveMotionBlur(bool motionBlur)
    {
        PlayerPrefs.SetInt(motionBlurKey, motionBlur ? 1 : 0);
    }

    public static bool LoadMotionBlur()
    {
        return PlayerPrefs.GetInt(motionBlurKey, defaultMotionBlur) == 1;
    }

    #endregion
}
