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
            SoundManager.instance.PlayPlayerOnHitSound();
            if (gameObject.name.Contains("MaceCircle"))
            {
                SoundManager.instance.PlayMaceOnPlayerSound();
            } 
            else if (gameObject.name.Contains("MaceSquare"))
            {
                SoundManager.instance.PlaySquareMaceOnPlayerSound();
            }
            else if (gameObject.name.Contains("Saw"))
            {
                SoundManager.instance.PlaySawOnPlayerSound();
            }
            else if (gameObject.name.Contains("Spike"))
            {
                SoundManager.instance.PlaySpikeOnPlayerSound();
            }
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            countingTime = 0f;
            isCollision = true;
            if (gameObject.name.Contains("MaceCircle"))
            {
                SoundManager.instance.PlayMaceOnPlatformSound();
            }
            else if (gameObject.name.Contains("MaceSquare"))
            {
                SoundManager.instance.PlaySquareMaceOnPlatformSound();
            }
            else if (gameObject.name.Contains("Saw"))
            {
                SoundManager.instance.PlaySawOnPlatformSound();
            }
            else if (gameObject.name.Contains("Spike"))
            {
                SoundManager.instance.PlaySpikeOnPlatformSound();
            }
        }


    }

    private void OnBecameInvisible()
    {
        if (!isCollision) // if obs doesn't fall in player or platform, then also decrease the number of obs
            GameManager.instance.DecreaseNumberOfObstacle();
        Destroy(gameObject);
    }
}
