using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCameraController : MonoBehaviour
{
    private bool musicStarted;
    public int musicToPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
