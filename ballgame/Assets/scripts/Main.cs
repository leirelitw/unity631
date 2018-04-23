using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(gameObject);

        GameObject main = GameObject.Find("MainObject");

        if (main == null)
            Debug.Log("Main is null...");
        else
            Debug.Log("Main is NOT null...");
        
        main.AddComponent<ConnectionManager>();
        main.AddComponent<PlayerHandler>();

    }
	
	// Use this for initialization
	void Start () {
		SceneManager.LoadScene("Login");
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
