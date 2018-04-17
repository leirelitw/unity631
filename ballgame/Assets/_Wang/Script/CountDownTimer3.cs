using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer3 : MonoBehaviour
{
    public int startTime = 3;
    public float timeLeft = 5f;
    public Text countdownText;


    private bool firstCount = true; //3,2,1, Start!!
    private bool canCount = true;
    private float timer;


    // Use this for initialization
    void Start()
    {
        StartCoroutine("LoseTime");
        timer = timeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime >= 0 && firstCount)
        {
            countdownText.text = startTime.ToString();
            if (startTime == 0)
            {
                StopCoroutine("LoseTime");
                countdownText.text = "Start!!";
                StartCoroutine("WaitOneSec");      
                
            }
        }
        else if (timer > 0.0f && canCount)
        {
            StopCoroutine("WaitOneSec");
            timer -= Time.deltaTime;
            countdownText.text = timer.ToString("F");
        }
        else if (timer <= 0.0f)
        {

            countdownText.text = "Times Up!";
            //GameOver();
        }
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            startTime--;

            
        }
        
    }
    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(1);
        firstCount = false;

    }

    void GameOver()
    {
        //Change Scene

    }



}