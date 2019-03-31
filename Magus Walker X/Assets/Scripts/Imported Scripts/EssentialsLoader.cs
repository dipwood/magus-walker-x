using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

    public GameObject UIScreen;
    public GameObject player;
    // public GameObject gameManager;
    public GameObject audioManager;
    // public GameObject battleManager;
    public GameObject eventManager;

	// Use this for initialization
	void Start () {
		if(UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if(PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        //if (GameManager.instance == null)
        //{
        //    GameManager.instance = Instantiate(gameManager).GetComponent<GameManager>();
        //}

        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioManager).GetComponent<AudioManager>();
        }

        //if(BattleManager.instance == null)
        //{
        //    BattleManager.instance = Instantiate(battleManager).GetComponent<BattleManager>();
        //}

        if (EventManager.instance == null)
        {
            EventManager.instance = Instantiate(eventManager).GetComponent<EventManager>();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
