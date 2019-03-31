using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServitorEvent : MonoBehaviour
{
    private bool eventStarted;
    private bool canActivate;
    private Dictionary<int, int> triggerDictionary;
    public UndeadController undead;
    public GameObject closedBook, openBook;

    public GameObject servitorTop;
    public GameObject servitorBottom;
    public Animator servitorTopAnimator;
    public Animator servitorBottomAnimator;
    private bool audioPlayOnce;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.canMove = false;

        triggerDictionary = new Dictionary<int, int>
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 },
            { 10, 0 },
            { 11, 0 },
            { 12, 0 },
            { 13, 0 },
            { 14, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && !eventStarted)
        {
            StartCoroutine(EventCo(1f, 0));

            PlayerController.instance.transform.position = new Vector3(0.05f, 0.18f, PlayerController.instance.transform.position.z);
            PlayerController.instance.playerAnimator.SetFloat("lastMoveX", -1);
            eventStarted = true;
        }

        foreach (KeyValuePair<int, int> entry in triggerDictionary)
        {
            if (entry.Value == 1)
            {
                NextStep(entry);
                break;
            }
        }
    }

    private void NextStep(KeyValuePair<int, int> entry)
    {
        switch (entry.Key)
        {
            case 0:
                FirstStep(entry);
                break;
            case 1:
                SecondStep(entry);
                break;
            case 2:
                ThirdStep(entry);
                break;
            case 3:
                FourthStep(entry);
                break;
            case 4:
                FifthStep(entry);
                break;
            case 5:
                SixthStep(entry);
                break;
            case 6:
                SeventhStep(entry);
                break;
            case 7:
                EigthStep(entry);
                break;
            case 8:
                NinthStep(entry);
                break;
            case 9:
                TenthStep(entry);
                break;
            case 10:
                EleventhStep(entry);
                break;
            case 11:
                TwelthStep(entry);
                break;
            case 12:
                ThirteenthStep(entry);
                break;
            case 13:
                FourteenthStep(entry);
                break;
            case 14:
                FifteenthStep(entry);
                break;
            default:
                Console.WriteLine("ERROR!!");
                break;
        }
    }

    private void FirstStep(KeyValuePair<int, int> entry)
    {
        undead.instance.Spook();
        StartCoroutine(EventCo(2f, 1));
        triggerDictionary[entry.Key] = 2;
    }

    private void SecondStep(KeyValuePair<int, int> entry)
    {
        PlayerController.instance.servitorEvent = true;
        PlayerController.instance.playerAnimator.Play("Player_Walk_Left");

        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(2f, 2));
    }

    private void ThirdStep(KeyValuePair<int, int> entry)
    {
        DialogManager.instance.ShowDialog(GetLines(), true, true);
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(3f, 3));
    }

    private void FourthStep(KeyValuePair<int, int> entry)
    {
        PlayerController.instance.playerAnimator.Play("Player_Point_Left");
        if (EventManager.instance.CheckIfEventRunning())
        {
            DialogManager.instance.ShowDialog(GetLines2(), true, true);
            EventManager.instance.EndEvent();
            triggerDictionary[entry.Key] = 2;
            StartCoroutine(EventCo(1f, 4));
        }
    }

    private void FifthStep(KeyValuePair<int, int> entry)
    {
        if (EventManager.instance.CheckIfEventRunning())
        {
            PlayerController.instance.playerAnimator.Play("Player_Idle");
            DialogManager.instance.ShowDialog(GetLines3(), true, true);
            EventManager.instance.EndEvent();
            triggerDictionary[entry.Key] = 2;
            StartCoroutine(EventCo(1f, 5));
        }
    }

    private void SixthStep(KeyValuePair<int, int> entry)
    {
        if (EventManager.instance.CheckIfEventRunning())
        {
            undead.instance.servitorEvent = true;
            undead.instance.undeadAnimator.Play("Prophet_Walk_Up");
            PlayerController.instance.playerAnimator.SetFloat("lastMoveY", 1);
            PlayerController.instance.playerAnimator.SetFloat("lastMoveX", 0);
            PlayerController.instance.playerAnimator.Play("Player_Idle");
            EventManager.instance.EndEvent();
            triggerDictionary[entry.Key] = 2;
            StartCoroutine(EventCo(2f, 6));
        }
    }

    private void SeventhStep(KeyValuePair<int, int> entry)
    {
        undead.instance.undeadAnimator.Play("Prophet_Idle_Down");
        closedBook.SetActive(true);
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(2f, 7));
    }

    private void EigthStep(KeyValuePair<int, int> entry)
    {
        closedBook.SetActive(false);
        openBook.SetActive(true);
        DialogManager.instance.ShowDialog(GetLines4(), true, true);
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(1f, 8));
    }

    private void NinthStep(KeyValuePair<int, int> entry)
    {
        servitorTop.SetActive(true);
        servitorBottom.SetActive(true);
        servitorBottomAnimator.Play("Servitor_Bottom_Walk");
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(2f, 9));
    }

    private void TenthStep(KeyValuePair<int, int> entry)
    {
        if (EventManager.instance.CheckIfEventRunning())
        {
            servitorTop.transform.position = new Vector3(servitorTop.transform.position.x, servitorTop.transform.position.y + 0.04f, servitorTop.transform.position.z);
            servitorBottom.transform.position = new Vector3(servitorBottom.transform.position.x, servitorBottom.transform.position.y + 0.04f, servitorBottom.transform.position.z);
            PlayerController.instance.playerAnimator.SetFloat("lastMoveY", 0);
            PlayerController.instance.playerAnimator.SetFloat("lastMoveX", 1);

            if (servitorTop.transform.position.y > 2.39 && servitorBottom.transform.position.y > 0.37)
            {
                EventManager.instance.EndEvent();
                servitorBottomAnimator.Play("Servitor_Bottom_Idle_Down");
                triggerDictionary[entry.Key] = 2;
                StartCoroutine(EventCo(1f, 10));
            }
        }
    }

    private void EleventhStep(KeyValuePair<int, int> entry)
    {
        servitorTopAnimator.Play("Servitor_Top_Scream");
        servitorBottomAnimator.Play("Servitor_Bottom_Scream");
        AudioManager.instance.PlaySFX(2);
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(1f, 11));
    }

    private void TwelthStep(KeyValuePair<int, int> entry)
    {
        servitorTopAnimator.Play("Servitor_Top_Idle");
        servitorBottomAnimator.Play("Servitor_Bottom_Idle_Down");
        DialogManager.instance.ShowDialog(GetLines5(), true, true);
        AudioManager.instance.PlayBGM(3);
        triggerDictionary[entry.Key] = 2;
        StartCoroutine(EventCo(2f, 12));
    }

    private void ThirteenthStep(KeyValuePair<int, int> entry)
    {
        if (EventManager.instance.CheckIfEventRunning())
        {
            PlayerController.instance.playerAnimator.Play("Player_Cast_Spell");
            AudioManager.instance.PlaySFX(3);
            EventManager.instance.EndEvent();
            audioPlayOnce = true;
            triggerDictionary[entry.Key] = 2;
            StartCoroutine(EventCo(2f, 13));
        }
    }

    private void FourteenthStep(KeyValuePair<int, int> entry)
    {
        if (!EventManager.instance.CheckIfEventRunning())
        {
            PlayerController.instance.playerAnimator.Play("Player_Idle");
            DialogManager.instance.ShowDialog(GetLines6(), true, true);
            if (audioPlayOnce)
            {
                AudioManager.instance.PlaySFX(2);
                audioPlayOnce = false;
                servitorTopAnimator.Play("Servitor_Top_Scream");
                servitorBottomAnimator.Play("Servitor_Bottom_Scream");
            }
        }

        if (EventManager.instance.CheckIfEventRunning())
        {
            servitorTop.transform.position = new Vector3(servitorTop.transform.position.x - 0.5f, servitorTop.transform.position.y, servitorTop.transform.position.z);
            servitorBottom.transform.position = new Vector3(servitorBottom.transform.position.x - 0.5f, servitorBottom.transform.position.y, servitorBottom.transform.position.z);
            UIFade.instance.shouldFadeToBlack = true;

            if (servitorBottom.transform.position.x < -4.82f)
            {
                EventManager.instance.EndEvent();
                DialogManager.instance.ShowDialog(GetLines7(), true, true);
                triggerDictionary[entry.Key] = 2;
                StartCoroutine(EventCo(1f, 14));
            }
        }
    }

    private void FifteenthStep(KeyValuePair<int, int> entry)
    {
        if (EventManager.instance.CheckIfEventRunning())
        {
            triggerDictionary[entry.Key] = 2;
            SceneManager.LoadScene("RedStorm");
        }
    }



    public IEnumerator EventCo(float timeToWait, int triggerNumber)
    {
        yield return new WaitForSeconds(timeToWait);
        triggerDictionary[triggerNumber] = 1;

    }

    string[] GetLines()
    {
        string[] lines = new string[7];

        lines[0] = "n-Magus";
        lines[1] = "Exit. Now.";
        lines[2] = "n-Robed Man";
        lines[3] = "W-what the?! Who are you?!";
        lines[4] = "n-Magus";
        lines[5] = "Wrong answer.";
        lines[6] = "Show me how to get out of here and I won't pop your head like a grape.";

        return lines;
    }

    private string[] GetLines2()
    {
        string[] lines = new string[4];

        lines[0] = "n-Robed Man";
        lines[1] = "A what?!";
        lines[2] = "Are you... Aren't you the Lost One that was brought in?";
        lines[3] = "Let go of me. I would gladly escort you out.";

        return lines;
    }

    private string[] GetLines3()
    {
        string[] lines = new string[4];

        lines[0] = "n-Magus";
        lines[1] = "Lost One?";
        lines[2] = "n-Robed Man";
        lines[3] = "You... I see. I understand what's happening now. You've recently awoken, yes? I will explain everything.";

        return lines;
    }

    private string[] GetLines4()
    {
        string[] lines = new string[12];

        lines[0] = "n-Robed Man";
        lines[1] = "We only take in Lost Ones here, as in those that have lost their immortal souls and leave behind the shells they once called bodies. If you're here, that means...";
        lines[2] = "NULL-";
        lines[3] = "His words trailed off, and he swallowed before having a stark realization about this man's arrival.";
        lines[4] = "n-Magus";
        lines[5] = "... So, I'm free to go?";
        lines[6] = "n-Robed Man";
        lines[7] = "Essentially, yes.";
        lines[8] = "n-Magus";
        lines[9] = "Why do I have a hard time believing that?";
        lines[10] = "n-Robed Man";
        lines[11] = "It doesn't matter. After the commotion you've caused, you won't have much of a choice since there's a Servitor waiting outside.";

        return lines;
    }

    private string[] GetLines5()
    {
        string[] lines = new string[2];

        lines[0] = "n-Magus";
        lines[1] = "Fine then.";

        return lines;
    }

    private string[] GetLines6()
    {
        string[] lines = new string[2];

        lines[0] = "n-Magus";
        lines[1] = "My magic isn't working? Wait-";

        return lines;
    }

    private string[] GetLines7()
    {
        string[] lines = new string[2];

        lines[0] = "n-Magus";
        lines[1] = "Gah!";

        return lines;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !eventStarted)
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
