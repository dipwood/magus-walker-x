using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarUnitController : MonoBehaviour
{
    public bool selected;
    public Vector3Int unitCellPosition;
    public Animator animator;

    public bool unitMoved;
    public bool isPlayerUnit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayIdleAnimation()
    {
        animator.Play("Idle");
    }

    public void PlayMovementAnimation()
    {
        animator.Play("Move");
    }
}
