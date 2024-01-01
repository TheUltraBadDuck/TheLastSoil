using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// -------------------------------------------------------------------------



public class EnergyTreeInterface : IvyInterface
{

    private float maxGeneratingCD = 12.0f;
    private float generatingCD = 7.0f;

    public GameObject coinPrefab; // Load EnergyCoin prefab
    private GameObject coinContainer;


    // -------------------------------------------------------------------------


    public override void Start()
    {
        base.Start();
        maxGeneratingCD = Random.Range(10.0f, 15.0f);
        coinContainer = GameObject.Find("CoinContainer");
    }

    public override void Update()
    {
        base.Start();
        generatingCD += Time.deltaTime;
        if (generatingCD > maxGeneratingCD)
        {
            generatingCD = 0;
            maxGeneratingCD = Random.Range(10.0f, 15.0f);
            animator.Play("MakeEnergy");
        }
    }


    // -------------------------------------------------------------------------

    public void FinishGeneratingAnim()
    {
        animator.Play("TreeIdle");
    }


    // Generate Coin
    public void MakeEffect(int scale = 1)
    {
        //EnergyCoin coin = new EnergyCoin();
        for (int i = 0; i < scale; i++)
        {
            GameObject coinObj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinObj.transform.parent = coinContainer.GetComponent<Transform>().transform;
            coinObj.transform.localPosition = new Vector3(transform.position.x + Random.Range(-0.10f, 0.10f), transform.position.y - 0.15f, 0.0f);
        }
    }
}
