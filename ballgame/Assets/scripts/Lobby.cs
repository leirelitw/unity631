using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

	//private GameObject mainObject;
	// Window Properties
	private float width = 280;
	private float height = 100;
	// Other
	public Texture background;
	private Rect windowRect;
	private bool isHidden;

	void Awake() {
		//mainObject = GameObject.Find("MainObject");

		//mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_AUTH, ResponseLogin);
	}

	// Use this for initialization
	void Start() {

	}

	void OnGUI() {
		// Background
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

		// Client Version Label
		GUI.Label(new Rect(Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Test");

		// Login Interface
		if (!isHidden) {
			windowRect = new Rect ((Screen.width - width) / 2, Screen.height / 2 - height, width, height);
			windowRect = GUILayout.Window((int) Constants.GUI_ID.Login, windowRect, MakeWindow, "Lobby");

			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
				//Submit();
			}
		}
	}

	void MakeWindow(int id) {

		GUILayout.Space(100);

		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 145, 100, 30), "Create a game")) {

		}
		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 185, 100, 30), "Join a game")) {

		}

		GUILayout.Space(100);

		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 145, 100, 30), "See Leaderboard")) {

		}
		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 185, 100, 30), "See Credits")) {

		}
		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 145, 100, 30), "Log out")) {
			SceneManager.LoadScene ("Login");
		}

	}
		

	public void Show() {
		isHidden = false;
	}

	public void Hide() {
		isHidden = true;
	}

	// Update is called once per frame
	void Update() {
	}
}
