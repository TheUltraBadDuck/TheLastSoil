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
    public GameObject respectiveButton;

    // Define an event to be triggered when the button is clicked
    public UnityEvent OnButtonClick = new ();
    private Enemy_Spawner spawner;
    private UpgradePanel upgradePanel;

    public string monsterFeature;
    public bool isMonster;
    public int monsterFeatureLevel;

    // Start is called before the first frame update
    private void Start()
    {
        upgradePanel = FindObjectOfType<UpgradePanel>();

        // Attach a listener to the button's onClick event
        GetComponent<Button>().onClick.AddListener(InvokeButtonClick);
        
    }


    // Method to invoke the OnButtonClick event
    private void InvokeButtonClick()
    {
        if (monsterFeature != "")
        {
            isMonster = true;


        }
        if (!isMonster)
        {
            upgradedTree.UpgradeLevel();
            UpdateLevelForAllTrees();

            // Upgrade one of the trees
            if (upgradedTree.GetTreeLevel() == 1)
                respectiveButton.SetActive(true);
            respectiveButton.GetComponent<TreeButton>().SetLevel(upgradedTree.GetTreeLevel());

            upgradePanel.upgradeChoicesCoroutineStarted = false;
            Debug.Log("buttonclicked");
            upgradePanel.IsReadyForNextWave = true;
        }
        else
        {
            Debug.Log("Monster! + " + monsterFeature);

            upgradePanel.upgradeChoicesCoroutineStarted = false;
            Debug.Log("buttonclicked");
            upgradePanel.IsReadyForNextWave = true;
        }

    }

    private void UpdateLevelForAllTrees()
    {
        if (upgradedTree != null)
        {
            // Find all objects with the AttackTreeInterface component
            IvyInterface[] allTreeInterfaces = GameObject.FindObjectsOfType<IvyInterface>();

            foreach (IvyInterface tree in allTreeInterfaces)
            {
                // Check if the treeInterface has the same TreeName as the upgradedTree
                // If so, increment the current level
                if (tree.GetType() == upgradedTree.GetType())
                    tree.UpgradeLevel();
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
            buttonText.text = text;
        else
            Debug.LogWarning("Text component not assigned in the inspector.");

        // Set the image of the button
        if (buttonImage != null)
            buttonImage.sprite = imageSprite;
        else
            Debug.LogWarning("Image component not assigned in the inspector.");
    }
    public void SetUpgradedTree(IvyInterface tree)
    {
        if (tree != null)
            upgradedTree = tree;
        else
            Debug.LogWarning("Null Tree.");
    }

    public void SetUpgradeMonsterFeature(string feature, int index, int level)
    {
        monsterFeature = feature;
        monsterFeatureLevel = level;
        upgradePanel.enemyUpgrades[index].currentLevel++;
    }
}
