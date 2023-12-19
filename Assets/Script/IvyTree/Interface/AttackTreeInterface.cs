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
    protected Queue<Behavior> nearbyEnemies = new Queue<Behavior>();



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

        // Check if there are nearby enemies in the queue
        bool hasNearbyEnemies = nearbyEnemies.Count > 0;

        if (hasNearbyEnemies && !attacking)
        {
            // If there are nearby enemies in the queue and not currently attacking, start attacking
            attacking = true;
            LaunchNextAttack();
        }

        // Continue with the existing logic if needed
        if (attacking)
        {
            // Check if the current target is still valid
            if (currentTarget != null)
            {
                attackCD += Time.deltaTime;

                if (attackCD >= maxAttackCD)
                {
                    // If the cooldown is reached, launch an attack
                    LaunchAttack(currentTarget);
                    attackCD = 0.0f; // Reset the cooldown
                }
            }
            else
            {
                // Current target is null (destroyed), stop attacking
                attacking = false;
                currentTarget = null;
            }
        }
        else
        {
            // If there are no nearby enemies in the queue, reset the attack cooldown
            attackCD = maxAttackCD;
        }
    }

    // -------------------------------------------------------------------------

    private Behavior currentTarget;

    public override void HandleEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Enqueue the enemy
            nearbyEnemies.Enqueue(collision.gameObject.GetComponent<Behavior>());

            // If not attacking, start attacking the first enemy in the queue
            if (!attacking)
            {
                attacking = true;
                LaunchNextAttack();
            }
        }
    }

    private void LaunchNextAttack()
    {
        if (nearbyEnemies.Count > 0)
        {
            // Dequeue the next enemy from the queue
            currentTarget = nearbyEnemies.Dequeue();
            LaunchAttack(currentTarget);
        }
        else
        {
            // No more enemies in the queue, stop attacking
            attacking = false;
            currentTarget = null;
        }
    }

    public virtual void LaunchAttack(Behavior targetEnemy)
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = bulletContainer.GetComponent<Transform>().transform;
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.SetTargetEnemy(targetEnemy);
        }

        // Play animation
        animator.Play("TreeAttack");
    }

    public override void BeAttacked(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            GameObject.Find("MapManager").GetComponent<MapManager>().RemoveAttackObserver(this);
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }
}
