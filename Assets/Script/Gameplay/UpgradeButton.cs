using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Text buttonText;
    public Image buttonImage;
    public IvyInterface upgradedTree;

    // Define an event to be triggered when the button is clicked
    public UnityEvent OnButtonClick = new UnityEvent();
    private Enemy_Spawner spawner;
    private UpgradePanel upgradePanel;

    // Start is called before the first frame update
    void Start()
    {
        upgradePanel = FindObjectOfType<UpgradePanel>();
        // Attach a listener to the button's onClick event
        GetComponent<Button>().onClick.AddListener(InvokeButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to invoke the OnButtonClick event
    private void InvokeButtonClick()
    {
        upgradedTree.currentLevel++;
        UpdateLevelForAllTrees(upgradedTree.currentLevel);

        upgradePanel.upgradeChoicesCoroutineStarted = false;
        Debug.Log("buttonclicked");
        upgradePanel.IsReadyForNextWave = true;

    }

    private void UpdateLevelForAllTrees(int level)
    {
        if (upgradedTree != null)
        {
            string upgradedTreeName = upgradedTree.getTreeName(); // Assuming getTreeName is a method in AttackTreeInterface that returns the name

            // Find all objects with the AttackTreeInterface component
            IvyInterface[] allTreeInterfaces = GameObject.FindObjectsOfType<IvyInterface>();

            foreach (IvyInterface treeInterface in allTreeInterfaces)
            {
                // Check if the treeInterface has the same TreeName as the upgradedTree
                if (treeInterface.getTreeName() == upgradedTreeName)
                {
                    // Increment the current level
                    treeInterface.currentLevel++;

                    // Ensure the current level is within bounds
                    if (treeInterface.currentLevel >= treeInterface.levelDescription.Length)
                    {
                        treeInterface.currentLevel = treeInterface.levelDescription.Length - 1;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Upgraded tree is null.");
        }
    }


    public void SetButtonContent(string text, Sprite imageSprite)
    {
        // Set the text of the button
        if (buttonText != null)
        {
            buttonText.text = text;
        }
        else
        {
            Debug.LogWarning("Text component not assigned in the inspector.");
        }

        // Set the image of the button
        if (buttonImage != null)
        {
            buttonImage.sprite = imageSprite;
        }
        else
        {
            Debug.LogWarning("Image component not assigned in the inspector.");
        }
    }
    public void SetUpgradedTree(IvyInterface tree)
    {
        if (tree != null)
        {
            upgradedTree = tree;
        }
        else
        {
            Debug.LogWarning("Null Tree.");
        }
    }
}
