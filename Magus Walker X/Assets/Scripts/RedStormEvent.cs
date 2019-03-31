using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedStormEvent : MonoBehaviour
{
    public bool startEvent;
    private bool musicStarted;
    public int musicToPlay;

    // Start is called before the first frame update
    void Start()
    {
        startEvent = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startEvent)
        {
            DialogManager.instance.ShowDialog(GetLines(), true, true);
            startEvent = false;
        }

        if (EventManager.instance.CheckIfEventRunning())
        {
            EventManager.instance.EndEvent();
            DeleteAll();
            SceneManager.LoadScene("GridTest");
        }
    }

    public void DeleteAll()
    {
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
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

    private string[] GetLines()
    {
        string[] lines = new string[11];

        lines[0] = "NULL-";
        lines[1] = "He felt his body being raised up by his head, struggling the whole time as he was unceremoniously carried through the labyrinth blind. The next thing he knew, he was tossed outside, with massive stone doors closing behind him.";
        lines[2] = "When his eyes finally opened, he saw the ground, which was still marble but without any of its sheen. It was worn and dull.";
        lines[3] = "He looked around near the ground, pulling his body up as his eyes darted to and fro. Aside from the enormous circular building he was tossed out of, there was nothing in his immediate vicinity.";
        lines[4] = "He felt as though he was 'outside', but it was still dark... and misty. Only a faint red light gave him vision, which made it hard to believe he was really free.";
        lines[5] = "He couldn't see more than twenty feet away. The outline of a massive wall protruded from the circular building, but what caught his attention was when he finally looked up.";
        lines[6] = "The sky. He could see all of it uninhibited, and he did not understand what he was viewing. A giant, swirling mass of black clouds with a red glow.";
        lines[7] = "It was infinitely far away, and yet he felt as though he could touch it. At its center was a whirlpool of red tendrils extending outward among the black clouds like the shape of a snail's shell.";
        lines[8] = "It looked like the center of a great storm, but completely quiet. He could only gawk at the sky in wonder.";
        lines[9] = "THE END... ?";
        lines[10] = "Nope! Minigame time!";

        return lines;
    }
}
