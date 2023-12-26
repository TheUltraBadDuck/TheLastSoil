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
    private SpriteRenderer whiteTreeRenderer;

    // When attacked
    private float hurtCD = 0.25f;
    private float hurtTimer = 0f;
    protected bool attacked = false;



    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        whiteTreeRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        // Blink
        if (attacked)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer < hurtCD / 2)
            {
                whiteTreeRenderer.color = new Color(1, 1, 1, hurtTimer / hurtCD * 2);
            }
            else if (hurtTimer < hurtCD)
            {
                whiteTreeRenderer.color = new Color(1, 1, 1, (hurtCD - hurtTimer) / hurtCD * 2);
            }
            else
            {
                hurtTimer = 0f;
                attacked = false;
            }
        }
    }



    public virtual void HandleEnter2D(Collider2D coll)
    {
        Debug.Log(treeName + " is ready to attack.");
    }


    public virtual void HandleExit2D(Collider2D coll)
    {
        Debug.Log(treeName + " stops attacking.");
    }


    public virtual void RemoveEnemy(Behavior enemy)
    {

    }

    public virtual void BeAttacked(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }
}
