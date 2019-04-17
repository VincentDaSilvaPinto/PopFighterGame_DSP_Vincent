using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDController : MonoBehaviour
{
    public Text tagPlayer1;
    public Text tagPlayer2;
    public Text scoreText;
    public int score=0;
    public Fighter player1;
    public Fighter player2;

    public Scrollbar LeftHealtBar;
    public Scrollbar RightHealtBar;

    public BattleController battle;
    public Text timer;
   

    void Update()
    {
        timer.text = battle.RoundTime.ToString();

        if (LeftHealtBar.size > player1.HealthPercent)
        {
            LeftHealtBar.size -= 0.01f;
        }
        if (RightHealtBar.size > player2.HealthPercent)
        {
            RightHealtBar.size -= 0.01f;
            score += 200;
        }
         
    }
}
