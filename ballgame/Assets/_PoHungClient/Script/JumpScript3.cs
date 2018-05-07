﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript3 : MonoBehaviour {


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
                Jump();
            }
            
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.CompareTag("Ground");
        onGround = true;
    }

    public void Jump()
    {

        if (onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHigh, rb.velocity.z);
            onGround = false;
        }
                
            
        
    }



}
