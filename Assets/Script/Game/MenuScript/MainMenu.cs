using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    
	Animator CameraObject;
	private string sceneName = "";
    public Text namePlayer;
    public Text score;
    public Text highScore;
    public Light lightMenu;
	public GameObject panelGame;
	public GameObject panelareYouSure;
    public GameObject btnReturnSurvive;
    public GameObject btnReturnVersus;
    public GameObject chooseText;
    public GameObject versusGame;
    public GameObject survivorGame;
    public Material[] skyBox;
    public GameObject ArtComponent;
    public GameObject survivorBtn;
	public GameObject versusBtn;
    public GameObject textDiff;
    public GameObject textNorm;
    
    void Start()
    {
        namePlayer.text = PlayerPrefs.GetString("NamePlayer");
        score.text= PlayerPrefs.GetInt("ScoreGame").ToString();
        versusGame.SetActive(false);
        survivorGame.SetActive(false);
        CameraObject = transform.GetComponent<Animator>();
        lightMenu.intensity = 3;
        chooseText.SetActive(false);
        btnReturnSurvive.SetActive(false);
        btnReturnVersus.SetActive(false);
        RenderSettings.skybox = skyBox[0];
        
    }

    public void  Play()
    {
		panelareYouSure.gameObject.SetActive(false);
		survivorBtn.gameObject.SetActive(true);
		versusBtn.gameObject.SetActive(true);

        score.color = Color.yellow;
        namePlayer.color = Color.blue;
        highScore.color = Color.yellow;
    }
    public void Survivor()
    {
        sceneName = "CinematiqueIntroSurvivor";
        RenderSettings.skybox = skyBox[1];
        PlayerPrefs.SetInt("Versus", 0);
        PlayerPrefs.SetInt("Survivor", 1);
        DisablePlay();
        survivorGame.SetActive(true);

        chooseText.SetActive(true);
        lightMenu.intensity = 1f;
        CameraObject.SetFloat("Animate", 2);
        DisablePlay();
        btnReturnSurvive.SetActive(true);
        ArtComponent.SetActive(false);

        if (PlayerPrefs.GetInt("Normal") == 1)
        {
            textDiff.SetActive(false);
            textNorm.SetActive(true);
        }
        else
        {
            textDiff.SetActive(true);
            textNorm.SetActive(false);
        }
    }

    public void Versus()
    {
        sceneName = "GameSceneVersus";
        PlayerPrefs.SetInt("Survivor", 0);
        PlayerPrefs.SetInt("Versus", 1);

        versusGame.SetActive(true);
        chooseText.SetActive(true);
        lightMenu.intensity = 1f;
        CameraObject.SetFloat("Animate", 2);
        DisablePlay();
        btnReturnVersus.SetActive(true);
        ArtComponent.SetActive(false);
    }

    
    public void NewGame(){

		if(sceneName != ""){
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
	}

	public void  DisablePlay (){
		survivorBtn.gameObject.SetActive(false);
		versusBtn.gameObject.SetActive(false);

        score.color = Color.yellow;
        namePlayer.color = Color.blue;
        highScore.color = Color.yellow;
    }
    public void Position1()
    {
        ArtComponent.SetActive(true);
        RenderSettings.skybox = skyBox[0];
        lightMenu.intensity = 3;
        btnReturnSurvive.SetActive(false);
        btnReturnVersus.SetActive(false);
        CameraObject.SetFloat("Animate", 0);
        versusGame.SetActive(false);
        survivorGame.SetActive(false);
    }

    public void  Position2 (){

        lightMenu.intensity = 3;
        DisablePlay();
        score.color = Color.yellow;
        namePlayer.color = Color.blue;
        highScore.color = Color.yellow;
        panelareYouSure.gameObject.SetActive(false);
        CameraObject.SetFloat("Animate",1);
	}
    
    public void Position4()
    {
        ArtComponent.SetActive(true);
        RenderSettings.skybox = skyBox[0];
        versusGame.SetActive(false);
        survivorGame.SetActive(false);
        chooseText.SetActive(false);
        lightMenu.intensity = 3;
        btnReturnSurvive.SetActive(false);
        btnReturnVersus.SetActive(false);
        CameraObject.SetFloat("Animate", 0);

        
    }

   
    // Are You Sure - Quit Panel Pop Up
    public void  AreYouSure (){
		panelareYouSure.gameObject.SetActive(true);
		DisablePlay();
        score.color = Color.clear;
        namePlayer.color = Color.clear;
        highScore.color = Color.clear;
    }

	public void  NoExit(){
		panelareYouSure.gameObject.SetActive(false);
        score.color = Color.yellow;
        namePlayer.color = Color.blue;
        highScore.color = Color.yellow;
    }

	public void  YesExit(){
		Application.Quit();
	}

  
    
}