using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusText : MonoBehaviour
{
    private float countingTime = 0;
    public float lifeTime = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive())
        {
            countingTime += Time.deltaTime;
            Color textureColor = gameObject.GetComponent<Text>().color;
            float a = (lifeTime - countingTime) / lifeTime;
            if (a > 0)
            {
                textureColor.a = a;
                gameObject.GetComponent<Text>().color = textureColor;
            }
            else
            {
                countingTime = 0;
                SetActive(false);
                textureColor.a = 255.0f;
                gameObject.GetComponent<Text>().color = textureColor;
            }
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void ShowText(string str)
    {
        SetActive(true);
        gameObject.GetComponent<Text>().text = str;
    }
}
