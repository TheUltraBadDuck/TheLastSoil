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
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private CameraMovement cameraMovement;


    private float fadeCD = 0f;
    private float fadeTimer = 1.5f;
    private float startGameTimer = 15f;

    private bool startGame = false;
    private bool finishmainMenuFadeOut = false;
    private bool finishUIGameFadeOut = false;
    private bool finishStartGame = false;
    private bool gameOver = false;

    private bool isPause = false;



    private void Start()
    {
        cameraMovement.isFreazeCamera = true;

        coinUI.SetActive(false);
        treeScroll.SetActive(false);
        shovelBackground.SetActive(false);
        waveTextCanvasGr.SetActive(false);
        treeDescription.SetActive(false);
        pauseButton.SetActive(false);

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
                    waveTextCanvasGr.GetComponent<CanvasGroup>().alpha = 0;
                    treeDescription.SetActive(true);
                    pauseButton.SetActive(true);

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
                }
            }
            else if (!finishStartGame)
            {
                fadeCD += Time.deltaTime;
                if (fadeCD > startGameTimer)
                {
                    finishStartGame = true;
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
        cameraMovement.isFreazeCamera = false;
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
        StopObjectMovings(true);
    }


    public void HandlePauseGame()
    {
        isPause = !isPause;

        if (isPause)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }


    public void StopObjectMovings(bool value)
    {
        isPause = value;
        Time.timeScale = isPause ? 0f : 1f;
    }



    private void SetAlpha(float value)
    {
        coinUI.GetComponent<CanvasGroup>().alpha = value;
        treeScroll.GetComponent<CanvasGroup>().alpha = value;
        shovelBackground.GetComponent<CanvasGroup>().alpha = value;
        treeDescription.GetComponent<CanvasGroup>().alpha = value;
        pauseButton.GetComponent<CanvasGroup>().alpha = value;
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
