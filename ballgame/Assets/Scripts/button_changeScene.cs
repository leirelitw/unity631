using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_changeScene : MonoBehaviour {

	public string sceneName;
    public AudioSource buttonPressed;

    public void ChangeScene(){
        buttonPressed.Play();
        SceneManager.LoadScene(sceneName);
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")){
			ChangeScene();
		}
	}
}
