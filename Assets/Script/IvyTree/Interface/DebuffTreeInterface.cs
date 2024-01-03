using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTreeInterface : IvyInterface
{
    [SerializeField]
    protected float range = 0.6f;

    [SerializeField]
    protected GameObject treeEffect;
    protected GameObject bulletContainer;

    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        bulletContainer = GameObject.Find("BulletContainer");
    }
}
