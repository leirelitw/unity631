using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    // Constants
    private string SESSION_ID = "";
    private int game_id = -1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setSessionID(string session_id)
    {
        SESSION_ID = session_id;
    }

    public string getSessionID()
    {
        return SESSION_ID;
    }

    public void setGameID(int new_game_id)
    {
        game_id = new_game_id;
    }

    public int getGameID()
    {
        return game_id;
    }
}
