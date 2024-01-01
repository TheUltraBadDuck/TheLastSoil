using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public void OnPressed()
    {
        GameObject.Find("MapManager").GetComponent<MapManager>().OnShovelPressed();
    }
}
