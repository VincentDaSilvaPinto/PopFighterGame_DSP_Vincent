using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    private GameObject[] characterList ;
    private int index;
    public string typeOfPlayer;
    Animator animator;
    public Text nameText;
    void Start()
    {
        animator = GetComponent<Animator>();
        characterList = new GameObject[transform.childCount];
        
        for(int i=0; i<transform.childCount; i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;

        }
        foreach(GameObject cara in characterList)
        {
            cara.SetActive(false);
        }

        if (characterList[0])
        {
            characterList[0].SetActive(true);
            PlayerPrefs.SetString(typeOfPlayer, characterList[0].name);
            nameText.text = characterList[0].ToString();
        }
    }
    
    public void toggleLeft()
    {
        //desactiver le personnage présenté en ce moment
        characterList[index].SetActive(false);

        index -= 1;
        if(index<0)
        {
            index = characterList.Length-1;
        }

        //Activer le nouveau personnage
        characterList[index].SetActive(true);
        PlayerPrefs.SetString(typeOfPlayer, characterList[index].name);
        nameText.text = characterList[index].ToString();
    }

    public void toggleRight()
    {
        //desactiver le personnage présenté en ce moment
        characterList[index].SetActive(false);

        index += 1;
        if (index == characterList.Length)
        {
            index = 0;
        }

        //Activer le nouveau personnage
        characterList[index].SetActive(true);
        PlayerPrefs.SetString(typeOfPlayer, characterList[index].name);
        nameText.text = characterList[index].ToString();
    }
}
