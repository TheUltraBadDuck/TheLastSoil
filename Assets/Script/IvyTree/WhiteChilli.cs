using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteChilli : DefenseTreeInterface
{
    [SerializeField]
    protected float damage = 0.0f;
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

        if (!attacking)
            return;

        attackCD += Time.deltaTime;
        if (attackCD > maxAttackCD)
        {
            attackCD = 0.0f;
            LaunchAttack();
        }
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



    public override void RemoveEnemy(Behavior enemy)
    {
        HandleExit2D(enemy.GetComponent<Collider2D>());
        nearbyEnemies.RemoveAll(item => item == null);
    }



    public void FinishAttackAnim()
    {
        animator.Play("TreeIdle");
    }


    public virtual void LaunchAttack()
    {
        foreach (var enemy in nearbyEnemies)
        {
            enemy.gameObject.GetComponent<Behavior>().GetDamageRaw(damage);
        }

        // Play animation
        animator.Play("TreeObstruct");
    }
}
