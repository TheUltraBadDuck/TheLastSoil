using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mangrove : BuffTreeInterface
{
    [SerializeField]
    private float extraDamage = 0.5f;
    [SerializeField]
    private float extraSpeed = 0.25f;


    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        maxBuffCD = buffCD;
        bulletContainer = GameObject.Find("BulletContainer");
    }


    public override void Update()
    {
        base.Update();

        buffCD += Time.deltaTime;
        if (buffCD > maxBuffCD)
        {
            buffCD = 0.0f;
            MakeEffect();
        }
    }


    // -------------------------------------------------------------------------

    // Heal all trees around
    public void MakeEffect(int scale = 1)
    {
        GameObject[] treeList = GameObject.FindGameObjectsWithTag("Ivy");

        foreach (var tree in treeList)
        {
            if (tree.name == gameObject.name)
                continue;

            if (Vector2.Distance(tree.transform.position, transform.position) < range)
            {
                tree.GetComponent<IvyInterface>().BeBuff(extraDamage, extraSpeed);

                // Effect
                GameObject effect = Instantiate(treeEffect);
                effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                effect.transform.localPosition = new Vector3(tree.transform.position.x, tree.transform.position.y, 0.0f);
            }
        }
    }


    public void FinishGeneratingAnim()
    {
        animator.Play("TreeIdle");
    }
}
