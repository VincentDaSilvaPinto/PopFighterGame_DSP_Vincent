using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FooterController : MonoBehaviour
{
    public Text tagLevelPlayer1;
    public Text tagLevelPlayer2;

    public Fighter player1;
    public Fighter player2;

    public Scrollbar LeftPowerBar;
    public Scrollbar RightPowerBar;
    

    void Update()
    {
        tagLevelPlayer1.text = Mathf.Floor(player1.powerBarPercent*7).ToString();
        tagLevelPlayer2.text = Mathf.Floor(player2.powerBarPercent*7).ToString();

        if (LeftPowerBar.size > player1.powerBarPercent)
        {
            LeftPowerBar.size -= 0.01f;
        }
        if (RightPowerBar.size > player2.powerBarPercent)
        {
            RightPowerBar.size -= 0.01f;
        }

        if (LeftPowerBar.size < player1.powerBarPercent)
        {
            LeftPowerBar.size += 0.01f;
        }
        if (RightPowerBar.size < player2.powerBarPercent)
        {
            RightPowerBar.size += 0.01f;
        }

    }

}
