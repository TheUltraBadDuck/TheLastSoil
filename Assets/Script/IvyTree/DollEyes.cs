using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEyes : AttackTreeInterface
{
    public override void LaunchAttack()
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            Behavior nearestEnemy = GetNearestEnemy();

            if (nearestEnemy != null)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

                BulletEffect effect = bullet.GetComponent<BulletEffect>();
                effect.SetTargetEnemy(nearestEnemy);
            }
            else
            {
                Debug.LogWarning("No nearby enemies found.");
            }
        }

        // Play animation
        animator.Play("TreeAttack");
    }

    private Behavior GetNearestEnemy()
    {
        float nearestDistance = float.MaxValue;
        Behavior nearestEnemy = null;

        foreach (Behavior enemy in nearbyEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
