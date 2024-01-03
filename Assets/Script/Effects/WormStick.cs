using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormStick : MonoBehaviour
{
    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    public void MakeDestroy()
    {
        Destroy(gameObject);
    }
}
