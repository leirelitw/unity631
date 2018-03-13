using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_changeScene : MonoBehaviour {

    public int sceneNumber;
	
    public void ChangeScene(){
		SceneManager.LoadScene(sceneNumber);
		//Application.LoadLevel(sceneNumber);
    }

	// Update is called once per frame
	void Update () {
        
	}
}
