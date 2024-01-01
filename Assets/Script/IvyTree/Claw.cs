using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : AttackTreeInterface
{
    public Claw()
    {
        SetLevelDescription(new string[]
        {
            "Unlock Claw in the shop.",
            "Claw' attacks get a bigger contact area.",
            "Claw now have two projectiles each attack."
        });
    }
    public override void LaunchAttack(Behavior targetEnemy)
    {
        Debug.Log("Attatck");
        if (bulletPrefab != null)
        {
            if (currentLevel == 2)
            {
                // Upgrade to 3-in-1 attack with hat-shaped spread
                float angleBetweenBullets = 0.05f;

                // Middle bullet
                CreateBulletWithOffset(targetEnemy, 0.0f);

                // Left bullet
                CreateBulletWithOffset(targetEnemy, -angleBetweenBullets);

                // Right bullet
                CreateBulletWithOffset(targetEnemy, angleBetweenBullets);
            }
            else
            {
                // Single bullet attack
                CreateBulletWithOffset(targetEnemy, 0.0f);
            }
        }

        // Play animation
        animator.Play("TreeAttack");
    }

    private void CreateBulletWithOffset(Behavior targetEnemy, float angleOffset)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);

        // Calculate the offset based on the angle
        Vector3 offset = Quaternion.Euler(0, 0, angleOffset) * Vector3.right;

        bullet.transform.localPosition = new Vector3(transform.position.x + angleOffset,
                                                     transform.position.y + 0.2f,
                                                     0.0f);

        BulletEffect effect = bullet.GetComponent<BulletEffect>();
        effect.setDamage(damage);
        effect.SetTargetEnemy(targetEnemy);
    }
}
