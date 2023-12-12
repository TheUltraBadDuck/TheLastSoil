using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Willow : AttackTreeInterface
{
    public override void Start()
    {
        base.Start();
        treeId = 0;
        treeName = "Willow tree";

        hp = 5;
        maxhp = hp;

        damage = 3;

        attackCD = 1.0f;
        maxAttackCD = 1.0f;
    }


    public override void LaunchAttack()
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.SetTargetEnemy(nearbyEnemies[0]);
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
