using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction = Vector3.right;

    float speed;

    private void Start()
    {
        speed = GameManager.instance.GetPlayerSpeed();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetHealth() <= 0) { return; }
        //transform.Translate(direction * SPEED * Time.deltaTime);   
        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touchPos.x > 0)
            {
                speed = GameManager.instance.GetPlayerSpeed();
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                speed = -GameManager.instance.GetPlayerSpeed();
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            speed = -GameManager.instance.GetPlayerSpeed();
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            speed = GameManager.instance.GetPlayerSpeed();
            transform.localScale = new Vector3(1, 1, 1);
        }



    }

    private void OnBecameInvisible()
    {
        GameManager.instance.SetHealth(0);
    }
}
