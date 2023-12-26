using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WitheredPalmTree : AttackTreeInterface
{
    public override void LaunchAttack()
    {
        // Summon the bullet
        Debug.Log("Attack enemy with damage = " + damage);

        // Play animation
        animator.Play("TreeAttack");
    }
}
