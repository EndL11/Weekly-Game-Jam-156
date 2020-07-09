using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings : MonoBehaviour
{
    public static float musicVolume = 1f;
    public static float soundsVolume = 1f;
    public static bool enableMusicAndSounds = true;

    private void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat("music");
        soundsVolume = PlayerPrefs.GetFloat("sounds");
        enableMusicAndSounds = PlayerPrefs.GetInt("allToggle") == 1 ? true : false;
    }

    public void SaveMusicVolumeValue(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("music", musicVolume);
    }

    public void SaveSoundsVolumeValue(float value)
    {
        soundsVolume = value;
        PlayerPrefs.SetFloat("sounds", soundsVolume);
    }

    public void SaveAllSoundsToggle(bool value)
    {
        enableMusicAndSounds = value;
        PlayerPrefs.SetInt("allToggle", enableMusicAndSounds ? 1 : 0);
    }
}
