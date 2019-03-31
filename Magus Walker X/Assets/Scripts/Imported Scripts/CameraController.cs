using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

    public Transform target;

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;

    public int musicToPlay;
    private bool musicStarted;

    public static CameraController instance;
    public bool isMovingTowards;
    private Transform targetToMoveTo;

    // Use this for initialization
    void Start () {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        target = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        theMap.CompressBounds();
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
	}
	
	// LateUpdate is called once per frame after Update
	void LateUpdate () {

        if (!isMovingTowards)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

            //keep the camera inside the bounds
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        }

        else
        {
            if (targetToMoveTo)
            {
                float step = 10f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    new Vector3(targetToMoveTo.position.x, targetToMoveTo.position.y, transform.position.z), step);

                if (transform.position.x == targetToMoveTo.position.x)
                {
                    isMovingTowards = false;
                    target = targetToMoveTo;
                    targetToMoveTo = null;
                }
            }
        }

        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
	}

    public void MoveToTarget(Transform objectToMoveTo)
    {
        isMovingTowards = true;
        targetToMoveTo = objectToMoveTo;

        if (transform.position == target.position)
        {
            isMovingTowards = false;
            target = targetToMoveTo;
        }
    }
}
