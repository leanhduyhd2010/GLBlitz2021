using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPower : Bonus
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.SetSpeedBonusTime(GameManager.instance.GetSpeedBonusTime() + GameManager.instance.SPEED_BONUS_TIME);
        Destroy(gameObject);
    }
}
