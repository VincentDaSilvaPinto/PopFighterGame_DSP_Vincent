using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{

    private float lastTimeUpdate = 0;
    private Fighter humanPlayer;
    private Fighter[] opponentsPlayer = new Fighter[4];
    private int difficulty;
    private Fighter battleLeftPlayer;
    private Fighter battleRightPlayer;
    private bool roundEnd;
    private int round = 0;
    private int roundTime = 200;
    private bool battleStarted;
   

    //Différent élément de l'interface utilisateur ayant besoin de savoir qui sont les joueurs
    public int RoundTime { get { return roundTime; } }
    public BannerController banner;
    public FooterController Footer;
    public HUDController HUD;
    public CameraMovement cameraMov;
    public LionBorder Lion1;
    public LionBorder Lion2;
    public PauseMenu PauseMenu;
    
    //Permet d'afficher le Score de Fin de Partie
    public GameObject finalPanelScore;
    public Text finalScore;
    public Text highScore;
    public GameObject boss;

    public Fighter[] players;
    public Fighter[] IA;
    private bool show = false;
   
    void Start()
    {
        Cursor.visible = false;
        battleStarted = false;
        roundEnd = false;
        round = 0;
        
        HUD.scoreText.text = "0";

        if (PlayerPrefs.GetInt("Normal") == 1)
        {
            difficulty = 2;
        }
        else
        {
            difficulty = 4;
        }

        // Permet de récuperer les noms des personnages choisis 
        string choice = PlayerPrefs.GetString("Joueur");
        string opponentchoice = PlayerPrefs.GetString("IA");

        //Permet de retrouver le joueurs choisis dans la liste
        if (PlayerPrefs.GetInt("Survivor") == 1)
        {
            opponentsPlayer = IA;
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (choice == players[i].fighterName)
            {
                humanPlayer = players[i];
            }
            if (PlayerPrefs.GetInt("Versus") == 1)
            {
                if (opponentchoice == IA[i].fighterName)
                {
                    opponentsPlayer[0] = IA[i];
                }
            }
        }
        //instancie les joueurs sur le terrain
        humanPlayer = Instantiate(humanPlayer, humanPlayer.transform.position = new Vector3(0, 0, 70), Quaternion.identity);
        opponentsPlayer[0] = Instantiate(opponentsPlayer[0], opponentsPlayer[0].transform.position = new Vector3(0, 0, 41), Quaternion.identity);

        //on instancie les lions (élement du décor) pour le joueur humain qu'une fois en début de partie 
        //Pour l'IA cela est fais a chaque round dans la fonction Instantiate()
        Lion2.fighter1 = humanPlayer;
        Lion1.fighter1 = humanPlayer;

        //On attribut les joueurs au différent Composant de l'interface de jeu (UI)
        InstanciateGameUtils(humanPlayer, opponentsPlayer, round);
        PauseMenu.player = humanPlayer;

        //Lance le Round
        banner.ShowRound();
    }

    void Update()
    {
        //Permet d'afficher le score en permanence sur l'écran de jeu
        HUD.scoreText.text = "Score : " + HUD.score.ToString();

        //Permet de déclencher les evenements lancant le début de la partie 
        if (!battleStarted && !banner.IsAnimating)
        {
            battleStarted = true;
            battleLeftPlayer.enable = true;
            battleRightPlayer.enable = true;
        }
        if (roundTime > 0 && Time.time - lastTimeUpdate > 1 && battleStarted)
        {
            roundTime -= 1;
            lastTimeUpdate = Time.time;
        }

        //Fin d'un round (mort d'un des joueurs)
        if (battleRightPlayer.HealthPercent <= 0 && !roundEnd)
        {
            banner.showYouWin();
            roundEnd = true;
            HUD.score += Mathf.CeilToInt(battleLeftPlayer.HealthPercent * 100)* difficulty;
            //Debug.Log(Mathf.CeilToInt(battleLeftPlayer.HealthPercent * 100 )* difficulty);
        }

        if ((battleLeftPlayer.HealthPercent <= 0 || RoundTime==0) && !roundEnd)
        {
            
            banner.showYouLose();
            roundEnd = true;
            HUD.score += 70 * difficulty;
            //Debug.Log("Test4");
        }
       
        if (!banner.IsAnimatingWinLose && (opponentsPlayer[round].currentState == FighterStates.DeadEnd || humanPlayer.currentState == FighterStates.Dead || roundTime == 0) && PlayerPrefs.GetInt("Survivor") == 0 && show == false)
        {
            battleLeftPlayer.enable = false;
            show = true;
            HUD.score += 500 * difficulty;
            ScoreShow();
            //Debug.Log("Test3");
        }
        if (!banner.IsAnimatingWinLose && (humanPlayer.HealthPercent == 0 || roundTime == 0) && PlayerPrefs.GetInt("Survivor") == 1 && show == false)
        {
            battleLeftPlayer.enable = false;
            show = true;
            HUD.score += 70 * difficulty;
            ScoreShow();
            //Debug.Log("Test1");
        }

        //Permet d'enlever le collider de l'IA pour donner l'effet d'enfoncement du joueur dans l'eau dans le jeu 
        //ainsi que d'éviter que l'IA ne géne les déplacements des joueurs du prochain round
        if (opponentsPlayer[round].currentState == FighterStates.DeadEnd && PlayerPrefs.GetInt("Survivor") == 1)
        {
            opponentsPlayer[round].capsule.enabled = false;
        }
        
        if (!banner.IsAnimatingWinLose && opponentsPlayer[round].capsule.enabled == false
            && roundTime > 1 && humanPlayer.HealthPercent > 0 && PlayerPrefs.GetInt("Survivor") == 1)
        {
            HUD.score += 100 * difficulty + roundTime * round;
            if (round + 1 <= 5)
            {
                round += 1;
                SurvivorGame(humanPlayer, round);
            }
            if (round==5)
            {
                boss.SetActive(false);
            }
            if (round == 5 && opponentsPlayer[5].currentState == FighterStates.DeadEnd && show == false)
            {
                HUD.score += 500 * difficulty;
                ScoreShow();
                //Debug.Log("Test2");
            }
        }
        
     
    }
    //Permet d 'afficher le score de fin de partie
    public void ScoreShow()
    {
        HUD.scoreText.color = Color.clear;
        finalPanelScore.SetActive(true);
        show = true;
        Cursor.visible = true;

        if (HUD.score >= PlayerPrefs.GetInt("ScoreGame"))
        {
            PlayerPrefs.SetString("NamePlayer", humanPlayer.fighterName);
            PlayerPrefs.SetInt("ScoreGame", HUD.score);
        }
        highScore.text = PlayerPrefs.GetInt("ScoreGame", HUD.score).ToString();
        finalScore.text = HUD.score.ToString();

    }
    //Permet de charger la scene du menu
    public void LoadNewScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    //Permet d'instancier les différents composant du jeu 
    public void InstanciateGameUtils(Fighter humanPlayer, Fighter[] IA, int round)
    {
        humanPlayer.opponent = IA[round];
        opponentsPlayer[round].opponent = humanPlayer;
        cameraMov.leftFighter = humanPlayer;
        cameraMov.rightFighter = IA[round];
        Lion2.fighter2 = IA[round];
        Lion1.fighter2 = IA[round];

        HUD.player1 = humanPlayer;
        HUD.player2 = IA[round];
        HUD.tagPlayer1.text = HUD.player1.fighterName;
        if (round != 5)
        {
            HUD.tagPlayer2.text = HUD.player2.fighterName + " IA";
        }
        else
        {
            HUD.tagPlayer2.text = "Final Boss";
        }
        HUD.RightHealtBar.size = 1.0f;
        Footer.player1 = humanPlayer;
        Footer.player2 = IA[round];
        battleLeftPlayer = humanPlayer;
        battleRightPlayer = IA[round];
    }
    //Permet d'instancier les différents composant du jeu pour chaque round
    public void SurvivorGame(Fighter player, int round)
    {

        player.Animator.ResetTrigger("Punch");
        player.Animator.ResetTrigger("Kick");
        roundTime = 200;
        lastTimeUpdate = 0;
        battleStarted = false;
        roundEnd = false;
        player.enable = false;
        banner.ShowRound();
        opponentsPlayer[round] = Instantiate(IA[round], opponentsPlayer[round].transform.position = new Vector3(0, 0, 41), Quaternion.identity);
        opponentsPlayer[round].opponent = player;
        player.opponent = opponentsPlayer[round];
        InstanciateGameUtils(player, opponentsPlayer, round);
    }
}
