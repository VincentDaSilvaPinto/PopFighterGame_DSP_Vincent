using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private static float MaxHealth = 100f;
    private static float MaxPowerBar = 7f;
    private float health = MaxHealth;
    private float powerBar = 0f;
    private AudioSource audioPlayer;
    private Position currentRotation;
    private Animator animator;
    public float HealthPercent
    {
        get { return health / MaxHealth; }
        set { if (value <= 1 && value >= 0) { HealthPercent = value; } }
    }
    public float powerBarPercent { get { return powerBar / MaxPowerBar; } }

    public string fighterName;
    public Fighter opponent;
    public PlayerType playerType;
    public FighterStates currentState = FighterStates.Idle;
    public bool usePower;

    public bool enable;
    private Rigidbody body;
    public CapsuleCollider capsule;
    public Rigidbody Body { get { return this.body; } }

    public Animator Animator { get { return animator; } }

    //Pour IA
    private float random;
    private float randomSetTime;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        enable = false;
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Yes") == 1)
        {
            // Debug.Log(" effet sonor est à " + PlayerPrefs.GetInt("Yes"));
            audioPlayer.volume = 1;
            audioPlayer.priority = 256;
        }
        else { audioPlayer.volume = 0; }

        //Permet de savoir si le joueurs est à Gauche/Droite de son adversaire
        currentRotation = PlayerPosition();

        //Permet de fixer les joueurs sur l'axe z en fixant le fxant sur x à 0
        transform.SetPositionAndRotation(new Vector3(0, transform.position.y, transform.position.z), transform.rotation);

        // Permet d'avoir l'orientation des joueurs 
        int motion = Orientation(currentRotation);

        //Permet de fixer la rotation des joueurs 
        if (!Attack && currentState != FighterStates.Hit && currentState != FighterStates.Hit_Fall && currentState != FighterStates.Hit_Power)
        {
            if (motion == 1)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        //Permet de connaitre sa hauteur par rapport au sol
        animator.SetFloat("DistanceFloor", GetDistanceFloor());

        //Permet de connaitre la distance entre les deux joueurs
        animator.SetFloat("DistanceOpponent", GetDistanceOpponnent());

        //Permet de savoir si le joueur est au sol
        if (transform.position.y <= 0)
        {
            animator.SetBool("Floor", true);
        }
        else
        {
            animator.SetBool("Floor", false);
            if (playerType == PlayerType.Joueur)
            {
                animator.SetBool("Jump", false); animator.SetBool("JumpFoward", false);
            }
        }
        if ((currentState == FighterStates.JumpFall || currentState == FighterStates.Idle) && GetDistanceOpponnent() <= 3 && GetDistanceFloor() >= 4)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z - motion * 4), Time.deltaTime * 7);
        }

        //permet ce savoir si le joueur use d'un pouvoir
        if (currentState == FighterStates.PowerPerso)
        {
            usePower = true;
            opponent.animator.SetTrigger("UsePowerOpponent");

        }
        if (currentState != FighterStates.PowerPerso)
        {
            usePower = false;
        }

        //Permet de prévnir d'une sortie de terrain 
        if (transform.position.z >= 230)
        {
            transform.position = new Vector3(0, 0, 225);
        }

        if (transform.position.z <= -100)
        {
            transform.position = new Vector3(0, 0, -95);
        }
        if (transform.position.y <= -1 && currentState != FighterStates.DeadEnd)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }

        //Permet de figer le joueur lors d'une teleportation
        if (opponent.currentState == FighterStates.Teleportation)
        {
            animator.SetTrigger("UseTeleport");
        }

        //Permet de bloquer les joeurs avant lancement de la partie
        if (enable)
        {
            if (playerType == PlayerType.Joueur && currentState != FighterStates.Dead)
            {
                UpdateJoueur1Input(motion);
            }
            if (playerType == PlayerType.IA)
            {
                UpdateIA(motion);
            }
        }

        animator.SetFloat("Health", HealthPercent);
        animator.SetFloat("PowerBar", powerBarPercent);

        if (opponent != null)
        {
            animator.SetFloat("OpponentHealth", opponent.HealthPercent);
        }
        else
        {
            animator.SetFloat("opponentHealth", 1);
        }

        if (health <= 0 && (currentState != FighterStates.Dead && currentState != FighterStates.DeadEnd))
        {
            animator.SetTrigger("Death");
        }
        if (health >= 0 && (currentState != FighterStates.Celebrate && (opponent.currentState == FighterStates.DeadEnd || opponent.currentState == FighterStates.Dead)))
        {
            animator.SetTrigger("Celebrate");
        }
    }

    //  Permet d'avoir des effets sonors liés aux animations jouées
    public void PlaySound(AudioClip sound)
    {
        AudioUtils.PlaySound(sound, audioPlayer);
    }

    // Permet de connaitre la position du personnage par rapport à l'autre 
    public Position PlayerPosition()
    {
        if ((transform.position.z < opponent.transform.position.z) && animator.GetBool("Floor")) { return currentRotation = Position.Droite; }
        if ((transform.position.z > opponent.transform.position.z) && animator.GetBool("Floor")) { return currentRotation = Position.Gauche; }
        else return currentRotation;
    }

    //  Permet d'orienter les déplacement du personnage Gauche/Droite
    public int Orientation(Position position)
    {
        if (position == Position.Gauche)
        { return 1; }
        if (position == Position.Droite) { return -1; }
        else return 0;
    }

    //  Permet de determiner la distance entre les joueurs
    public float GetDistanceOpponnent()
    {
        return (Mathf.Abs(transform.position.z - opponent.transform.position.z));
    }

    //  Permet de determiner la distance entre le joueur et le sol
    private float GetDistanceFloor()
    {
        return (transform.position.y);
    }

    // Le personnag est invunérable s'il répond à ces condition d'état
    public bool Invunerable
    {
        get
        {
            return ((currentState == FighterStates.Hit_Defend && opponent.currentState != FighterStates.Combo) || currentState == FighterStates.Dead ||
                (currentState == FighterStates.Defend && opponent.currentState != FighterStates.Combo) || currentState == FighterStates.Teleportation ||
                currentState == FighterStates.PowerPerso || currentState == FighterStates.Combo || currentState == FighterStates.Hit_Fall || currentState == FighterStates.Hit_Power ||
                Attack && opponent.currentState == FighterStates.Hit);
        }
    }
    //  Permet de savoir si le personnage defend
    public bool Defend
    {
        get { return (currentState == FighterStates.Defend || currentState == FighterStates.Hit_Defend); }
    }

    //  Permet de savoir si le personnage attaque
    public bool Attack
    {
        get
        {
            return (currentState == FighterStates.PunchL || currentState == FighterStates.PunchR ||
              currentState == FighterStates.KickL || currentState == FighterStates.KickR ||
              currentState == FighterStates.DoubleKick || currentState == FighterStates.DoublePunch
              || currentState == FighterStates.Combo || currentState == FighterStates.PowerPerso || currentState == FighterStates.PunchUpperCut
              || currentState == FighterStates.JumpAttack);
        }
    }

    //  Permet de savoir si le personnage fait un Combo
    public bool AttackCombo
    {
        get
        {
            return (currentState == FighterStates.Combo);
        }
    }

    //  Renvoyer un string lié au type de l'attaque
    public string TypeOfHit(FighterStates AttackPart)
    {
        if (opponent.Attack)
        { return AttackPart.ToString(); }
        else return "Recovery";
    }

    // Retirer de la vie au personnage 
    public virtual void Damage(float damage)
    {
        if (!Invunerable && Orientation(currentRotation) == opponent.Orientation(currentRotation))
        {

            if (health >= damage) { health -= damage; }
            else { health = 0; }

            if (health > 0)
            {
                //Debug.Log("Je suis touche par " + opponent + "avec " + TypeOfHit(opponent.currentState));
                animator.SetTrigger(TypeOfHit(opponent.currentState));
            }

            if (powerBar < MaxPowerBar) { powerBar += 0.1f; }
            if (opponent.powerBar < MaxPowerBar) { opponent.powerBar += 0.05f; }
        }
        else if (currentState == FighterStates.Hit_Defend || currentState == FighterStates.Defend)
        {
            animator.SetTrigger("Hit-Defend"); opponent.powerBar += 0.05f;
        }
    }


    //  Entrer si IA
    public void UpdateIA(int motion)
    {
        animator.SetBool("AttackingOpponent", opponent.Attack);

        animator.SetBool("OpponentFall", opponent.currentState == FighterStates.Hit_Fall || opponent.currentState == FighterStates.Hit_Power);
        animator.SetBool("Enable", enable);

        if (currentState == FighterStates.PowerPerso)
        {
            powerBar = 0;
        }
        if (currentState == FighterStates.Teleportation && currentState != FighterStates.Hit_Power)
        {
            if (!opponent.animator.GetBool("Floor"))
            {
                opponent.animator.SetTrigger("Recovery");
            }
            if (opponent.currentState == FighterStates.PowerPerso)
            {
                transform.SetPositionAndRotation(new Vector3(0, 0, opponent.transform.position.z - motion * 5.5f), transform.rotation);
            }
            else
            {
                transform.SetPositionAndRotation(new Vector3(0, opponent.transform.position.y, opponent.transform.position.z - motion * 5.5f), transform.rotation);
            }

        }
        if (currentState == FighterStates.Load)
        {
            powerBar += 0.001f;
        }
        if (Time.time - randomSetTime > 1)
        {
            random = Random.Range(-1.0f, 1.0f);
            randomSetTime = Time.time;
        }
        animator.SetFloat("Random", random);

    }

    //  Entrer si joueur humain
    public void UpdateJoueur1Input(int motion)
    {
        //Courrir

        if (Input.GetKey(KeyCode.X))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Walk-Back", false);
            animator.SetBool("Walk", false);
            animator.ResetTrigger("Punch");
            animator.ResetTrigger("Kick");

            if (Input.GetKey(KeyCode.Q))
            {
                animator.SetTrigger("BigPunch");
            }
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetTrigger("BigKick");
            }
        }
        else { animator.SetBool("Run", false); }

        //Déplacements horizontaux
        //Marche 
        if (motion * Input.GetAxisRaw("Horizontal") > 0.1)
        {
            //saut avant
            if (Input.GetKeyDown(KeyCode.UpArrow) && animator.GetBool("Floor") && currentState != FighterStates.Hit && currentState != FighterStates.Hit_Fall)
            {

                animator.SetBool("JumpFoward", true);
                animator.SetBool("Jump", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Walk-Back", false);
                //Debug.Log(fighterName + " saut avant");

            }
            else { animator.SetBool("Walk", true); }

        }
        else { animator.SetBool("Walk", false); }

        if (motion * Input.GetAxis("Horizontal") < -0.1)
        {
            if (Input.GetKey(KeyCode.X)) { animator.SetBool("Run", true); animator.SetBool("Walk-Back", false); }

            else { animator.SetBool("Walk-Back", true); }

        }
        else { animator.SetBool("Walk-Back", false); }

        //Teleportation
        if (Input.GetKeyDown(KeyCode.Z) && powerBar >= 1 && currentState != FighterStates.Hit
            && currentState != FighterStates.Hit_Fall && currentState != FighterStates.Hit_Power)
        {
            if (!opponent.animator.GetBool("Floor"))
            {
                opponent.animator.SetTrigger("Recovery");
            }
            if (opponent.currentState == FighterStates.PowerPerso)
            {
                transform.SetPositionAndRotation(new Vector3(0, 0, opponent.transform.position.z - motion * 5.5f), transform.rotation);
            }
            else
            {
                transform.SetPositionAndRotation(new Vector3(0, opponent.transform.position.y, opponent.transform.position.z - motion * 6), transform.rotation);
            }
            animator.SetTrigger("Teleportation");
            powerBar -= 1;
        }

        // Permet au personnage envoyé dans les aires de se remettre droit
        if (currentState == FighterStates.Hit_Fall && !animator.GetBool("Floor"))
        {
            if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) ||
               Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && GetDistanceOpponnent() >= 10)
            {
                animator.SetTrigger("Recovery");
            }
        }
        //Defense
        if (Input.GetKey(KeyCode.S) && animator.GetBool("Floor"))
        {
            animator.SetBool("Defend", true);
        }
        if (Input.GetKeyUp(KeyCode.S)) { animator.SetBool("Defend", false); }

        //Saut simple
        if (Input.GetKeyDown(KeyCode.UpArrow) && motion * Input.GetAxisRaw("Horizontal") < 0.1)
        {
            animator.SetBool("Jump", true);
            //Debug.Log(fighterName + " saut simple");
        }

        //Attaque simple
        if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.D) && currentState != FighterStates.Hit && currentState != FighterStates.Load && currentState != FighterStates.DoublePunch)
        { animator.SetTrigger("Punch"); }

        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.Q) && currentState != FighterStates.Hit && currentState != FighterStates.Load && currentState != FighterStates.KickR)
        { animator.SetTrigger("Kick"); }

        // Attaque Combo
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Walk-Back", false);
            if (motion * Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetKeyUp(KeyCode.Q) && Input.GetKeyUp(KeyCode.D) && powerBar >= 2 && currentState != FighterStates.Hit && currentState != FighterStates.Hit_Fall)
                {
                    animator.SetTrigger("AttackCombo");
                    powerBar += -2;
                }
            }
            //Lancer attack lié au pouvoir
            if (motion * Input.GetAxis("Horizontal") < -0.1)
            {
                if (Input.GetKeyUp(KeyCode.Q) && Input.GetKeyUp(KeyCode.D) && powerBar >= 3 && opponent.currentState != FighterStates.PowerPerso && currentState != FighterStates.Hit && currentState != FighterStates.Hit_Fall && !Attack)
                {
                    animator.SetBool("Run", false);
                    animator.SetBool("Walk-Back", false);
                    animator.SetBool("Walk", false);
                    animator.ResetTrigger("Punch");
                    animator.ResetTrigger("Kick");
                    animator.SetTrigger("PowerAttack");
                    powerBar += -3;
                }
            }
        }

        // Recharge Pouvoir
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.D) && currentState != FighterStates.Hit &&
            currentState != FighterStates.Hit_Defend && currentState != FighterStates.Hit_Fall && animator.GetBool("Floor")
            && currentState != FighterStates.Defend && !Attack)
        {
            animator.SetBool("Load", true);
            if (powerBar < 7)
            {
                powerBar += 0.02f;
            }
            else { powerBar = 7; }
        }
        else { animator.SetBool("Load", false); }
    }
}

