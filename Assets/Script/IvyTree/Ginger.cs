using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ginger : DefenseTreeInterface
{
    [SerializeField]
    private GameObject bombPrefab;
    protected GameObject bulletContainer;
    [SerializeField]
    private float damage = 0.0f;

    private float extraDamage = 1f;
    private float buffNegCD = 2.0f;


    public override void Start()
    {
        base.Start();
        bulletContainer = GameObject.Find("BulletContainer");
    }


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


    public override void BeAttacked(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);

            GameObject effect = Instantiate(bombPrefab);
            effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
            effect.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
            effect.GetComponent<BuffectExplosion>().setDamage((this.damage + ((currentLevel == 3) ? 15f : 0f)) * extraDamage);

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


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 2)
        {
            hp += 60;
            maxhp += 60;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 2)
        {
            hp += 60;
            maxhp += 60;
        }
    }
}
