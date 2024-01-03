using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : DefenseTreeInterface
{
    [SerializeField]
    protected float damage = 0.0f;
    [SerializeField]
    protected float attackCD = 0.0f;
    protected float maxAttackCD = 0.0f;

    bool attacking = false;
    protected List<Behavior> nearbyEnemies = new();

    private float extraDamage = 1f;
    private float extraSpeed = 0.0f;
    private float buffNegCD = 2.0f;

<<<<<<< Updated upstream
=======
    private float extraDamageLvl2 = 0f;

>>>>>>> Stashed changes

    public override void Start()
    {
        base.Start();
        maxAttackCD = attackCD;
    }

    public float getDamage()
    {
        return damage * extraDamage;
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



    public override void RemoveEnemy(Behavior enemy)
    {
        HandleExit2D(enemy.GetComponent<Collider2D>());
        nearbyEnemies.RemoveAll(item => item == null);
    }



    public override void BeAttacked(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            // Restore the button to the map
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }



    public void FinishAttackAnim()
    {
        animator.Play("TreeIdle");
    }


    public virtual void LaunchAttack()
    {
        Debug.Log(nearbyEnemies.Count);
        foreach (var enemy in nearbyEnemies)
        {
<<<<<<< Updated upstream
            enemy.gameObject.GetComponent<Behavior>().GetDamageRaw(damage * extraDamage);
=======
            if (Vector2.Distance(enemy.transform.position, transform.position) < attackRange)
            {
                enemy.GetComponent<Behavior>().MakeDamage((damage + extraDamageLvl2) * extraDamage);
                haveEnemy = true;
            }
>>>>>>> Stashed changes
        }

        // Play animation
        animator.Play("TreeObstruct");
    }


    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraDamage += extraDamage;
        this.extraSpeed += extraSpeed;
    }
<<<<<<< Updated upstream
=======


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 2)
        {
            extraDamageLvl2 = 0.2f;
        }
        else if (currentLevel == 3)
        {
            attackRange += 0.15f;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 2)
        {
            extraDamageLvl2 = 0.2f;
        }
        else if (currentLevel == 3)
        {
            attackRange += 0.15f;
        }
    }
>>>>>>> Stashed changes
}
