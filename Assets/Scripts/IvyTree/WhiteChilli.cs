using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteChilli : DefenseTreeInterface
{
    [SerializeField]
    protected float damage = 0.0f;

    private bool attacking = false;
    
    private GameObject[] playerObjs;  // Enemies

    public override void Start()
    {
        base.Start();
        treeId = 0;
        treeName = "White chilli tree";
        hp = 30;
        maxhp = hp;
        damage = 1;
    }

    public override void HandleEnter2D(Collider2D coll)
    {
        base.HandleEnter2D(coll);
        attacking = true;
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        animator.Play("TreeAttack");
        for (int i = 0; i < playerObjs.Length; i++)
            Debug.Log(playerObjs[i].GetComponent<PlayerTemp>().velocity);
    }

    public override void HandleExit2D(Collider2D coll)
    {
        base.HandleExit2D(coll);
        attacking = false;
        playerObjs = null;
        animator.Play("TreeIdle");
    }
}
