using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraScore : Bonus
{
    public int SCORE_BONUS = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.SetScore(GameManager.instance.GetScore() + SCORE_BONUS);
        GameManager.instance.ShowBonusScore(SCORE_BONUS);
        Destroy(gameObject);
    }
}
