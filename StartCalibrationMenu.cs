using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartCalibrationMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI confirmation;
    private Text loadingText;
    [Header("Confirmation Field Text")]
    public string calibrationError;
    public string confirmationText = "Recalibration complete. Press A button to enter Antworld in your new body";
    public static bool calibrationComplete = false;
    public bool isLoading;
    private bool headsetConnected = false;
    private bool animalScaling = false;
    [Header("Gameobject references")]
    public MainMenu mainMenu;
    public PlayerScaling_LoadScreen scaleScript;
    //public GameObject ant_right, ant_left, bird;    // add for scaling
    //public GameObject[] antPlatforms;
    public GameObject normalPlatforms;
    public GameObject shrunkPlatforms;
    public GameObject player;
    [Header("Audio & effects")]
    public bool loopAmbient = true;
    [Range(0.0f,1.0f)]
    public float ambientSoundVolume = .5f;
    public AudioSource calibrationEffectsSource, ambientSource, smallSparrowSounds, bigSparrowSounds, antRight, antLeft;
    public AudioClip birdSmall, birdBig, antSmall, antBig, antMovement;
    public AudioClip enterTheMachineBackground;
    public AudioClip machineShrinkSound;
    public ParticleSystem machineShrinkParticleSys;
    private ParticleSystem[] machineShrinkChildren;

    //public OVRScreenFade OVRFade;
    //public ScreenFade screenFade;

    void Start()
    {
        // add help text
        confirmationText = "Before you can join Antworld, we must recalibrate your VR rig. \n\n With your headset on, please stand up straight, and press the A button on your Touch controller.";
        confirmation.text = confirmationText;
        calibrationError = "No headset detected.\n\nPress A button to try again.\n\n If this error persists, please make sure your VR device is connected and Oculus software is running properly. ";

        // make sure shrunk platforms are off
        shrunkPlatforms.SetActive(false);
        normalPlatforms.SetActive(true);
        // Set up audio & effects
        ambientSource.clip = enterTheMachineBackground;
        ambientSource.loop = loopAmbient;
        ambientSource.volume = ambientSoundVolume;
        ambientSource.Play();
        calibrationEffectsSource.clip = machineShrinkSound;
        machineShrinkParticleSys.Stop();
        machineShrinkChildren = machineShrinkParticleSys.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem childPS in machineShrinkChildren)
        {
            ParticleSystem.EmissionModule childPSEM = childPS.emission;
            childPSEM.enabled = false;
        }
        //set bools
        isLoading = false;
        animalScaling = false;
        // detect VR headset
        StartCoroutine("DetectVRHeadset");
    }

    void Update()
    {
        // let user quit game w/ "q" press
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MainMenu.QuitGame();
        }

        if (isLoading == true)
        {
            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
        // if clibration is complete, show help text
        if (calibrationComplete == true)
        {
            confirmationText = "Recalibration complete.\n\nPress A button to enter your new ant body (this may take a few moments to load).\n\nAs a worker ant, your job is to return food to the nest without dying.\n\nGood luck!";
            confirmation.text = confirmationText;
            // scale up comparison models if not yet complete
            if (animalScaling == false)
            {
                //re-scale player controller
                player.transform.localScale = new Vector3(.25f, .25f, .25f);
                //"re-scale" ant & bird objects by switching gameobject activation
                shrunkPlatforms.SetActive(true);
                normalPlatforms.SetActive(false);
                animalScaling = true;
                // play new animal sounds
                StartCoroutine("GiantAntSounds");
                StartCoroutine("GiantBirdSounds");

            }

        }
        if ((OVRInput.GetDown(OVRInput.Button.One)) || (OVRInput.GetDown(OVRInput.Button.Three)))
        {
            StartCoroutine("DetectVRHeadset");
            if (calibrationComplete == true && headsetConnected == true)
            {
                //Debug.Log("Changing Scene now...");
                MainMenu.LoadNextScene();
            }
            else if (headsetConnected == false)
            {
                confirmation.text = calibrationError;
            }
            else if (headsetConnected == true && calibrationComplete == false)
            {
                StartCoroutine("RecalibrationEffects");
                StartCoroutine("CalibrateHeadset");
                //CalibrateHeadset();
            }
            //else
            //{
            //    confirmation.text = confirmationText;
            //    //StartCoroutine("RecalibrationEffects");
            //}
        }

    }
    IEnumerator GiantBirdSounds()
    {
        for (; ; )
        {
            bigSparrowSounds.PlayOneShot(birdBig);
            yield return new WaitForSeconds(7);
        }

    }
    IEnumerator GiantAntSounds()
    {
        antRight.PlayOneShot(antMovement);
        for (; ; )
        {
            antLeft.PlayOneShot(antSmall);
            yield return new WaitForSeconds(3);
            antRight.PlayOneShot(antBig);
            yield return new WaitForSeconds(5);
            antLeft.PlayOneShot(antMovement);
        }
        
    }

    // play Recalibration audio and particle effects, then change help text
    IEnumerator RecalibrationEffects()
    {

        //calibrationEffectsSource.clip = machineShrinkSound;
        calibrationEffectsSource.Play();
        machineShrinkParticleSys.Play();
        foreach (ParticleSystem childPS in machineShrinkChildren)
        {
            ParticleSystem.EmissionModule childPSEM = childPS.emission;
            childPSEM.enabled = true;
            childPS.Play();
        }
        yield return new WaitForSeconds(5);
        machineShrinkParticleSys.Stop();
        foreach (ParticleSystem childPS in machineShrinkChildren)
        {
            ParticleSystem.EmissionModule childPSEM = childPS.emission;
            childPSEM.enabled = false;
            childPS.Stop();
        }
        confirmation.text = confirmationText;
        //yield return null;
    }

    public IEnumerator DetectVRHeadset()
    {
        yield return new WaitForEndOfFrame();
        if(UnityEngine.XR.XRDevice.isPresent == true)
        {
            Debug.Log("Headset detected");
            headsetConnected = true;
        }
    }

    IEnumerator CalibrateHeadset()
    {
        yield return new WaitForSeconds(3.5f);
        scaleScript.StartCalibration();

    }

    public void ChangeScene(int sceneNum)
    {
        loadingText.text = "Loading...";
        isLoading = true;
        //screenFade.Fade();
        //OVRFade.FadeOut();
        //sceneToLoad = sceneNum;
        StartCoroutine(LoadNewScene());
    }

    // The coroutine runs on its own at the same time as Update()
    IEnumerator LoadNewScene()
    {
        // slight delay
        yield return new WaitForSeconds(3);

        var scene = SceneManager.GetActiveScene().buildIndex + 1;
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        // NOTE: Need to change this so that the screen fades while loading; see: https://forum.unity.com/threads/vr-headset-locks-and-loading-bar-freezes-on-loadsceneasync.530610/
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
