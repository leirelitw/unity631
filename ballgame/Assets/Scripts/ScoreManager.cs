using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	public class ScoreManager : MonoBehaviour {

	Dictionary <string, Dictionary<string, int>> playerScores;

	void Start(){
		SetScore ("Leire", "points", 321);
	}

	void Init(){
		if(playerScores == null){
			return;
		}
		playerScores = new Dictionary <string, Dictionary<string, int>> ();
	}

	public int GetScore(string username, string scoreType){
		Init ();

		if(playerScores.ContainsKey(username) == false){
			return 0;
		}

		if(playerScores[username].ContainsKey(scoreType) == false){
			return 0;
		}

		return playerScores[username][scoreType];
	}

	public void SetScore(string username, string scoreType, int value){
		Init ();

		if(playerScores.ContainsKey(username) == false){
			playerScores[username] = new Dictionary<string, int> ();
		}

		playerScores[username][scoreType] = value;
	}

}

