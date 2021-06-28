using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : Bonus
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.SetHealth(GameManager.instance.GetHealth() + GameManager.instance.BONUS_HEALTH);
        GameManager.instance.ShowHealthBonus(GameManager.instance.BONUS_HEALTH);
        SoundManager.instance.PlayOnGetHealthBonusSound();
        Destroy(gameObject);
    }
}
