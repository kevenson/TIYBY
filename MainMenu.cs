using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int startDelay = 5;
    public GameObject startButton, quitButton,countDown;
    //private TextMesh countDownText;
    public TMPro.TextMeshProUGUI countDownText;
    public TMPro.TextMeshProUGUI subtitleText;
    public static MainMenu instance;   // for change scene coroutine
    public ScreenFade screenFade;

    private void Awake()
    {
        // set our static reference to our newly initialized instance
        instance = this;
        if (subtitleText != null)
        {
            subtitleText.text = "a big journey in a small body, vol. 1";
        }
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        startButton.SetActive(false);
        quitButton.SetActive(false);
        countDown.SetActive(true);
        subtitleText.text = "please put your VR headset on now";
        //countDownText = countDown.GetComponent<TextMesh>();
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        for (int i = 0; i < startDelay; i++)
        {
            var num = startDelay - i;
            countDownText.text = num.ToString();
            yield return new WaitForSeconds(1);
        }
        print("Loading scene: " + SceneManager.GetActiveScene().buildIndex + 1);
        LoadNextScene();
    }

    public static void LoadNextScene()
    {
        //this will launch the coroutine on our instance
        instance.StartCoroutine("LoadNewScene");
    }

    // The coroutine runs on its own at the same time as Update()
    IEnumerator LoadNewScene()
    {
        //screenFade.Fade();
        // slight delay
        //yield return new WaitForSeconds(3);
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        // NOTE: Need to change this so that the screen fades while loading; see: https://forum.unity.com/threads/vr-headset-locks-and-loading-bar-freezes-on-loadsceneasync.530610/
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

}
