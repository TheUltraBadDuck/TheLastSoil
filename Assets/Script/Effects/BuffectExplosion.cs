using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffectExplosion : MonoBehaviour
{
    public Behavior targetEnemy;
    public float damage = 0;

    public void Explode()
    {
        Destroy(gameObject);
    }

    public float getDamage()
    {
        return damage;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // If collider is an enemy
       // if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && (targetEnemy != collision.gameObject.GetComponent<Behavior>()))
       // {
            //collision.gameObject.GetComponent<Behavior>();
       // }
    }
}
