using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// -------------------------------------------------------------------------



public class AttackTreeInterface : IvyInterface
{
    [SerializeField]
    protected float damage = 0.0f;
    [SerializeField]
    protected float attackCD;
    protected float maxAttackCD = 0.0f;

    protected float shorteningTimer = 0.0f;     // For upgrading

    [SerializeField]
    protected GameObject bulletPrefab;
    protected GameObject bulletContainer;

    protected bool attacking = false;
    protected Queue<Behavior> nearbyEnemies = new ();

    protected float extraDamage = 1f;
    protected float extraSpeed = 0.0f;
    protected float buffNegCD = 2.0f;

    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        maxAttackCD = attackCD;
        bulletContainer = GameObject.Find("BulletContainer");
    }
    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraDamage += extraDamage;
        this.extraSpeed += extraSpeed;
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

        // Check if there are nearby enemies in the queue
        if ((nearbyEnemies.Count > 0) && !attacking)
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
                if (attackCD >= (maxAttackCD - extraSpeed - shorteningTimer))
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
            Debug.Log(transform.position);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = bulletContainer.GetComponent<Transform>().transform;
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.setDamage(damage*extraDamage);
            if (targetEnemy != null)
            {
                effect.SetTargetEnemy(targetEnemy);
            }
            else
            {
                effect.SetTargetEnemy(null);
            }
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
