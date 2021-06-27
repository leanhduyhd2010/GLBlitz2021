using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Collider2D cl;
    public float DAMAGE = 20f;

    private void Awake()
    {
        cl = GetComponent<Collider2D>();
        if (cl == null)
        {
            Debug.Log("No collider found");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cl.isTrigger = true;
        if (collision.gameObject.CompareTag("Player"))
        {
            Handheld.Vibrate();
            GameManager.instance.SetHealth(GameManager.instance.GetHealth() - DAMAGE);
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
