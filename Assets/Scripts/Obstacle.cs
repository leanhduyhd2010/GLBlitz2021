using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Collider2D cl;
    public float DAMAGE = 20f;
    public float durationLifeTime = 1f;

    private float countingTime;
    private bool isCollision = false;

    private void Awake()
    {
        cl = GetComponent<Collider2D>();
        if (cl == null)
        {
            Debug.Log("No collider found");
        }
        isCollision = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isCollision)
        {
            countingTime += Time.deltaTime;
            var textureColor = gameObject.GetComponent<Renderer>().material.color;
            float a = (durationLifeTime - countingTime) / durationLifeTime;
            if (a > 0)
            {
                textureColor.a = a;
                gameObject.GetComponent<Renderer>().material.color = textureColor;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cl.isTrigger = true;
        GameManager.instance.DecreaseNumberOfObstacle();
        if (collision.gameObject.CompareTag("Player"))
        {
            Handheld.Vibrate();
            GameManager.instance.SetHealth(GameManager.instance.GetHealth() - DAMAGE);
            countingTime = 0f;
            isCollision = true;

        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            countingTime = 0f;
            isCollision = true;
        }


    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
