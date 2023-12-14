using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteChilli : DefenseTreeInterface
{
    [SerializeField]
    protected float damage = 0.0f;

    private bool attacking = false;


    //public override void HandleEnter2D(Collider2D coll)
    //{
    //    base.HandleEnter2D(coll);
    //    attacking = true;
    //    playerObjs = GameObject.FindGameObjectsWithTag("Player");
    //    animator.Play("TreeAttack");
    //    for (int i = 0; i < playerObjs.Length; i++)
    //        Debug.Log(playerObjs[i].GetComponent<Behavior>().velocity);
    //}

    //public override void HandleExit2D(Collider2D coll)
    //{
    //    base.HandleExit2D(coll);
    //    attacking = false;
    //    playerObjs = null;
    //    animator.Play("TreeIdle");
    //}
}
