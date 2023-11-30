using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : DefenseTreeInterface
{
    private bool attacking = false;
    private GameObject[] playerObjs;  // Enemies

    public override void Start()
    {
        base.Start();
        treeId = 0;
        treeName = "Cactus tree";
        hp = 30;
        maxhp = hp;
    }

    public override void OnTriggerEnter2D(Collider2D coll)
    {
        attacking = true;
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        animator.Play("TreeAttack");
    }

    public override void OnTriggerExit2D(Collider2D coll)
    {
        attacking = false;
        playerObjs = null;
        animator.Play("TreeIdle");
    }
}
