using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodenGrave : DefenseTreeInterface
{
    private float healthCD = 0f;
    private float maxHealthCD = 10f;
    private bool healing = false;


    [SerializeField]
    private GameObject healingEffect;
    protected GameObject bulletContainer;

    public override void Start()
    {
        base.Start();
        bulletContainer = GameObject.Find("BulletContainer");
    }


    public override void Update()
    {
        base.Update();

        if (healing)
        {
            healthCD += Time.deltaTime;
            if (healthCD > maxHealthCD)
            {
                healthCD = 0f;
                BeHealed(10);

                GameObject effect = Instantiate(healingEffect);
                effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                effect.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
            }
        }
    }


    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 2)
        {
            hp += 90;
            maxhp += 90;
        }
        else if (currentLevel == 3)
        {
            healing = true;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 2)
        {
            hp += 90;
            maxhp += 90;
        }
        else if (currentLevel == 3)
        {
            healing = true;
        }
    }
}
