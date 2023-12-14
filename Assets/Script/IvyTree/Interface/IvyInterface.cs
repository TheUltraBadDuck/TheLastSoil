using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvyInterface : MonoBehaviour
{
    // Tree's status
    [SerializeField]
    protected int treeId = -1;
    [SerializeField]
    protected string treeName = "[None]";
    [SerializeField]
    protected int hp = 0;
    [SerializeField]
    protected int maxhp = 0;
    [SerializeField]
    protected Vector2 position = Vector2.zero;

    // Tree's support
    protected Animator animator;



    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {

    }



    public virtual void HandleEnter2D(Collider2D coll)
    {
        Debug.Log(treeName + " is ready to attack.");
    }


    public virtual void HandleExit2D(Collider2D coll)
    {
        Debug.Log(treeName + " stops attacking.");
    }
}
