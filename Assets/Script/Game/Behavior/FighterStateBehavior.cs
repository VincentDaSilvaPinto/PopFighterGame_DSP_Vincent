using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateBehavior : StateMachineBehaviour
{
    public float horizontalForce;
    public float verticalForce;
    public FighterStates behaviorState;
    public AudioClip soundEffect; 
    protected Fighter fighter;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
       
        if (fighter == null)
        {
            fighter = animator.gameObject.GetComponent<Fighter>();
        }

        fighter.currentState = behaviorState;

        if(soundEffect!=null)
        {
            fighter.PlaySound(soundEffect);
        }
        fighter.Body.AddForce(new Vector3(0, verticalForce, 0));
    } 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(fighter.fighterName + " " + fighter.Orientation(fighter.currentRotation));
        fighter.Body.AddForce( new Vector3(0, 0,fighter.Orientation(fighter.PlayerPosition())* horizontalForce));
    }
}
