using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Willow : AttackTreeInterface
{
    public override void LaunchAttack(Behavior targetEnemy)
    {
        if (bulletPrefab != null) {
            if (currentLevel == 1)
                CreateBullet(targetEnemy);
            else
                StartCoroutine(ShootTwice(targetEnemy));
        }
        
        animator.Play("TreeAttack");
    }

    private IEnumerator ShootTwice(Behavior targetEnemy)
    {
        CreateBullet(targetEnemy, 0.75f);
        yield return new WaitForSeconds(0.125f);
        CreateBullet(targetEnemy, 0.75f);
    }


    private void CreateBullet(Behavior targetEnemy, float reducedDamage = 0f)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        BulletEffect effect = bullet.GetComponent<BulletEffect>();

        bullet.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
        bullet.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);
        effect.setDamage((damage - reducedDamage) * extraDamage);
        effect.SetTargetEnemy(targetEnemy);
        effect.SetSlowDown(currentLevel == 3);
    }
}
