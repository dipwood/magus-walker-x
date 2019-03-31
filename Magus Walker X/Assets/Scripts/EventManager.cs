using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    //public Tilemap tilesToActivate;
    //public Tilemap tilesToDeactivate;
    private bool eventRunning;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunEvent()
    {
        eventRunning = true;
        //AudioManager.instance.PlaySFX(0);

        //tilesToActivate.gameObject.SetActive(true);
        //tilesToDeactivate.gameObject.SetActive(false);
    }

    public bool CheckIfEventRunning()
    {
        return eventRunning;
    }

    public void EndEvent()
    {
        eventRunning = false;
    }
}
