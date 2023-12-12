using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// -------------------------------------------------------------------------



public class AttackTreeInterface : IvyInterface
{

    [SerializeField]
    protected float damage = 0.0f;
    [SerializeField]
    protected float attackCD = 0.0f;
    protected float maxAttackCD = 0.0f;

    [SerializeField]
    protected GameObject bulletPrefab;
    protected GameObject bulletContainer;

    protected bool attacking = false;
    protected List<PlayerTemp> nearbyEnemies = new ();



    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        bulletContainer = GameObject.Find("BulletContainer");
    }


    public override void Update()
    {
        if (!attacking)
        {
            attackCD = maxAttackCD;
            return;
        }

        attackCD += Time.deltaTime;
        if (attackCD > maxAttackCD)
        {
            attackCD = 0.0f;
            LaunchAttack();
        }
    }

    // -------------------------------------------------------------------------

    public override void OnTriggerEnter2D(Collider2D coll)
    {
        // Add the enemy to the attacking list
        PlayerTemp enemy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTemp>();
        attacking = true;
        nearbyEnemies.Add(enemy);
    }


    public override void OnTriggerExit2D(Collider2D coll)
    {
        // Remove enemy that can be attacked by looking for the id
        int index = nearbyEnemies.FindIndex(e => e.name == coll.name);
        if (index == -1)
            return;

        nearbyEnemies.RemoveAt(index);

        // Disable attacking if there is no enemy
        if (nearbyEnemies.Count == 0)
            attacking = false;
    }


    public void FinishAttackAnim()
    {
        animator.Play("TreeIdle");
    }


    public virtual void LaunchAttack()
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = bulletContainer.GetComponent<Transform>().transform;
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.SetTargetEnemy(nearbyEnemies[0]);
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
