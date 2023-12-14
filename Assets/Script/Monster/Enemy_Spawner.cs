using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
    public int enemiesNumber;
    public int enemiesEachWave;
    public float spawnTime;
    public bool bigWave = false;
}
public class Enemy_Spawner : MonoBehaviour
{
    private System.Random rand = new System.Random();
    private float timer;
    private int enemySpawned = 0;
    private int levelTraversal = 0;

    public Level[] levels;
    public float X1, X2, Y1, Y2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLevels());
    }
    private void Update()
    {
        
    }
    private IEnumerator SpawnLevels()
    {
        while (levelTraversal < levels.Length)
        {
            CalculateWeights(levels[levelTraversal]);
            Debug.Log("Level " + (levelTraversal + 1));

            // Start the LevelSpawner coroutine and wait for it to finish
            yield return StartCoroutine(LevelSpawner(levels[levelTraversal]));

            levelTraversal++;
            Debug.Log("Level finished!");

            // You can introduce a delay between levels if needed
            yield return new WaitForSeconds(1f); // Adjust the delay time as necessary
        }

        // All levels completed
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
                    Vector3 position = new Vector3(
                        UnityEngine.Random.Range(X1, X2),
                        UnityEngine.Random.Range(Y1, Y2),
                        0f);

                    RandomSpawn(position, level);
                    enemySpawned++;
                }

                timer = level.spawnTime;

                if (enemySpawned >= level.enemiesNumber)
                    break;
            }

            yield return null;
        }

        enemySpawned = 0; // Reset for the next level
    }

    private void RandomSpawn(Vector3 position, Level level)
    {
        Enemy randomEnemy = level.enemies[GetRandomEnemyIndex(level)];
        Instantiate(randomEnemy.Prefab, position, Quaternion.identity, transform);
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
}
