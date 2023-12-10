using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string name;
    public GameObject Prefab;
    [Range(0f, 100f)] public float Chance = 100f;

    [HideInInspector] public double _weight;

}
public class Enemy_Spawner : MonoBehaviour
{
    public Enemy[] enemies;
    public Vector2 spawnArea;
    public int enemiesNumber;
    public float spawnTimer;

    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    private int enemySpawned = 0;
    private float timer;


    private void Awake()
    {
        CalculateWeights();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && enemySpawned < enemiesNumber)
        {
            Vector3 position = new Vector3(
                UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
                UnityEngine.Random.Range(-spawnArea.y, spawnArea.y), // Corrected the Y-axis range
                0f);

            RandomSpawn(position);
            timer = spawnTimer;
            enemySpawned++;
        }

    }

    private void RandomSpawn (Vector3 position)
    {
        Enemy randomEnemy = enemies[GetRandomEnemyIndex()];
        Instantiate(randomEnemy.Prefab, position, Quaternion.identity, transform);
    }

    private int GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i]._weight >= r)
                return i;
        }
        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (Enemy enemy in enemies)
        {
            accumulatedWeights += enemy.Chance;
            enemy._weight = accumulatedWeights;
        }
    }
}
