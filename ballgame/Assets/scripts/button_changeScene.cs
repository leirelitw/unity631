using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_changeScene : MonoBehaviour {

    public int sceneNumber;
	
    public void ChangeScene()
    {
        Application.LoadLevel(sceneNumber);
    }

	// Update is called once per frame
	void Update () {
        
	}
}
