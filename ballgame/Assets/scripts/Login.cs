using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
	
	private GameObject main;
    private ConnectionManager con_man;
    private PlayerHandler player;
	// Window Properties
	private float width = 280;
	private float height = 100;
	// Other
	public Texture background;
	private string user_id = "";
	private string password = "";
	private Rect windowRect;
	private bool isHidden;
	
	void Awake() {
        main = GameObject.Find("MainObject");
        con_man = main.GetComponent<ConnectionManager>();
        player = main.GetComponent<PlayerHandler>();
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
			windowRect = GUILayout.Window((int) Constants.GUI_ID.Login, windowRect, MakeWindow, "Login");
		
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
				Submit();
			}
		}
	}
	
	void MakeWindow(int id) {
		GUILayout.Label("User ID");
		GUI.SetNextControlName("username_field");
		user_id = GUI.TextField(new Rect(30, 45, windowRect.width - 60, 30), user_id, 25);

		GUILayout.Space(50);
		
		GUILayout.Label("Password");
		GUI.SetNextControlName("password_field");
		password = GUI.PasswordField(new Rect(30, 100, windowRect.width - 60, 30), password, "*"[0], 25);
		
		GUILayout.Space(100);

		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 145, 100, 30), "Log In")) {
			Submit();
		}
		if (GUI.Button(new Rect(windowRect.width / 2 - 50, 185, 100, 30), "Sign up")) {
			SceneManager.LoadSceneAsync("SignUp");
		}
		
	}
	
	public bool Submit() {
		user_id = user_id.Trim();
		password = password.Trim();
		
		if (user_id.Length == 0) {
			Debug.Log("User ID Required");
			GUI.FocusControl("username_field");
            return false;
		} else if (password.Length == 0) {
			Debug.Log("Password Required");
			GUI.FocusControl("password_field");
            return false;
		} else {

            //tests request sending
            Debug.Log("onTriggerEnter()");

            Debug.Log("get connection manager");
            if (con_man == null)
                Debug.Log("ConnectionManager is null....");
            else
                Debug.Log("ConnectionManager is NOT null");
            //con_man.setupSocket();
            con_man.send("/login?username="+user_id+"&password="+password, Constants.response_login, ResponseLogin);
            Debug.Log("Sent request");
            return true;
        }


	}
    

    public IEnumerator ResponseLogin(Response eventArgs)
    {
        Debug.Log("ResponseLogin(): " + eventArgs.response);

        if (eventArgs.response != "error")
        {
            player.setSessionID(eventArgs.response);
            SceneManager.LoadScene("Lobby");
        }
        else
            Debug.Log("Invalid login...");

        yield return 0;
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
