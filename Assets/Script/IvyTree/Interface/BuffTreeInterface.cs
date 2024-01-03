using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTreeInterface : IvyInterface
{
    [SerializeField]
    protected float range = 0.6f;

    [SerializeField]
    protected float buffCD = 0.0f;
    protected float maxBuffCD = 0.0f;

    [SerializeField]
    protected GameObject treeEffect;
    protected GameObject bulletContainer;

    // -------------------------------------------------------------------------

    public override void Start()
    {
        base.Start();
        maxBuffCD = buffCD;
        bulletContainer = GameObject.Find("BulletContainer");
    }
}
