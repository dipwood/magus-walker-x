using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeEvent : MonoBehaviour
{
    private bool isFirstAnimation;
    private bool firstDialogue;

    // Start is called before the first frame update
    void Start()
    {
        isFirstAnimation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstAnimation)
        {
            PlayerController.instance.playerAnimator.Play("Getting_Up");
            PlayerController.instance.canMove = false;
            isFirstAnimation = false;
        }

        if (PlayerController.instance.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Getting_Up") 
            && PlayerController.instance.playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !PlayerController.instance.playerAnimator.IsInTransition(0))
        {
            PlayerController.instance.playerAnimator.SetFloat("lastMoveY", -1);
            PlayerController.instance.playerAnimator.Play("Player_Idle");
            DialogManager.instance.ShowDialog(GetLines(), true, true);
            firstDialogue = true;
        }

        if (EventManager.instance.CheckIfEventRunning() && firstDialogue)
        {
            PlayerController.instance.canMove = true;
            firstDialogue = false;
            EventManager.instance.EndEvent();

        }
    }

    private string[] GetLines()
    {
        string[] lines = new string[8];

        lines[0] = "NULL-";
        lines[1] = "The next thing he knew, his eyes opened. He was lying down on a slab of stone.";
        lines[2] = "While the room was dark, he could make out shelves of various tools and liquids.";
        lines[3] = "He felt groggy, the kind of sensation associated with a long night of binge drinking, but without the hangover. There was also a strange coldness in his chest.";
        lines[4] = "n-Magus";
        lines[5] = "... Where is the exit?";
        lines[6] = "NULL-";
        lines[7] = "The room appeared to have no clear esape route. Subduing feelings of panic for the moment, he looked for a way out.";

        return lines;
    }
}
