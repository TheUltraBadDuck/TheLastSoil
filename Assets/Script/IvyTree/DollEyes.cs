using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEyes : AttackTreeInterface
{
    private PlayerTemp firstEnemy;  // 3 enemies
    private PlayerTemp secondEnemy;
    private PlayerTemp thirdEnemy;


    public override void Start()
    {
        base.Start();

        treeId = 0;
        treeName = "Claw tree";

        hp = 5;
        maxhp = hp;

        damage = 1;

        attackCD = 1.0f;
        maxAttackCD = 1.0f;
    }


    public override void LaunchAttack()
    {
        // Summon the bullet
        Debug.Log("Attack 5 enemies with each damage = " + damage);

        //for (int i = 0; i < ; i++)
        //{
        //    if (arr[i] > first)
        //    {
        //        third = second;
        //        second = first;
        //        first = arr[i];
        //    }

        //    else if (arr[i] > second && arr[i] != first)
        //    {
        //        third = second;
        //        second = arr[i];
        //    }

        //    else if (arr[i] > third && arr[i] != second)
        //        third = arr[i];
        //}

        // Play animation
        animator.Play("TreeAttack");
    }
}
