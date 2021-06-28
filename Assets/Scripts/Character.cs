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

        //transform.Translate(direction * SPEED * Time.deltaTime);   
        //rb.velocity = new Vector2(speed, rb.velocity.y);


        /*if (transform.eulerAngles.z < GameManager.instance.MIN_PLAYER_ROTATION)
            transform.eulerAngles = new Vector3(0, 0, GameManager.instance.MIN_PLAYER_ROTATION);

        if (transform.eulerAngles.z > GameManager.instance.MAX_PLAYER_ROTATION)
            transform.eulerAngles = new Vector3(0, 0, GameManager.instance.MAX_PLAYER_ROTATION);
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                speedDirection = -speedDirection;
            }
            if (touchPos.x > 0)
            {
                speedDirection = -speedDirection;
                speed = GameManager.instance.GetPlayerSpeed();
                //transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                speedDirection = -speedDirection;
                speed = -GameManager.instance.GetPlayerSpeed();
                //transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            speedDirection = -speedDirection;
            speed = GameManager.instance.GetPlayerSpeed();
            //transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            speedDirection = -speedDirection;
            speed = GameManager.instance.GetPlayerSpeed();
            //transform.localScale = new Vector3(1, 1, 1);
        }*/



    }

    private void OnBecameInvisible()
    {
        GameManager.instance.SetHealth(0);
    }
}
