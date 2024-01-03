using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyUpgrade
{
    public Sprite sprite;
    public string parameter;
    public int currentLevel;
    public string[] levelDescription = { };
}
public class UpgradePanel : MonoBehaviour
{
    public GameObject panel;
    public IvyInterface[] trees;
    public GameObject[] respectiveTreeButtons;
    public EnemyUpgrade[] enemyUpgrades;
    public SceneLoader sceneLoader;
    private GameManager gameManager;
    private Enemy_Spawner spawner;

    public bool upgradeChoicesCoroutineStarted = false;
    public bool IsReadyForNextWave = false;


    public void ResetAllLevels()
    {
        for (int i = 0; i < trees.Length; i++)
        {
            IvyInterface tree = trees[i];
            GameObject button = respectiveTreeButtons[i];

            tree.SetTreeLevel(1);
            button.SetActive(true);
            button.GetComponent<TreeButton>().SetLevel(1);

            //// Except for some trees
            ////if ((tree is Willow) || (tree is LightBulb) || (tree is WoodenGrave) || (tree is Claw) || (tree is DollEyes))
            //if ((i < 6) || (i == 9))  // Also including Cactus and Worm Root
            //{
            //    tree.SetTreeLevel(1);
            //    button.SetActive(true);
            //    button.GetComponent<TreeButton>().SetLevel(1);
            //}
            //// Initialize each prefab by disabling them
            //else
            //{
            //    tree.Initialize();
            //    button.SetActive(false);
            //}
        }
    }



    private IEnumerator ShowUpgradeChoices()
    {
        // Get random update choices
        List<int> availableIndices = new();
        List<IvyInterface> upgradeChoices = new();
        List<int> respectiveId = new();

        // Populate available indices based on conditions (e.g., level less than maximum)
        for (int i = 0; i < trees.Length; i++)
        {
            // Check if the tree has room for more upgrades (level less than 3)
            if (trees[i].GetTreeLevel() < trees[i].GetMaxLevel())
            {
                availableIndices.Add(i);
            }
        }

        // Randomly select three upgrade choices
        for (int i = 0; i < 3; i++)
        {
            if (availableIndices.Count > 0)
            {
                int randomIndex = Random.Range(0, availableIndices.Count);
                int chosenIndex = availableIndices[randomIndex];
                upgradeChoices.Add(trees[chosenIndex]);
                respectiveId.Add(chosenIndex);
                availableIndices.RemoveAt(randomIndex);
            }
        }

        // Find each UpgradeButton by name in the children of the UpgradePanel
        UpgradeButton[] upgradeButtons = panel.GetComponentsInChildren<UpgradeButton>();

        // Set the content of each upgrade button
        for (int i = 0; i < upgradeChoices.Count && i < upgradeButtons.Length-1; i++)
        {
            IvyInterface tree = upgradeChoices[i];
            string buttonText = tree.GetLevelDescription();
            Sprite buttonImage = tree.GetComponentInChildren<SpriteRenderer>().sprite;
            UpgradeButton upgradeButtonScript = upgradeButtons[i];

            if (upgradeButtonScript != null)
            {
                upgradeButtonScript.respectiveButton = respectiveTreeButtons[respectiveId[i]];
                upgradeButtonScript.SetButtonContent(buttonText, buttonImage);
                upgradeButtonScript.SetUpgradedTree(tree);
            }
        }

            if (enemyUpgrades.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyUpgrades.Length);
                upgradeButtons[upgradeButtons.Length-1].SetButtonContent(enemyUpgrades[randomIndex].levelDescription[enemyUpgrades[randomIndex].currentLevel]
                                                                      ,enemyUpgrades[randomIndex].sprite);
                upgradeButtons[upgradeButtons.Length-1].SetUpgradeMonsterFeature(enemyUpgrades[randomIndex].parameter, randomIndex, enemyUpgrades[randomIndex].currentLevel);

            }
  
        yield return 1;
    }


    private void Start()
    {
        ResetAllLevels();
        gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<Enemy_Spawner>();
    }


    private void Update()
    {
        if (spawner.waveCleared && !upgradeChoicesCoroutineStarted)
        {
            Debug.Log("Showing Upgrade Panel");
            panel.SetActive(true);
            StartCoroutine(ShowUpgradeChoices());
            upgradeChoicesCoroutineStarted = true;
        }
    }
}
