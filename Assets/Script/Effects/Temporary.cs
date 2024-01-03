using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Temporary : MonoBehaviour
{
    [SerializeField]
    private float duration = 1.0f;
    private float maxDuration = 1.0f;


    private void Start()
    {
        maxDuration = duration;
        duration = 0f;
    }


    private void Update()
    {
        duration += Time.deltaTime;
        if (duration > maxDuration)
        {
            Destroy(gameObject);
        }
    }
}
