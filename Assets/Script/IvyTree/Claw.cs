using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : AttackTreeInterface
{
    private float reducedDamage = 0.0f;

    public override void LaunchAttack(Behavior targetEnemy)
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            if (currentLevel == 1)
            {
                // Single bullet attack
                CreateBulletWithOffset(targetEnemy, 0.0f);
            }
            else if (currentLevel == 2)
            {
                // Upgrade to 3-in-1 attack with hat-shaped spread
                float angleBetweenBullets = 0.05f;

                BulletEffect effect = bulletPrefab.GetComponent<BulletEffect>();
                effect.SetTargetEnemy(targetEnemy);

                CreateBulletWithOffset(targetEnemy, 0.0f);                      // Middle bullet
                CreateBulletWithOffset(targetEnemy, -angleBetweenBullets);      // Left bullet
                CreateBulletWithOffset(targetEnemy, angleBetweenBullets);       // Right bullet
            }
            else if (currentLevel == 3)
            {
                // Upgrade to 5-in-1 attack with hat-shaped spread
                float angleBetweenBullets = 0.05f;

                BulletEffect effect = bulletPrefab.GetComponent<BulletEffect>();
                effect.SetTargetEnemy(targetEnemy);

                CreateBulletWithOffset(targetEnemy, 0.0f);
                CreateBulletWithOffset(targetEnemy, -angleBetweenBullets);
                CreateBulletWithOffset(targetEnemy, angleBetweenBullets);
                CreateBulletWithOffset(targetEnemy, -2 * angleBetweenBullets);
                CreateBulletWithOffset(targetEnemy, 2 * angleBetweenBullets);
            }
        }

        // Play animation
        animator.Play("TreeAttack");
    }


    private void CreateBulletWithOffset(Behavior targetEnemy, float angleOffset)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        //bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);

        // Calculate the offset based on the angle
        Vector3 offset = Quaternion.Euler(0, 0, angleOffset) * Vector3.right;

        bullet.transform.localPosition = new Vector3(transform.position.x + angleOffset,
                                                     transform.position.y + 0.2f,
                                                     0.0f);

        BulletEffect effect = bullet.GetComponent<BulletEffect>();
        effect.setDamage((damage - reducedDamage) * extraDamage);
        effect.SetTargetEnemy(targetEnemy);
    }


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 2)
        {
            reducedDamage = 0.5f;
        }
        else if (currentLevel == 3)
        {
            reducedDamage = 0.4f;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 2)
        {
            reducedDamage = 0.5f;
        }
        else if (currentLevel == 3)
        {
            reducedDamage = 0.4f;
        }
    }
}

   
