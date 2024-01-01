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
    protected List<Behavior> nearbyEnemies = new ();

    protected float extraDamage = 1f;
    protected float extraSpeed = 0.0f;
    protected float buffNegCD = 2.0f;       // Subtracting instead of adding



    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        maxAttackCD = attackCD;
        bulletContainer = GameObject.Find("BulletContainer");
    }


    public override void Update()
    {
        base.Update();

        buffNegCD -= Time.deltaTime;
        if (buffNegCD < 0f)
        {
            buffNegCD = 2f;
            extraDamage = 1f;
            extraSpeed = 0f;
        }

        if (!attacking)
            return;

        attackCD += Time.deltaTime;
        if (attackCD > maxAttackCD - extraSpeed)
        {
            attackCD = 0.0f;
            LaunchAttack();
        }
    }

    // -------------------------------------------------------------------------

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


    public override void BeAttacked(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            // Restore the button to the map
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);
            GameObject.Find("MapManager").GetComponent<MapManager>().RemoveAttackObserver(this);
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
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


    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraDamage += extraDamage;
        this.extraSpeed += extraSpeed;
    }
}
