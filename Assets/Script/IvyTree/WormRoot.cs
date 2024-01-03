using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WormRoot : DefenseTreeInterface
{
    [SerializeField]
    protected float attackCD = 0.0f;
    protected float maxAttackCD = 0.0f;
    [SerializeField]
    protected float attackRange = 0.25f;
    [SerializeField]
    protected GameObject wormPrefab;
    protected GameObject bulletContainer;

    private float extraSpeed = 0.0f;
    private float buffNegCD = 2.0f;


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
                // 67% rate to stun enemies
                if (Random.Range(0, 2) == 2)
                    continue;

                GameObject effect = Instantiate(wormPrefab);
                effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                effect.transform.localPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0.0f);

                enemy.GetComponent<Behavior>().SlowDown();

                haveEnemy = true;
            }
        }

        // Play animation
        if (haveEnemy)
            animator.Play("TreeObstruct");
    }


    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraSpeed += extraSpeed;
    }
}
