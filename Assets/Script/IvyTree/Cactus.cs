using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : DefenseTreeInterface
{
    [SerializeField]
    protected float attackCD = 0.0f;
    protected float maxAttackCD = 0.0f;

    bool attacking = false;
    protected List<Behavior> nearbyEnemies = new();


    public override void Start()
    {
        base.Start();
        maxAttackCD = attackCD;
    }



    public override void Update()
    {
        base.Update();
        
    }



    public override void HandleEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Add the enemy to the attacking list
            attacking = true;
            nearbyEnemies.Add(collision.gameObject.GetComponent<Behavior>());
        }
    }


    public override void HandleExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Remove enemy that can be attacked by looking for the id
            int index = nearbyEnemies.FindIndex(e => e.name == collision.name);
            if (index == -1)
                return;

            nearbyEnemies.RemoveAt(index);

            // Disable attacking if there is no enemy
            if (nearbyEnemies.Count == 0)
                attacking = false;
        }
    }
}
