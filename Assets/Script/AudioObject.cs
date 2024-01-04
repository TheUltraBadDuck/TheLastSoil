using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class AudioObject : MonoBehaviour
{
    public AudioSource musicAdudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip clicked;

    public Slider soundSlider;
    private float soundOffset = 0.8f;


    public float GetSoundOffset()
    {
        return soundOffset;
    }

    private void Start()
    {
        soundOffset = LoadSoundOffset();

        soundSlider.value = soundOffset * 100;
        musicAdudioSource.volume = soundOffset * 100;
        musicAdudioSource.clip = musicClip;
        musicAdudioSource.loop = true;
        musicAdudioSource.Play();
    }

    public void OnSliderChanged()
    {
        soundOffset = soundSlider.value / 100f;
        musicAdudioSource.volume = soundOffset;
    }

    public void SaveSoundOffset()
    {
        PlayerPrefs.SetFloat("soundOffset", soundOffset);
        PlayerPrefs.Save();
    }

    public float LoadSoundOffset()
    {
        if (PlayerPrefs.HasKey("soundOffset"))
            return PlayerPrefs.GetFloat("soundOffset");
        return 0.8f;
    }
}