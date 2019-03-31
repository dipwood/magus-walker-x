using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphetController : MonoBehaviour
{
    public Animator prophetAnimator;
    public bool walkRight;
    public bool stopWalking;

    public ProphetController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (walkRight)
        {
            prophetAnimator.Play("Prophet_Walk_Right");
            walkRight = false;
            CameraController.instance.target = transform;
        }

        StartCoroutine(ProcessionMoveCo());

        if (!stopWalking)
        {
            transform.position = new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z);
        }
    }

    public IEnumerator ProcessionMoveCo()
    {
        yield return new WaitForSeconds(5f);
        stopWalking = true;
        prophetAnimator.Play("Prophet_Idle_Right");
    }

    public void DestroyProphet()
    {
        Destroy(gameObject);
    }
}
