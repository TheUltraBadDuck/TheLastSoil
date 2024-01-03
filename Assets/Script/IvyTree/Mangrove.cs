using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mangrove : BuffTreeInterface
{
    private AudioSource effectSound;

    [SerializeField]
    private float extraDamage = 0.5f;
    [SerializeField]
    private float extraSpeed = 0.25f;


    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        effectSound = GetComponent<AudioSource>();
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
        if (gameObject == null)
            return;

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
        effectSound.Play();
    }


    public void FinishGeneratingAnim()
    {
        animator.Play("TreeIdle");
    }
}
