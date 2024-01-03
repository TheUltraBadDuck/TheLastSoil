using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {

    }

    // Update is called once per frame
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
}
