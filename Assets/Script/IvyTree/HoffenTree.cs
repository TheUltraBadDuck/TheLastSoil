using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoffenTree : IvyInterface
{
    [SerializeField]
    private float healCD = 3f;
    private float healTimer = 0f;

    [SerializeField]
    private Enemy_Spawner enemySpawner;
    [SerializeField]
    private SceneLoader loader;

    private bool gameOver = false;


    public override void Start()
    {
        base.Start();
        healTimer = healCD;
        healCD = 0f;
    }



    public override void Update()
    {
        base.Update();

        if (hp < maxhp)
        {
            healCD += Time.deltaTime;
            if (healCD > healTimer)
            {
                healCD += 1f;
                healCD = 0;
            }
        }
    }



    public override void BeAttacked(float damage)
    {
        if (gameObject == null)
            return;

        if (gameOver)
            return;

        hp -= damage;

        if (hp <= 0)
        {
            // Game over
            enemySpawner.gameOver = true;
            loader.GameOver();
            gameOver = true;
        }
        else
        {
            attacked = true;
        }
    }

}
