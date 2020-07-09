using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    private Settings settings;

    private float musicVolume;
    private float soundsVolume;
    private bool enableMusicAndSounds;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider soundsSlider;

    public GameObject allSoundsObject;

    public AudioSource musicAudioSource;
    public AudioSource soundsAudioSource;

    private void Awake()
    {
        settings = GetComponent<Settings>();
    }

    void Start()
    {
        musicVolume = Settings.musicVolume;
        soundsVolume = Settings.soundsVolume;
        enableMusicAndSounds = Settings.enableMusicAndSounds;

        musicAudioSource.enabled = Settings.enableMusicAndSounds;
        musicAudioSource.volume = musicVolume;

        musicSlider.value = musicVolume;
        soundsSlider.value = soundsVolume;

        allSoundsObject.transform.GetChild(0).gameObject.SetActive(!enableMusicAndSounds);

        if (soundsAudioSource)
        {
            soundsAudioSource.enabled = Settings.enableMusicAndSounds;
            soundsAudioSource.volume = soundsVolume;
        }
    }

    private void LateUpdate()
    {
        musicAudioSource.enabled = enableMusicAndSounds;
        musicAudioSource.volume = musicVolume;
        if (soundsAudioSource)
        {
            soundsAudioSource.enabled = enableMusicAndSounds;
            soundsAudioSource.volume = soundsVolume;
        }

        if(LevelManager.instance == null)
        {
            return;
        }

        if (LevelManager.instance.pause)
        {
            musicAudioSource.Pause();
        }
        else
        {
            musicAudioSource.UnPause();
        }
    }    

    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        settings.SaveMusicVolumeValue(musicVolume);
    }

    public void SetSoundsVolume()
    {
        soundsVolume = soundsSlider.value;
        settings.SaveSoundsVolumeValue(soundsVolume);
    }

    public void ToggleAllSounds()
    {
        enableMusicAndSounds = !enableMusicAndSounds;
        allSoundsObject.transform.GetChild(0).gameObject.SetActive(!enableMusicAndSounds);
        settings.SaveAllSoundsToggle(enableMusicAndSounds);
    }
}
