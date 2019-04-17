using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject panelSetting;
    public GameObject panelCommand;
    public GameObject yestext;
    public GameObject yestextLINE;
    public GameObject notext;
    public GameObject notextLINE;
    public Button btnSetting;
    public Image banner; // car les images pour le round Passe devant le menue génant la visibilitée
    public GameObject panelareYouSure;
    public GameObject btnjumpG;
    public GameObject btnUltraG;
    public GameObject btnCombotG;
    public GameObject btnjumpD;
    public GameObject btnUltraD;
    public GameObject btnCombotD;
    private bool show;
    public Fighter player;


    // sliders

    public GameObject musicSlider;
    private float sliderMusicValue;

    private void Start()
    {
        show = true;
        sliderMusicValue = PlayerPrefs.GetFloat("MusicVolume");
        btnSetting.Select();
        panelSetting.SetActive(true);
        panelCommand.SetActive(false);
        panelareYouSure.SetActive(false);
        if (PlayerPrefs.GetInt("Yes") == 1)
        {
            yestextLINE.gameObject.SetActive(true);
            notextLINE.gameObject.SetActive(false);
        }
        else
        {
            notextLINE.gameObject.SetActive(true);
            yestextLINE.gameObject.SetActive(false);
        }
        // choix du niveau sonor
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume")*4;
    }
    void Update()
    {
        sliderMusicValue = musicSlider.GetComponent<Slider>().value;

        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (GameIsPaused)
            {
                Resume(); 
            }
            else
            {
                Pause();
            }
        }
        if (player.PlayerPosition() == Position.Gauche)
        {
            show = true;
        }
        else
        {
            show = false;
        }
        

    }
    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", sliderMusicValue/4);
    }
    public void Resume()
    {
        //Debug.Log("resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        banner.color = Color.white ; //rendre les images rounds  leur couleurs d'origines
        btnSetting.Select();
        Cursor.visible = false;
    }
    //Permet la mise en pause lors d'une partie
    public void Pause()
    {
        btnUltraG.SetActive(show);
        btnjumpG.SetActive(show);
        btnCombotG.SetActive(show);
        btnUltraD.SetActive(!show);
        btnjumpD.SetActive(!show);
        btnCombotD.SetActive(!show);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        banner.color=Color.clear ; // rendre les images rounds transparents
        btnSetting.Select();
        panelareYouSure.SetActive(false);
        panelCommand.gameObject.SetActive(false);
        panelSetting.SetActive(true);
        Cursor.visible = true;
    }


    //Differents panels présents dans le menu pause sont afficher ou non
    //ansi que l'UI associé
    public void Exit()
    {
        panelareYouSure.SetActive(true);
        panelSetting.SetActive(false);
        panelCommand.SetActive(false);
    }
    public void NoExit()
    {
        panelareYouSure.gameObject.SetActive(false);
        panelSetting.SetActive(true);
    }
    //Quitter la partie, retour un timeScale de 1
    public void YesExit()
    {
        //Debug.Log("Charger Menu");
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    //panel permettant d'accéder aux paramétres
    public void Setting()
    {
        panelareYouSure.SetActive(false);
        panelCommand.gameObject.SetActive(false);
        panelSetting.SetActive(true);
    }
    //panel permettant d'accéder aux commandes
    public void Command()
    {
        panelareYouSure.SetActive(false);
        panelSetting.SetActive(false);
        panelCommand.gameObject.SetActive(true);
    }
    public void YesEffect()
    {
        yestext.GetComponent<Text>().text = "Yes";
        notext.GetComponent<Text>().text = "No";
        notextLINE.gameObject.SetActive(false);
        yestextLINE.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Yes", 1);
        PlayerPrefs.SetInt("No", 0);
    }
    public void NoEffect()
    {
        yestext.GetComponent<Text>().text = "Yes";
        notext.GetComponent<Text>().text = "No";
        notextLINE.gameObject.SetActive(true);
        yestextLINE.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Yes", 0);
        PlayerPrefs.SetInt("No", 1);
    }
}
