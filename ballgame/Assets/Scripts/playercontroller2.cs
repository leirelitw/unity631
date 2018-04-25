using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller2 : MonoBehaviour {

    public float speed = 100f;
    public GameObject playerballobj;

    //public GameObject cameraobj;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 force;

    public Transform objA_cam;
    public Transform objB_player;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        pointA = objA_cam.position;
        pointB = objB_player.position;

        force = pointB - pointA;
        


        if (Input.GetKeyDown("w"))
        {
            //Debug.Log("Press w");
            playerballobj.GetComponent<Rigidbody>().AddForce(force * speed);
        }
        else if(Input.GetKeyDown("s"))
        {
            
            playerballobj.GetComponent<Rigidbody>().AddForce(-force * speed);
        }







    }
}
