using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public GameObject theEnd;

    [Header("Component")]
    public TextMeshProUGUI timerText;
    

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public bool hasLimit;
    public float timerLimit;
    public bool isRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        
            if ((hasLimit && ((countDown && currentTime <= timerLimit)) || (countDown && currentTime >= timerLimit)))
            {
                currentTime = timerLimit;
            }

            if (isRunning) timerText.text = currentTime.ToString();
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("the end");
            isRunning = false;
        }
    }
    
}
