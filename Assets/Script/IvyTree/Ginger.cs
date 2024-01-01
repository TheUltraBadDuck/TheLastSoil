using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ginger : DefenseTreeInterface
{
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private float damage = 0.0f;

    private float extraDamage = 1f;
    private float buffNegCD = 2.0f;


    public override void Update()
    {
        base.Update();

        buffNegCD -= Time.deltaTime;
        if (buffNegCD < 0f)
        {
            buffNegCD = 2f;
            extraDamage = 1f;
        }
    }


    public override void BeAttacked(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);

            GameObject effect = Instantiate(bombPrefab);
            effect.transform.SetParent(bombPrefab.GetComponent<Transform>().transform);
            effect.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
            effect.GetComponent<BuffectExplosion>().SetDamage(this.damage * extraDamage);

            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }


    public override void BeBuff(float extraDamage, float extraSpeed)
    {
        this.extraDamage += extraDamage;
        buffNegCD = 2f;
    }
}
