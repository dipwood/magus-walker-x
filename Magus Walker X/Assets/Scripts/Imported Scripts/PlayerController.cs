using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D theRB;
    public float moveSpeed;

    public Animator playerAnimator;
    public static PlayerController instance;

    public string areaTransitionName;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true;
    public bool isFirstAnimation;
    public bool servitorEvent;

    // Use this for initialization
    void Start () {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        } else
        {
            theRB.velocity = Vector2.zero;
        }


        playerAnimator.SetFloat("moveX", theRB.velocity.x);
        playerAnimator.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                playerAnimator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                playerAnimator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if (servitorEvent)
        {
            transform.position = new Vector3(transform.position.x - 0.04f, transform.position.y, transform.position.z);
            if (transform.position.x < -4.8f)
            {
                servitorEvent = false;
                playerAnimator.Play("Player_Idle");
            }
        }
    }

    //private string[] GetLines()
    //{
    //    string[] lines = new string[8];

    //    lines[0] = "NULL-";
    //    lines[1] = "The next thing he knew, his eyes opened. He was lying down on a slab of stone.";
    //    lines[2] = "While the room was dark, he could make out shelves of various tools and liquids.";
    //    lines[3] = "He felt groggy, the kind of sensation associated with a long night of binge drinking, but without the hangover. There was also a strange coldness in his chest.";
    //    lines[4] = "n-Magus";
    //    lines[5] = "... Where is the exit?";
    //    lines[6] = "NULL-";
    //    lines[7] = "The room appeared to have no clear esape route. Subduing feelings of panic for the moment, he looked for a way out.";


    //    return lines;
    //}

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }
}
