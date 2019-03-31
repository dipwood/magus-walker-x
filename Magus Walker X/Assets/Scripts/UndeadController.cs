using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadController : MonoBehaviour
{
    public Animator undeadAnimator;

    public UndeadController instance;
    public GameObject questionMark;
    public bool servitorEvent;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        undeadAnimator.Play("Prophet_Idle_Left");
    }

    // Update is called once per frame
    void Update()
    {
        if (servitorEvent)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.04f, transform.position.z);
            if (transform.position.y > 3.31f)
            {
                servitorEvent = false;
                undeadAnimator.Play("Prophet_Idle_Up");
            }
        }
    }

    internal void Spook()
    {
        undeadAnimator.Play("Prophet_Idle_Right");
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        StartCoroutine(SpookCo());
    }

    public IEnumerator SpookCo()
    {
        yield return new WaitForSeconds(0.15f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        yield return new WaitForSeconds(1f);
        questionMark.SetActive(true);
        yield return new WaitForSeconds(1f);
        questionMark.SetActive(false);

    }
}
