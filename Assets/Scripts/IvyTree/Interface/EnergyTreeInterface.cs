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
        generatingCD += Time.deltaTime;
        if (generatingCD > maxGeneratingCD)
        {
            generatingCD = 0;
            maxGeneratingCD = Random.Range(10.0f, 15.0f);
            SummonEnergy();
        }
    }


    // -------------------------------------------------------------------------

    public void FinishGeneratingAnim()
    {
        animator.Play("TreeIdle");
    }

    public void SummonEnergy()
    {
        animator.Play("MakeEnergy");
    }

    public void CreateEnergyCoin(int amount = 1)
    {
        //EnergyCoin coin = new EnergyCoin();
        for (int i = 0; i < amount; i++)
        {
            GameObject coinObj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinObj.transform.parent = coinContainer.GetComponent<Transform>().transform;
            coinObj.transform.localPosition = new Vector3(transform.position.x + Random.Range(-0.10f, 0.10f), transform.position.y - 0.15f, 0.0f);
        }
    }
}
