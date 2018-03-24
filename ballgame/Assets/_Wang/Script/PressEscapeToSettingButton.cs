using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEscapeToSettingButton : MonoBehaviour {

    //public int sceneNumber;
    public string scenename;

    public void ChangeScene()
    {
        SceneManager.LoadScene(scenename);
        //SceneManager.LoadScene(sceneNumber - 1);
        //Application.LoadLevel(sceneNumber-1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ChangeScene();
        }

    }
}
