using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Disable_Script : MonoBehaviour {


    public GameObject enableDisableObject;
    public GameObject playerObject;
    //public GameObject cameraObject;

    private bool status = false;

	// Use this for initialization
	public void Enable () {
        status = true;
        enableDisableObject.SetActive(status);
        //To pause player object by disable scripts relate
        playerObject.GetComponent<SinglePlayerController>().enabled = !status;
        //cameraObject.GetComponent<CamRotation>().enabled = !status;
        //Cursor.lockState = CursorLockMode.None;
        


    }
	
	// Update is called once per frame
	public void Disable () {
        status = false;

        enableDisableObject.SetActive(status);
        //enable player object movement
        playerObject.GetComponent<SinglePlayerController>().enabled = !status;
        //cameraObject.GetComponent<CamRotation>().enabled = !status;
        //Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        

        if (Input.GetKeyDown("escape"))
        {
            if(status == true)
            {
                Disable();
                
            }
            else
            {
                Enable();
                
            }
        }
    }
}
