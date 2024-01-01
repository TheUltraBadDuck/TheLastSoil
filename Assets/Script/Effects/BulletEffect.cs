using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletEffect : MonoBehaviour
{
    protected Behavior targetEnemy;
    [SerializeField]
    protected GameObject effectPrefab;
    protected GameObject bulletContainer;
    [SerializeField]
    protected float damage = 1;

    protected bool launching = false;
    protected float speed = 3.0f;




    public void SetTargetEnemy(Behavior targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }
    public void setDamage(float dame)
    {
        damage = dame;
    }

    protected virtual void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer");
    }


    protected virtual void Update()
    {
        if (launching)
        {
            // Moving the bullet
            if (targetEnemy == null) {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = Vector3.Normalize(targetEnemy.transform.position - transform.position);
            transform.localPosition += speed * Time.deltaTime * direction;

            // Rotating the bullet
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180.0f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // The bullet is near the enemy: explode
            if (Vector3.Distance(transform.localPosition, targetEnemy.transform.position) < 0.2)
            {
                GameObject effect = Instantiate(effectPrefab);
                effect.GetComponent<BuffectExplosion>().setDamage(damage);
                effect.GetComponent<BuffectExplosion>().targetEnemy = targetEnemy;
                //effect.GetComponent<BuffectExplosion>().damage = damage;
                effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                effect.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
                Destroy(gameObject);
            }
        }
    }

    public void Launch()
    {
        launching = true;
    }
}
