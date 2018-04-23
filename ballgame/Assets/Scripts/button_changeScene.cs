using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_changeScene : MonoBehaviour {

	public string sceneName;
	
    public void ChangeScene(){
		SceneManager.LoadScene(sceneName);
		//Application.LoadLevel(sceneNumber-1);
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")){
			ChangeScene();
		}
	}
}
