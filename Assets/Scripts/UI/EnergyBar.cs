using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField]
    private int score;
    public int Score
    {
        get {
            return score;
        }
        set {
            score = value;
            Text textComponent = text.GetComponent<Text>();
            textComponent.text = score.ToString();
        }
    }
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private GameObject text;


    public GameObject GetImage()
    {
        return image;
    }

    public void AddScore(int value)
    {
        Score += value;
        Text textComponent = text.GetComponent<Text>();
        textComponent.text = Score.ToString();
    }
}
