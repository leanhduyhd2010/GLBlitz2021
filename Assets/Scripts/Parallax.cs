using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float FALLING_SPEED;

    private float length;

    private void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GetHealth() <= 0) { return; }
        transform.Translate(Vector3.left * FALLING_SPEED * GameManager.instance.GetProfile().difficultyLevel * Time.deltaTime);

        if (transform.position.y + length < -GameManager.instance.GetWorldScreenSizeY())
        {
            float newY = GameManager.instance.GetWorldScreenSizeY() + length * 2;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // random flip obj
            if (Random.Range(0,1) > 0.5)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
