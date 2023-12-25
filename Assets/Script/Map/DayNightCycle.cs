using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycle : MonoBehaviour
{
    public float duration = 10f;
    [SerializeField] private Gradient gradient;

    private Light2D _worldlight;
    private float startTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _worldlight = GetComponent<Light2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - startTime;
        float percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI) * 0.5f + 0.5f;

        percentage = Mathf.Clamp01(percentage);
        _worldlight.color =gradient.Evaluate(percentage);
    }
}
