using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarEssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioManager).GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
