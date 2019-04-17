using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCinématique : MonoBehaviour
{
    public string sceneToLoadAfterTimeLine;

    // permet de charger la scene de jeu aprés que l'animation soit finie
    void OnEnable()
    {
        
        if (sceneToLoadAfterTimeLine != "")
        {
            SceneManager.LoadScene(sceneToLoadAfterTimeLine, LoadSceneMode.Single);
        }
    }

    // permet de charger la scene de jeu avant que l'animation ne soit terminée
    public void Skip()
    {

        if (sceneToLoadAfterTimeLine != "")
        {
            if (sceneToLoadAfterTimeLine != "")
            {
                SceneManager.LoadScene(sceneToLoadAfterTimeLine, LoadSceneMode.Single);
            }
                
        }
    }

}
