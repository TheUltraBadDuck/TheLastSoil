using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public GameObject panel;
    public AttackTreeInterface[] attackTrees;

    private GameManager gameManager;
    private bool upgradeButtonClicked = false;
    private Enemy_Spawner spawner;

    public bool upgradeChoicesCoroutineStarted = false;
    public bool IsReadyForNextWave = false;

    public static void ResetAllLevels()
    {
        // Find all objects with the AttackTreeInterface component
        AttackTreeInterface[] allTreeInterfaces = Resources.FindObjectsOfTypeAll<AttackTreeInterface>();

        foreach (AttackTreeInterface treeInterface in allTreeInterfaces)
        {
            // Initialize each prefab
            treeInterface.Initialize();
        }
    }
    private List<AttackTreeInterface> GetRandomUpgradeChoices()
    {
        List<AttackTreeInterface> upgradeChoices = new List<AttackTreeInterface>();
        List<int> availableIndices = new List<int>();

        // Populate available indices based on conditions (e.g., level less than maximum)
        for (int i = 0; i < attackTrees.Length; i++)
        {
            // Check if the tree has room for more upgrades (level less than 3)
            if (attackTrees[i].currentLevel <= 2)
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
                upgradeChoices.Add(attackTrees[chosenIndex]);
                availableIndices.RemoveAt(randomIndex);
            }
        }

        return upgradeChoices;
    }

    private IEnumerator ShowUpgradeChoices()
    {
        List<AttackTreeInterface> upgradeChoices = GetRandomUpgradeChoices();

        // Find each UpgradeButton by name in the children of the UpgradePanel
        UpgradeButton[] upgradeButtons = panel.GetComponentsInChildren<UpgradeButton>();

        // Set the content of each upgrade button
        for (int i = 0; i < upgradeChoices.Count && i < upgradeButtons.Length; i++)
        {
            string buttonText = upgradeChoices[i].levelDescription[upgradeChoices[i].currentLevel];
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
