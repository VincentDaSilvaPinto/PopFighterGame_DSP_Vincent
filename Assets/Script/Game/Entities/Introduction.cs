using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    public Text clicktText;
    public GameObject button;
    public GameObject particuleEffect;
    public GameObject image;
    private bool showText;

    void Start()
    {
        clicktText.gameObject.SetActive(false);
        showText = false;
        button.SetActive(false);
        image.SetActive(false);
        particuleEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showText)
        {
            button.SetActive(true);
            clicktText.gameObject.SetActive(true);
            image.SetActive(true);
            particuleEffect.SetActive(true);
        }
       /* if (showImage)
        {
           
        }*/
    }

    public void AnimationEnded()
    {
        showText = true;
    }

   /* public void AnimationImage()
    {
        showImage = true;
    }*/
    public void LoadNewScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

}
