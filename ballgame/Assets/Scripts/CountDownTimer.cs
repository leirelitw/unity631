using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

	public int timeLeft = 5;
	public Text countdownText;
	private int finish = 0;
	// Use this for initialization
	void Start () {
		StartCoroutine("LoseTime");
	}

	// Update is called once per frame
	void Update () {
		countdownText.text = (""+timeLeft);
		if(timeLeft <= 0){
			countdownText.text = "Start!!";
			StartCoroutine("WaitOneSec");      
		}
		if (finish == 1) {
			countdownText.text = "";
			StopCoroutine("WaitOneSec");  
		}
	}

	IEnumerator LoseTime(){
		while (true){
			yield return new WaitForSeconds(1);
			timeLeft--;
		}
	}
	IEnumerator WaitOneSec(){
		yield return new WaitForSeconds(1);
		finish = 1;
	}
}
