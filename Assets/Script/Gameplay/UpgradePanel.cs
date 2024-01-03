using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public GameObject panel;
    public IvyInterface[] trees;

    private GameManager gameManager;
    private bool upgradeButtonClicked = false;
    private Enemy_Spawner spawner;

    public bool upgradeChoicesCoroutineStarted = false;
    public bool IsReadyForNextWave = false;

    public static void ResetAllLevels()
    {
<<<<<<< Updated upstream
        // Find all objects with the AttackTreeInterface component
        IvyInterface[] allTreeInterfaces = Resources.FindObjectsOfTypeAll<IvyInterface>();

        foreach (IvyInterface treeInterface in allTreeInterfaces)
        {
            // Initialize each prefab
            treeInterface.Initialize();
=======
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
>>>>>>> Stashed changes
        }
    }
    private List<IvyInterface> GetRandomUpgradeChoices()
    {
        List<IvyInterface> upgradeChoices = new List<IvyInterface>();
        List<int> availableIndices = new List<int>();

        // Populate available indices based on conditions (e.g., level less than maximum)
        for (int i = 0; i < trees.Length; i++)
        {
            // Check if the tree has room for more upgrades (level less than 3)
            if (trees[i].currentLevel <= 2)
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
                availableIndices.RemoveAt(randomIndex);
            }
        }

        return upgradeChoices;
    }

    private IEnumerator ShowUpgradeChoices()
    {
        List<IvyInterface> upgradeChoices = GetRandomUpgradeChoices();
        Debug.Log(upgradeChoices);

        // Find each UpgradeButton by name in the children of the UpgradePanel
        UpgradeButton[] upgradeButtons = panel.GetComponentsInChildren<UpgradeButton>();

        // Set the content of each upgrade button
        for (int i = 0; i < upgradeChoices.Count && i < upgradeButtons.Length; i++)
        {
            string buttonText = upgradeChoices[i].levelDescription[upgradeChoices[i].currentLevel];
            Debug.Log(buttonText);
            Sprite buttonImage = upgradeChoices[i].GetComponentInChildren<SpriteRenderer>().sprite;
            UpgradeButton upgradeButtonScript = upgradeButtons[i];
            if (upgradeButtonScript != null)
            {
                upgradeButtonScript.SetButtonContent(buttonText, buttonImage);
                upgradeButtonScript.SetUpgradedTree(upgradeChoices[i]);
            }
        }

        yield return null;
    }
    private void Start()
    {
        ResetAllLevels();
       gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<Enemy_Spawner>();
    }

    void Update()
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
