using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionBorder : MonoBehaviour
{
    public BoxCollider boxCollider;
    public Fighter fighter1;
    public Fighter fighter2;
    private Animator animator;
    public enum LionSide
    {
        Right,Left
    }
    public LionSide side;
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        if ((Mathf.Abs(transform.position.z-fighter1.transform.position.z)<=10 ) || (Mathf.Abs(transform.position.z - fighter2.transform.position.z) <= 10))
        {
            animator.SetTrigger("Punch");
        }
        if (side ==LionSide.Left)
        {
            boxCollider.transform.SetPositionAndRotation(new Vector3(0, 0, boxCollider.transform.position.z), Quaternion.Euler(0,180, 0));

        }
        else
        {
            boxCollider.transform.SetPositionAndRotation(new Vector3(0, 0, boxCollider.transform.position.z), Quaternion.Euler(0, 0, 0));

        }


    }
}
