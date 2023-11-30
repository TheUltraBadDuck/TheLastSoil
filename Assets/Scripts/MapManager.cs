using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static float GetDistanceToHoffen(PlayerTemp enemy)
    {
        return Vector3.Distance(enemy.transform.position, new Vector3(0, 0, 0));
    }






}
