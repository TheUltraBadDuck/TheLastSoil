using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    // Score
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
            UpdateButtonsPressable();
        }
    }
    // Gameobjects
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private GameObject text;

    // List of Buttons
    private readonly List<TreeButton> treeButtons = new();



    // Load Trees
    public void Start()
    {
        foreach (Transform button in GameObject.Find("UIGameplay/Scroll/TreePanel").transform)
        {
            treeButtons.Add(button.GetComponent<TreeButton>());
        }

        Text textComponent = text.GetComponent<Text>();
        textComponent.text = score.ToString();
        UpdateButtonsPressable();
    }



    // Make buttons pressable / unpressable
    public void UpdateButtonsPressable()
    {
        foreach (TreeButton button in treeButtons)
        {
            button.UpdatePressableByTotalEnergy(button.energyScore <= score);
        }
    }



    public GameObject GetImage()
    {
        return image;
    }



    public void AddScore(int value)
    {
        Score += value;
        Text textComponent = text.GetComponent<Text>();
        textComponent.text = Score.ToString();
        UpdateButtonsPressable();
    }


    public void SubtractScore(int value)
    {
        Score -= value;
        Text textComponent = text.GetComponent<Text>();
        textComponent.text = Score.ToString();
        UpdateButtonsPressable();
    }
}
