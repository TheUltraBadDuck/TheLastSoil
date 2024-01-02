using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public AudioSource musicAdudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip clicked;

    void Start()
    {
        musicAdudioSource.clip = musicClip;
        musicAdudioSource.Play();
    }
}
