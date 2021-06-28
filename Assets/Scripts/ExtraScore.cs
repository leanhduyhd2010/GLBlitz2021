using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraScore : Bonus
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.SetScore(GameManager.instance.GetScore() + GameManager.instance.BONUS_SCORE);
        GameManager.instance.ShowBonusScore(GameManager.instance.BONUS_SCORE);
        SoundManager.instance.PlayOnGetScoreBonusSound();
        Destroy(gameObject);
    }
}
