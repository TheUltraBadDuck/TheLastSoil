using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightBulb : EnergyTreeInterface
{
    // Generate Coin
    public override void MakeEffect(int scale = 1)
    {
        int extraScale = 0;
        if (currentLevel == 2)
            extraScale = Random.Range(1, 4) / 4;
        else if (currentLevel == 3)
            extraScale = Random.Range(0, 1);

        //EnergyCoin coin = new EnergyCoin();
        for (int i = 0; i < scale + extraScale; i++)
        {
            GameObject coinObj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinObj.transform.parent = coinContainer.GetComponent<Transform>().transform;
            coinObj.transform.localPosition = new Vector3(transform.position.x + Random.Range(-0.10f, 0.10f), transform.position.y - 0.15f, 0.0f);
        }
    }
}
