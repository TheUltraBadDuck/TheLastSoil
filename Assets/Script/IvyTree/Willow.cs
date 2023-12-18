using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Willow : AttackTreeInterface
{
    public override void LaunchAttack()
    {
        // Summon the bullet
        if (bulletPrefab != null)
        {
            nearbyEnemies.Sort(delegate (Behavior x, Behavior y)
            {
                // return (x.GetDistanceToHoffen() < y.GetDistanceToHoffen()) ? -1 : 1;
                return (x.GetDistance(this) < y.GetDistance(this)) ? -1 : 1;
            });

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
