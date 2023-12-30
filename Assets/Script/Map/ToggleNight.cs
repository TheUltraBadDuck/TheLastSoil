using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ToggleNight : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Volume nightControl;
    [SerializeField] private GameObject fireflies;
    [SerializeField] private Light2D EntitiesLight;
    private ParticleSystem firefliesParticle;
    public bool isNight = false;
    [SerializeField] private Gradient mapColor;



    private float changeTime = 6f;
    private ColorAdjustments colorFilter;

    void Start()
    {
        nightControl.weight = 0.2f;
        EntitiesLight.intensity = 1f;
        firefliesParticle = fireflies.GetComponent<ParticleSystem>();
        if (!nightControl.profile.TryGet<ColorAdjustments>(out colorFilter))
        {
            Debug.LogError("Failed to get ColorAdjustments from VolumeProfile");
        }
        fireflies.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace this with your condition
        {
            isNight = !isNight;
            Gradient gradient = isNight ? mapColor : ReverseGradient(mapColor);
            StopAllCoroutines();
            StartCoroutine(TransitionGradient(gradient, changeTime));
        }
    }

    Gradient ReverseGradient(Gradient gradient)
    {
        Gradient reversedGradient = new Gradient();
        GradientColorKey[] colorKeys = gradient.colorKeys;
        Array.Reverse(colorKeys);
        reversedGradient.SetKeys(colorKeys, gradient.alphaKeys);
        return reversedGradient;
    }


    IEnumerator TransitionGradient(Gradient gradient, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Color color = gradient.Evaluate(t);

            // Set the color filter of the Color Adjustment
            colorFilter.colorFilter.overrideState = true;
            colorFilter.colorFilter.value = color;

            // Adjust the intensity of the Global Light 2D
            float intensity = isNight ? (1 - t) * 0.7f + 0.3f : t * 0.7f + 0.3f;
            EntitiesLight.intensity = intensity;

            // Activate or deactivate the fireflies
            if (t >= 0.8 && isNight)
            {
                fireflies.SetActive(true);
            }
            else if (t <= 0.8 && !isNight)
            {
                fireflies.SetActive(false);
            }

            // Set the weight of the Volume
            if (isNight)
            {
                nightControl.weight = t * 0.8f + 0.2f; // Interpolates between 0.2 and 1
            }
            else
            {
                nightControl.weight = (1 - t) * 0.8f + 0.2f; // Interpolates between 1 and 0.2
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
