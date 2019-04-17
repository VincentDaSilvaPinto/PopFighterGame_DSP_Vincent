using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerController : MonoBehaviour
{
    private Animator animatorBanner;
    private bool animating;
    private bool animatingWinLose;
    public bool IsAnimating { get { return animating; } }

    public bool IsAnimatingWinLose { get { return animatingWinLose; } }
    void Start()
    {
        animatorBanner = GetComponent<Animator>();
    }
    public void ShowRound()
    {
        animating = true;
        animatorBanner.SetTrigger("ShowRound");
    }
    public void showYouWin()
    {
        animatingWinLose = true;
        animatorBanner.SetTrigger("YouWin");
    }

    public void showYouLose()
    {
        animatingWinLose = true;
        animatorBanner.SetTrigger("YouLose");
    }
    public void AnimationRoundEnded()
    {
        animating = false;
    }

    public void AnimationWinLoseEnded()
    {
        animatingWinLose = false;
    }

  

}
