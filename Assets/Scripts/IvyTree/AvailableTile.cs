using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableTile : MonoBehaviour
{
    public int this_i = 0;
    public int this_j = 0;
    public bool pressable = false;

    private MapManager mapManager;


    public void SetMapManager(MapManager mapManager)
    {
        this.mapManager = mapManager;
    }


    private void OnMouseDown()
    {
        if (pressable)
        {
            Debug.Log("Pressing at [" + this_i + ", " + this_j + "]");
            mapManager.PutTree(this_i, this_j);
        }
    }
}
