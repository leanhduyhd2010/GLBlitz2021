using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : Bonus
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float newHealth = GameManager.instance.GetHealth() + GameManager.instance.BONUS_HEALTH;
        if (newHealth > GameManager.instance.GetPlayerMaxHealth())
            newHealth = GameManager.instance.GetPlayerMaxHealth();
        GameManager.instance.SetHealth(newHealth);
        GameManager.instance.ShowHealthBonus(GameManager.instance.BONUS_HEALTH);
        SoundManager.instance.PlayOnGetHealthBonusSound();
        Destroy(gameObject);
    }
}
