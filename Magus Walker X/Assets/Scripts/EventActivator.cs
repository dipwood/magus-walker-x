using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventActivator : MonoBehaviour
{
    private bool canActivate;
    public bool eventHappened;
    public Sprite chestClosed, chestOpened;
    public SpriteRenderer spriteRenderer;

    public string[] lines;
    public bool isPerson = true;

    public float timeUntilDialogue;
    private bool countdownUntilDialogue;

    public Tilemap tilesToActivate;
    public Tilemap tilesToDeactivate;
    private bool chestOpenEvent;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.isFirstAnimation = true;
        chestOpenEvent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!eventHappened && canActivate && Input.GetButtonDown("Fire1") && gameObject.activeInHierarchy)
        {
            AudioManager.instance.PlaySFX(0);
            eventHappened = true;
            spriteRenderer.sprite = chestOpened;
            countdownUntilDialogue = true;
            PlayerController.instance.canMove = false;
        }

        if (countdownUntilDialogue && timeUntilDialogue > 0)
        {
            timeUntilDialogue -= Time.deltaTime;
            if (timeUntilDialogue <= 0)
            {
                DialogManager.instance.ShowDialog(lines, isPerson, true);
            }

            chestOpenEvent = true;
        }

        if (EventManager.instance.CheckIfEventRunning() && chestOpenEvent)
        {
            AudioManager.instance.PlaySFX(1);

            tilesToActivate.gameObject.SetActive(true);
            tilesToDeactivate.gameObject.SetActive(false);

            EventManager.instance.EndEvent();

            PlayerController.instance.playerAnimator.SetFloat("lastMoveX", 0);
            PlayerController.instance.playerAnimator.SetFloat("lastMoveY", -1);
            PlayerController.instance.playerAnimator.Play("Player_Idle");
            PlayerController.instance.canMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            canActivate = false;
        }
    }
}
