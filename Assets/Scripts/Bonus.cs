using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public float FALL_SPEED = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * FALL_SPEED * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
