using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormStick : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioObject>().GetSoundOffset();
        audioSource.Play();
    }

    public void MakeDestroy()
    {
        Destroy(gameObject);
    }
}
