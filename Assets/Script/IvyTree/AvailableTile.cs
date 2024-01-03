using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class AvailableTile : MonoBehaviour
{
    public int this_i = 0;
    public int this_j = 0;
    public bool pressable = false;

    private MapManager mapManager;


    public void SetPressable(bool value)
    {
        pressable = value;
        GetComponent<SpriteRenderer>().color =
            pressable ? new Color(0.33f, 0.5f, 0.33f, 0.5f) : new Color(0.33f, 0.5f, 0.33f, 0f);
    }


    public void SetMapManager(MapManager mapManager)
    {
        this.mapManager = mapManager;
    }


    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Sprite shadowSprite = mapManager.GetCursorShadow().sprite;

        if (pressable && (shadowSprite != null))
        {
            // If it is a shovel
            if (shadowSprite.name == mapManager.GetShovel().name)
            {
                if (mapManager.GetTree(this_i, this_j) != null)
                    mapManager.RemoveTree(this_i, this_j);
            }
            else
            {
                // If it is an empty cell
                if (mapManager.GetTree(this_i, this_j) == null)
                    mapManager.PutTree(this_i, this_j);
            }
        }
    }
}
