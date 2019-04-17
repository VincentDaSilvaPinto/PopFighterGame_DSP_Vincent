using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    //public string nameAttack;
    public float damage;
    public Fighter owner;
    public float difficulty;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Normal") == 1)
        {
            if (PlayerPrefs.GetInt("Survivor") == 1)
            {
                difficulty = 1f;
            }
            else
            {
                difficulty = 2f;
            }
            
        }else
        {
            if (PlayerPrefs.GetInt("Survivor") == 1)
            {
                difficulty =2f;
            }
            else
            {
                difficulty = 2.5f;
            }
        }
    }
    //Permet de savoir quand le collder est en contact avec l'adversaire
    //Et ainsi attribuer des dégats si le mouvement correspond à une attaque
    public void OnTriggerEnter(Collider Other)
    {
        Fighter opponent = Other.gameObject.GetComponent<Fighter>();
        if (owner.Attack)
        { 
            if (opponent !=null && opponent!=owner)
                {
                //Debug.Log("Je touche " + opponent + "avec " + nameAttack);

                if (owner.playerType == PlayerType.IA)
                {
                    opponent.Damage(damage * difficulty);
                }
                else
                {
                    if (difficulty == 2)
                    { opponent.Damage(damage *difficulty); }
                    else
                    {
                        if (owner.HealthPercent >= 0.5)
                        {
                            opponent.Damage(damage * 2 * owner.HealthPercent);
                        }
                        
                        else
                        {
                            opponent.Damage(damage);
                        }
                    }
                }

                    
            }
        }

        if (owner.AttackCombo)
        {
            if (opponent != null && opponent != owner)
            {
                //Debug.Log("Je touche " + opponent + "avec " + NameAttack);
                owner.transform.SetPositionAndRotation(new Vector3(0,owner.transform.position.y, opponent.transform.position.z - opponent.Orientation(owner.PlayerPosition()) * 2), opponent.transform.rotation);
                if (owner.playerType == PlayerType.IA)
                {
                    opponent.Damage(damage * difficulty/3);
                }
                else {
                    if (owner.HealthPercent >= 0.5)
                    {
                        opponent.Damage(damage * 2 * owner.HealthPercent);
                    }

                    else
                    {
                        opponent.Damage(damage * 2 * owner.HealthPercent);
                    }
                }
            }
        }
    }
}
