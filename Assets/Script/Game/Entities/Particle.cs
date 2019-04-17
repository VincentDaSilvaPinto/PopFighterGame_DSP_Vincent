using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem particle;
    public Fighter owner;
    public FighterStates behaviorState;
    
    public int emitNumber;
   
    void Update()
    {
        if (owner.currentState == behaviorState)
        {
            particle.Emit(emitNumber);
        }
    }
}
