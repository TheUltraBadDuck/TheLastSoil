using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Willow : AttackTreeInterface
{
    public Willow()
    {
        SetLevelDescription(new string[]
        {
            "Unlock Willow in the shop.",
            "Willow' attacks get a bigger contact area.",
            "Willow now have two projectiles each attack."
        });
    }
    public override void LaunchAttack(Behavior targetEnemy)
    {
        if (bulletPrefab != null)
        {

            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.setDamage(damage * extraDamage);
            effect.SetTargetEnemy(targetEnemy);
        }

        // Play animation
        animator.Play("TreeAttack");
    }
}
