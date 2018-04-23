using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer2 : MonoBehaviour
{

	public float timeLeft = 5f;
	public Text countdownText;

	private bool canCount = true;
	private float timer;


	// Use this for initialization
	void Start()
	{
		timer = timeLeft;
	}

	// Update is called once per frame
	void Update()
	{
		if(timer > 0.0f && canCount)
		{
			timer -= Time.deltaTime;
			countdownText.text = timer.ToString("F");
		}
		else if (timer <= 0.0f)
		{

			countdownText.text = "Times Up!";
			//GameOver();
		}
	}


	void GameOver()
	{
		//Change Scene

	}

}