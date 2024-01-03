using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEyes : AttackTreeInterface
{
    public override void LaunchAttack(Behavior targetEnemy)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
            bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

            BulletEffect effect = bullet.GetComponent<BulletEffect>();
            effect.setDamage((damage + ((currentLevel > 2) ? 0f : 1.2f)) * extraDamage);
            effect.SetTargetEnemy(targetEnemy);
        }

        // Play animation
        animator.Play("TreeAttack");
    }


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 3)
        {
            shorteningTimer = 0.15f;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 3)
        {
            shorteningTimer = 0.15f;
        }
    }
}
