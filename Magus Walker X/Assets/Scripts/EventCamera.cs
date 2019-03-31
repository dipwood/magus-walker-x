using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventCamera : MonoBehaviour
{
    public int musicToPlay;
    private bool musicStarted;
    public string[] lines;
    public float timeUntilDialogue;
    public bool isPerson = false;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToRed;

    public GameObject openingScene;

    // Start is called before the first frame update
    void Start()
    {
        shouldFadeToRed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToRed)
        {
            fadeScreen.color = new Color(Mathf.MoveTowards(fadeScreen.color.r, 1f, fadeSpeed * Time.deltaTime), 
                fadeScreen.color.g, fadeScreen.color.b, fadeScreen.color.a);

            if (fadeScreen.color.r == 1f)
            {
                shouldFadeToRed = false;
            }
        }

        if (timeUntilDialogue > 0)
        {
            timeUntilDialogue -= Time.deltaTime;
            if (timeUntilDialogue <= 0)
            {
                DialogManager.instance.ShowDialog(GetLines(), isPerson, true);
            }
        }

        if (EventManager.instance.CheckIfEventRunning())
        {
            EventManager.instance.EndEvent();
            SceneManager.LoadScene("Cell");
            openingScene.SetActive(false);
        }
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
       
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }

    string[] GetLines()
    {
        string[] lines = new string[4];

        lines[0] = "NULL-";
        lines[1] = "Two giant, glowing red orbs cast down a blinding light down upon him, their sharpness turning the rest of his surroundings into black mush.";
        lines[2] = "He stood in front of the crimson radiance, terror becoming a tangible, living force that crept over him like a hungry beast, immobilizing his brain, holding him captive.";
        lines[3] = "He could hear his own heartbeat, beating louder and louder in rhythm with the growing brightness. When his heart felt like it was going to burst, everything stopped.";

        return lines;
    }
}
