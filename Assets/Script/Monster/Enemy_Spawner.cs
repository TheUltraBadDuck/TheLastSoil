using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class Enemy
{
    public float point;
    public GameObject Prefab;

    [Range(0f, 100f)] public float Chance = 100f;
    [HideInInspector] public double _weight;

}



[System.Serializable]
public class Level
{
    public Enemy[] enemies;
    public double accumulatedWeights;

    public float difficulty;
    public int enemiesNumber;           // Number of enemies in a wave
    public int enemiesEachWave;          
    public float spawnTime;
    public bool bigWave = false;
}


public class Enemy_Spawner : MonoBehaviour
{
    private System.Random rand = new ();
    private List<GameObject> spawnedEnemies = new ();
    private float timer;
    private int enemySpawned = 0;
    private int levelTraversal = 0;
    private UpgradePanel upgradePanel;


    public Level[] levels;
    public CanvasGroup waveTextCanvasGroup;
    public float X1, X2, Y1, Y2;
    public bool waveCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        StartNextWave();
        upgradePanel = FindObjectOfType<UpgradePanel>();
    }

    private void StartNextWave()
    {
        waveCleared = false;
        
        if (levelTraversal < levels.Length)
        {
            StartCoroutine(StartWave(levels[levelTraversal]));
            levelTraversal++;
            enemySpawned = 0;

        }
        else
        {
            // All levels completed, you might want to handle this case
            Debug.Log("All levels completed!");
        }
    }

    private IEnumerator StartWave(Level level)
    {
        CalculateWeights(level);
        Debug.Log("Level " + (levelTraversal + 1));

        yield return StartCoroutine(FadeText("Wave " + (levelTraversal + 1), 2f, 2f, 2f));

        yield return StartCoroutine(LevelSpawner(level));

        Debug.Log("Level finished!");

        // Wait for response from "UpgradePanel"
        yield return StartCoroutine(WaitForUpgradePanelResponse());
        
    }

    private IEnumerator LevelSpawner(Level level)
    {
        while (enemySpawned < level.enemiesNumber)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                int enemiesToSpawn = Mathf.Min(level.enemiesEachWave, level.enemiesNumber - enemySpawned);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector3 position = new (
                        UnityEngine.Random.Range(X1, X2),
                        UnityEngine.Random.Range(Y1, Y2),
                        0f);

                    GameObject spawnedEnemy = RandomSpawn(position, level);
                    spawnedEnemies.Add(spawnedEnemy);

                    enemySpawned++;
                }

                timer = level.spawnTime;

                if (enemySpawned >= level.enemiesNumber)
                    break;
            }

            yield return null;
        }

        // Wait until all enemies are defeated
        while (spawnedEnemies.Count > 0)
        {
            spawnedEnemies.RemoveAll(enemy => enemy == null); // Remove destroyed enemies
            yield return null;
        }

        Debug.Log("All enemies defeated, show 'Wave Clear'");
        yield return StartCoroutine(FadeText("Wave Clear", 2f, 2f, 2f));

        waveCleared = true;
    }

    private IEnumerator WaitForUpgradePanelResponse()
    {
        while (!upgradePanel.IsReadyForNextWave)
        {
            yield return null;

        }
        
        upgradePanel.upgradeChoicesCoroutineStarted = false;
        upgradePanel.panel.SetActive(false);
        upgradePanel.IsReadyForNextWave = false;

        StartNextWave();
    }

    private GameObject RandomSpawn(Vector3 position, Level level)
    {
        Enemy randomEnemy = level.enemies[GetRandomEnemyIndex(level)];
        GameObject spawnedEnemy = Instantiate(randomEnemy.Prefab, position, Quaternion.identity, transform);
        return spawnedEnemy;
    }

    private int GetRandomEnemyIndex(Level level)
    {
        double r = rand.NextDouble() * level.accumulatedWeights;
        for (int i = 0; i < level.enemies.Length; i++)
        {
            if (level.enemies[i]._weight >= r)
                return i;
        }
        return 0;
    }

    private void CalculateWeights(Level level)
    {
        level.accumulatedWeights = 0f;
        foreach (Enemy enemy in level.enemies)
        {
            level.accumulatedWeights += enemy.Chance;
            enemy._weight = level.accumulatedWeights;
        }
    }

    //Text
    private IEnumerator FadeText(string text, float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        // Set the text here
        waveTextCanvasGroup.GetComponentInChildren<Text>().text = text;
        float elapsedTime = 0f;
        ShowWaveText();

        // Fade In
        while (elapsedTime < fadeInDuration)
        {
            waveTextCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        

        // Stay
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            waveTextCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hide the text after fading out
        HideWaveText();
    }

    private void ShowWaveText()
    {
        waveTextCanvasGroup.alpha = 0f;
        waveTextCanvasGroup.gameObject.SetActive(true);
    }

    private void HideWaveText()
    {
        waveTextCanvasGroup.gameObject.SetActive(false);
    }
}
