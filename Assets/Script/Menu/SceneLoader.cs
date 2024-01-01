using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject MainMenu;
    public Slider loadingSlider;

    public void LoadLevelBtn(string levelToLoad)
    {
        StartCoroutine(LoadlevelASync(levelToLoad));
    }

    IEnumerator LoadlevelASync(string levelToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

        loadingScreen.SetActive(true);
        MainMenu.SetActive(false);
        while(!operation.isDone)
        {
            loadingSlider.value = operation.progress;
            yield return null;
        }
    }
}
