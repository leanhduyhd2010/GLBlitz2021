using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction = Vector3.right;
    [SerializeReference]
    CharacterController2D charr;

    float speed;
    int speedDirection;

    private void Start()
    {
        speedDirection = 1;
        speed = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameStart() || GameManager.instance.GetHealth() <= 0) { return; }

        speed = GameManager.instance.GetPlayerSpeed();
        charr.Move(speed * speedDirection * Time.deltaTime, false, false);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                speedDirection = -speedDirection;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            speedDirection = -speedDirection;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Die"))
        {
            GameManager.instance.SetHealth(0);
        }
    }


    private void OnBecameInvisible()
    {
    }
}
