using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : EnergyTreeInterface
{
    public override void Start()
    {
        base.Start();
        treeId = 1;
        treeName = "Light bulb tree";
        hp = 10;
        maxhp = hp;
    }
}
