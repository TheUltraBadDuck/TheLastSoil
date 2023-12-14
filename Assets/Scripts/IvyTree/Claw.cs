using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : AttackTreeInterface
{
    public override void LaunchAttack()
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            // Get 3 nearest enemies
            nearbyEnemies.Sort(delegate (PlayerTemp x, PlayerTemp y)
            {
                return (x.GetDistance() < y.GetDistance()) ? -1 : 1;
            });

            for (int i = 0; i < Mathf.Min(3, nearbyEnemies.Count); i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

                BulletEffect effect = bullet.GetComponent<BulletEffect>();
                effect.SetTargetEnemy(nearbyEnemies[i]);
            }
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
