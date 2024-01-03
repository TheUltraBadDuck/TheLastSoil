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
    [SerializeField]
    protected float attackRange = 0.25f;

    private float extraDamage = 1f;
    private float extraSpeed = 0.0f;
    private float buffNegCD = 2.0f;

    private float extraDamageLvl2 = 0f;
    private float extraRangeLvl3 = 0f;


    public override void Start()
    {
        base.Start();
        maxAttackCD = attackCD;
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

        attackCD += Time.deltaTime;
        if (attackCD > maxAttackCD - extraSpeed)
        {
            attackCD = 0.0f;
            LaunchAttack();
        }
    }



    public override void BeAttacked(float damage)
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



    public void FinishAttackAnim()
    {
        animator.Play("TreeIdle");
    }


    public virtual void LaunchAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        bool haveEnemy = false;

        foreach (var enemy in enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < attackRange)
            {
                enemy.GetComponent<Behavior>().MakeDamage(damage * extraDamage);
                haveEnemy = true;
            }
        }

        // Play animation
        if (haveEnemy)
           animator.Play("TreeObstruct");
    }


    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraDamage += extraDamage;
        this.extraSpeed += extraSpeed;
    }


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 2)
        {
            extraDamageLvl2 = 0.2f;
        }
        else if (currentLevel == 3)
        {
            extraRangeLvl3 = 0.15f;
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
            extraRangeLvl3 = 0.15f;
        }
    }
}
