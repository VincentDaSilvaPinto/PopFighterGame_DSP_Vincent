using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Power : MonoBehaviour
{
    public Fighter owner;
    public float damage;
    private Vector3 targetPosition;
    public float smoothFactor = 8;
    private float creationTime;
    public ParticleSystem ImpactParticle;
    public int numberOfParticleEmission;
    private float positionY;
    void Start()
    {
       
        creationTime = Time.time;
        if (owner.fighterName == "DaffyDuck" )
        {
            if (owner.Orientation(owner.PlayerPosition()) == -1)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
           
            positionY = 3;
        }
        if (owner.fighterName=="Luffy")
        {
            transform.rotation = Quaternion.Euler(0, owner.Orientation(owner.PlayerPosition())*90, 0);
            positionY = 2.8f;
        }
        if(owner.fighterName == "GokuSJ3" || owner.fighterName == "Picolo"|| owner.fighterName == "Wolfy")
        {
            if (owner.Orientation(owner.PlayerPosition()) == 1)
            {
                transform.rotation = Quaternion.Euler(0, -180 * owner.Orientation(owner.PlayerPosition()), 0);
            }
            positionY = 0;
        }
        if (owner.fighterName == "StanLee")
        { 
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            positionY = 0;
        }
        targetPosition = new Vector3(0, positionY, owner.opponent.transform.position.z);
        
    }

    void Update()
    {
        if (Time.time - creationTime > 1.5f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
        }
        if (Time.time - creationTime > 4f)
        {
            Destroy(gameObject);
        }
        if (transform.position.y <= 1f && ( owner.fighterName== "GokuSJ3" || owner.fighterName == "StanLee"))
        {
            if (ImpactParticle!=null)
            {
                ImpactParticle.Emit(numberOfParticleEmission);
            }
        }
        if (transform.position.y <= 3.3f && owner.fighterName != "GokuSJ3" &&  owner.fighterName != "StanLee")
        {
            if (ImpactParticle != null)
            {
                ImpactParticle.Emit(numberOfParticleEmission);
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Fighter fighter = collision.gameObject.GetComponent<Fighter>();
        if (fighter !=null && fighter!= owner)
        {
            owner.usePower = false;
            fighter.Damage(damage);
            fighter.Animator.SetTrigger("PowerPerso");

            if (owner.fighterName == "Luffy"  )
            {
                //Debug.Log("destroy");
                Destroy(gameObject);
            }
        }
    }
}

