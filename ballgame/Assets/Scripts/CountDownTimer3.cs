using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDownTimer3 : MonoBehaviour
{
	private int startTime = 6;
	private int timeLeft = 100;
	private int minutes = 1;
	private int seconds = 1;
	public Text countdownText;

	private bool firstCount = true; //3,2,1, Start!!

	// Use this for initialization
	void Start(){
		StartCoroutine("LoseTime");
	}

	// Update is called once per frame
	void Update()
	{
		if(startTime >= 0 && firstCount){
			countdownText.text = "";
			if (startTime == 0){
				countdownText.text = "";
				firstCount = false;     
			}
		}
		else if (timeLeft > 0 && !firstCount){
			minutes = timeLeft / 60;
			seconds = timeLeft % 60;
			if (seconds > 9) {
				countdownText.text = "0" + minutes.ToString () + ":" + seconds.ToString ();
			} else {
				countdownText.text = "0" + minutes.ToString () + ":0" + seconds.ToString ();
			}
		}
		else if (timeLeft <= 0){
			countdownText.text = "Times Up!";
			StopCoroutine("LoseTime");
			GameOver();
		}
	}

	IEnumerator LoseTime(){
		while (true){
			yield return new WaitForSeconds(1);
			if (startTime > 0) {
				startTime--;
			} else {
				timeLeft--;
			}
		}
	}

	void GameOver(){
		SceneManager.LoadScene("Ranking");
	}
		
}