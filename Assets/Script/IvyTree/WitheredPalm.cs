using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WitheredPalmTree : AttackTreeInterface
{
    protected PlayerTemp playerTemp;  // One enemy

    public override void Start()
    {
        base.Start();
        treeId = 0;
        treeName = "Withered Palm tree";

        hp = 5;
        maxhp = hp;

        damage = 5;

        attackCD = 3.0f;
        maxAttackCD = 3.0f;
    }

    public override void LaunchAttack()
    {
        // Summon the bullet
        Debug.Log("Attack enemy with damage = " + damage);

        // Play animation
        animator.Play("TreeAttack");
    }
}
