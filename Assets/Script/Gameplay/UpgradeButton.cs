using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
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
    private EnergyBar energy;
    public string monsterFeature;
    public bool isMonster;
    public int monsterFeatureLevel;

    // Start is called before the first frame update
    private void Start()
    {
        upgradePanel = FindObjectOfType<UpgradePanel>();
        spawner = FindObjectOfType<Enemy_Spawner>();
        energy = FindObjectOfType<EnergyBar>();
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

            if(monsterFeature == "hp")
            {
                if(monsterFeatureLevel == 0)
                {
                    spawner.enemyHpIncrease = 1.25f;
                }
                if (monsterFeatureLevel == 1)
                {
                    spawner.enemyHpIncrease = 1.50f;
                }
                if (monsterFeatureLevel == 2)
                {
                    spawner.enemyHpIncrease = 2f;
                }
            }
            if (monsterFeature == "damage")
            {
                if (monsterFeatureLevel == 0)
                {
                    spawner.enemyDamageIncrease = 1.25f;
                }
                if (monsterFeatureLevel == 1)
                {
                    spawner.enemyDamageIncrease = 1.50f;
                }
                if (monsterFeatureLevel == 2)
                {
                    spawner.enemyDamageIncrease = 2f;
                }
            }
            if (monsterFeature == "speed")
            {
                if (monsterFeatureLevel == 0)
                {
                    spawner.enemySpeedIncrease = 1.25f;
                }
                if (monsterFeatureLevel == 1)
                {
                    spawner.enemySpeedIncrease = 1.50f;
                }
                if (monsterFeatureLevel == 2)
                {
                    spawner.enemySpeedIncrease = 2f;
                }
            }
            if (monsterFeature == "number")
            {
                if (monsterFeatureLevel == 0)
                {
                    spawner.enemyNumberIncrease = 1.25f;
                }
                if (monsterFeatureLevel == 1)
                {
                    spawner.enemyNumberIncrease = 1.50f;
                }
                if (monsterFeatureLevel == 2)
                {
                    spawner.enemyNumberIncrease = 2f;
                }
            }
            energy.AddScore(500);
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
