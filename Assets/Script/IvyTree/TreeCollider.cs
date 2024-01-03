using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    [SerializeField]
    private IvyInterface m_Interface;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
            m_Interface.HandleEnter2D(collision);
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
            m_Interface.HandleExit2D(collision);
    }

}
