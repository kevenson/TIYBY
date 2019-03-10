using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Camera playerCam;
    public TMPro.TextMeshProUGUI primaryUIText;
    public string startHelpText;
    [Range(0.0f, 1.0f)]
    public float typingDelay = 0.15f;
    public AudioClip[] typingSounds;
    private AudioSource UIAudio;
    [Range(0.0f, 1.0f)]
    public float UIAudioVolume = 0.65f;
    private bool isUIHidden = false;
    // Food Score UI
    public TMPro.TextMeshProUGUI scoreUIText;
    public string foodScoreText;

    // Start is called before the first frame update
    void Start()
    {
        // set up text and audio & start coroutine
        primaryUIText.text = null;
        UIAudio = gameObject.AddComponent<AudioSource>();
        UIAudio.loop = false;
        UIAudio.playOnAwake = false;
        UIAudio.volume = UIAudioVolume;
        StartCoroutine("WriteText", startHelpText);
        foodScoreText = "Score: 0/2";
        scoreUIText.text = foodScoreText;

    }

    // take string and write on main canvas char by char with slight delay between each
    IEnumerator WriteText(string text)
    {
        var newText = "";
        for (int i = 0; i < text.Length -1; i++)
        {
            newText += text[i];
            primaryUIText.text = newText;
            if(typingSounds != null)
            {
                // play random sound from soudns array
                UIAudio.clip = typingSounds[Random.Range(0, typingSounds.Length - 1)];
                UIAudio.Play();
            }
            yield return new WaitForSeconds(typingDelay);

        }
    }

    void Update()
    {
        // keep score updated
        scoreUIText.text = "Score: " + FoodCounter.foodScore + "/2";

        // stop coroutine & hide text if "A" or "X" button is pressed
        if ((OVRInput.GetDown(OVRInput.Button.One)) || (OVRInput.GetDown(OVRInput.Button.Three)))
        {
            if (!isUIHidden)
            {
                primaryUIText.gameObject.SetActive(false);
                scoreUIText.gameObject.SetActive(false);
                UIAudio.Stop();
                StopCoroutine("WriteText");
                isUIHidden = true;
            }
            else if (isUIHidden == true && FoodCounter.winState == false)
            {
                primaryUIText.text = startHelpText;
                primaryUIText.gameObject.SetActive(true);
                scoreUIText.gameObject.SetActive(true);
                isUIHidden = false;
            }
        }
        if (FoodCounter.winState == true)
        {
            primaryUIText.text = "Congratulations, you have completed the demo.\n\n Thanks for playing!";
            primaryUIText.gameObject.SetActive(true);
            scoreUIText.gameObject.SetActive(true);
            isUIHidden = false;
        }
    }

}
