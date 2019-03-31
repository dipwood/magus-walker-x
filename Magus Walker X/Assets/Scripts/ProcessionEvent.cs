using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessionEvent : MonoBehaviour
{
    private bool canActivate;
    public bool eventCompleted;
    public ProphetController[] prophets;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        eventCompleted = false;
        PlayerController.instance.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && !eventCompleted)
        {
            for (int i = 0; i < prophets.Length; i++)
            {
                prophets[i].instance.walkRight = true;
                eventCompleted = true;
                StartCoroutine(ProcessionEventCo());
            }
        }

        if (EventManager.instance.CheckIfEventRunning())
        {
            AudioManager.instance.PlaySFX(4);
            EventManager.instance.EndEvent();

            playerTransform = FindObjectOfType<PlayerController>().transform;
            CameraController.instance.MoveToTarget(playerTransform);

            PlayerController.instance.canMove = true;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !eventCompleted)
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

    public IEnumerator ProcessionEventCo()
    {
        yield return new WaitForSeconds(5f);
        DialogManager.instance.ShowDialog(GetLines(), true, true);
    }

    string[] GetLines()
    {
        string[] lines = new string[4];

        lines[0] = "n-Robed Man";
        lines[1] = "On this day, the third solstice of Arkh'Girah, set in the Uli of the fifth, let it be known that Kirrak The Jubilant has become a Lost One.";
        lines[2] = "We are grief-stricken to lose another one of our kind, yet proud to offer him back into The Void.";
        lines[3] = "May The Writhing God bless our sacrifice of his body, and may his spirit return to the plane.";

        return lines;
    }
}
