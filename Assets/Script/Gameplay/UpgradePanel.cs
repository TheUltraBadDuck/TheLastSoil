using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public bool IsReadyForNextWave()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    void Update()
    {
        // Check for the "C" key press
        if (IsReadyForNextWave())
        {
            // Do something when the "C" key is pressed
            Debug.Log("UpgradePanel is ready for the next wave!");
        }
    }
}
