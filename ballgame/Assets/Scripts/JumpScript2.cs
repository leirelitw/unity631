using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript2 : MonoBehaviour {


    public float jumpHigh = 20f;
    private bool onGround = true;
    private Rigidbody rb;
    


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if (onGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector3(0f, jumpHigh, 0f);
                onGround = false;
            }
        }



	}

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.CompareTag("Ground");
        onGround = true;
    }
}
