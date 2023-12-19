using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : AttackTreeInterface
{
    private void LaunchAttack(Behavior targetEnemy)
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

                BulletEffect effect = bullet.GetComponent<BulletEffect>();
                effect.SetTargetEnemy(targetEnemy);
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
