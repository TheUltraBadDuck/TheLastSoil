using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMushroom : BuffTreeInterface
{
    private AudioSource effectSound;

    private int extraHealthL2 = 0;
    private float extraTimeL2 = 0f;

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
        if (buffCD > maxBuffCD - extraTimeL2)
        {
            buffCD = 0.0f;
            animator.Play("MakeEnergy");
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
                tree.GetComponent<IvyInterface>().BeHealed(10 + extraHealthL2);

                // Effect
                GameObject effect = Instantiate(treeEffect);
                effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                effect.transform.localPosition = new Vector3(tree.transform.position.x, tree.transform.position.y, 0.0f);
            }
        }

        effectSound.Play();
    }


    public override void BeAttacked(float damage)
    {
        if (gameObject == null)
            return;

        hp -= damage;

        if (blood != null)
            Instantiate(blood, gameObject.transform.position, Quaternion.identity);

        if (hp <= 0)
        {
            // Restore the button to the map
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);
            // Heal all trees around
            if (currentLevel == 3)
            {
                GameObject[] treeList = GameObject.FindGameObjectsWithTag("Ivy");
                foreach (var tree in treeList)
                {
                    if (tree.name == gameObject.name)
                        continue;

                    if (Vector2.Distance(tree.transform.position, transform.position) < range)
                    {
                        tree.GetComponent<IvyInterface>().BeHealed(180);

                        // Effect
                        for (int step = 0; step < 3; step++)
                        {
                            GameObject effect = Instantiate(treeEffect);
                            effect.transform.SetParent(bulletContainer.GetComponent<Transform>().transform);
                            effect.transform.localPosition = new Vector3(tree.transform.position.x, tree.transform.position.y, 0.0f);
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }


    public void FinishGeneratingAnim()
    {
        animator.Play("TreeIdle");
    }



    public override void SetTreeLevel(int currentLevel = 1)
    {
        this.currentLevel = currentLevel;
        if (currentLevel == 3)
        {
            extraHealthL2 = 2;
            extraTimeL2 = 2f;
        }
    }


    public override void UpgradeLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, 3);
        if (currentLevel == 3)
        {
            extraHealthL2 = 2;
            extraTimeL2 = 2f;
        }
    }
}
