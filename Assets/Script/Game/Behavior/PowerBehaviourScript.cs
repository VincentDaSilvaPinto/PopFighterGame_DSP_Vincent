using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBehaviourScript : FighterStateBehavior
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string folderPower="";
        base.OnStateEnter(animator, stateInfo, layerIndex);

        float fighterZ = fighter.transform.position.z;
        float fighterY = fighter.transform.position.y ;
        float fighterX = fighter.transform.position.x;

        if (fighter.fighterName == "GokuSJ3" )
        {
            folderPower="Genkinama/Genkinama";
            fighterY += 23;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition());
        }
        if (fighter.fighterName == "StanLee")
        {
            folderPower = "StanLeePower/StanLeePower";
            fighterY += 23;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition());
        }
        if (fighter.fighterName == "Picolo")
        {
            folderPower = "PicoloPower/catOfAndrea";
            fighterY += 3.3f;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition()) * 7.6f;
            fighterX = -1.69f;
        }
        if (fighter.fighterName == "DaffyDuck" )
        {
            folderPower = "Unicorn/UNICORN";
            fighterY += 15;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition()) * 2;
        }
        if (fighter.fighterName == "Wolfy")
        {
            folderPower = "Wolfy/WolfyPower";
            fighterY += 3.3f;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition()) * 7.6f;
            fighterX = -1.69f;
        }
        if (fighter.fighterName == "Luffy" )
        {
            folderPower = "LuffyArm/LuffyArm";
            fighterY += 3.3f;
            fighterZ += -fighter.Orientation(fighter.PlayerPosition()) * 7.6f;
            fighterX = -1.69f;
        }

        GameObject instance = Object.Instantiate(
            Resources.Load(folderPower),
            new Vector3(fighterX, fighterY, fighterZ),
            Quaternion.Euler(0, 0, 0)
                ) as GameObject;

        Power entitie= instance.GetComponent<Power>();
        entitie.owner = fighter;
    }
}
