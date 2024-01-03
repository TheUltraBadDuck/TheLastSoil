using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject coinUI;
    [SerializeField]
    private GameObject treeScroll;
    [SerializeField]
    private GameObject shovelBackground;
    [SerializeField]
    private GameObject waveTextCanvasGr;
    [SerializeField]
    private GameObject treeDescription;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Enemy_Spawner enemySpawner;
    [SerializeField]
    private GameObject blackScreen;
    [SerializeField]
    private GameObject gameOverButton;


    private float fadeCD = 0f;
    private float fadeTimer = 1.5f;

    private bool startGame = false;
    private bool finishmainMenuFadeOut = false;
    private bool finishUIGameFadeOut = false;
    private bool gameOver = false;



    private void Start()
    {
        coinUI.SetActive(false);
        treeScroll.SetActive(false);
        shovelBackground.SetActive(false);
        waveTextCanvasGr.SetActive(false);
        treeDescription.SetActive(false);

        mainMenu.SetActive(true);
    }


    private void Update()
    {
        // Press start
        if (startGame)
        {
            if (!finishmainMenuFadeOut)
            {
                fadeCD += Time.deltaTime;
                mainMenu.GetComponent<CanvasGroup>().alpha = 1f - fadeCD / fadeTimer;
                if (fadeCD > fadeTimer)
                {
                    fadeCD = 0;
                    finishmainMenuFadeOut = true;

                    mainMenu.SetActive(false);

                    coinUI.SetActive(true);
                    treeScroll.SetActive(true);
                    shovelBackground.SetActive(true);
                    waveTextCanvasGr.SetActive(true);
                    treeDescription.SetActive(true);

                    SetAlpha(0f);
                }
            }
            else if (!finishUIGameFadeOut)
            {
                fadeCD += Time.deltaTime;
                SetAlpha(fadeCD / fadeTimer);
                if (fadeCD > fadeTimer)
                {
                    fadeCD = 0;
                    finishUIGameFadeOut = true;
                    // Start Game
                    enemySpawner.StartNextWave();
                }
            }
            else if (gameOver)
            {
                fadeCD += Time.deltaTime;
                blackScreen.GetComponent<CanvasGroup>().alpha = fadeCD / fadeTimer;
                waveTextCanvasGr.GetComponent<CanvasGroup>().alpha = fadeCD / fadeTimer;
                gameOverButton.GetComponent<CanvasGroup>().alpha = fadeCD / fadeTimer;
                if (fadeCD > fadeTimer)
                {
                    fadeCD = 0;
                    gameOver = false;

                    // Start Game
                    enemySpawner.StartNextWave();
                }
            }
        }
    }



    public void StartGame()
    {
        startGame = true;
    }



    public void QuitGame()
    {
        Application.Quit();
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GameOver()
    {
        gameOver = true;
        blackScreen.SetActive(true);
        gameOverButton.SetActive(true);
        waveTextCanvasGr.SetActive(true);
        waveTextCanvasGr.GetComponent<Text>().text = "GAME OVER";
    }


    private void SetAlpha(float value)
    {
        coinUI.GetComponent<CanvasGroup>().alpha = value;
        treeScroll.GetComponent<CanvasGroup>().alpha = value;
        shovelBackground.GetComponent<CanvasGroup>().alpha = value;
        waveTextCanvasGr.GetComponent<CanvasGroup>().alpha = value;
        treeDescription.GetComponent<CanvasGroup>().alpha = value;
    }


    //public void LoadLevelBtn(string levelToLoad)
    //{
    //    StartCoroutine(LoadlevelASync(levelToLoad));
    //}

    //IEnumerator LoadlevelASync(string levelToLoad)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

    //    //loadingScreen.SetActive(true);
    //    //MainMenu.SetActive(false);
    //    //while(!operation.isDone)
    //    //{
    //    //    loadingSlider.value = operation.progress;
    //    //    yield return null;
    //    //}
    //}
}
