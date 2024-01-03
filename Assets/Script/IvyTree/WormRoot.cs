using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
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
>>>>>>> Stashed changes
    }
}
