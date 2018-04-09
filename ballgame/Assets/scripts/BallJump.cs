

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallJump : MonoBehaviour {

    public float jumpPower = 5f;
    public float lowJumpMultiplier = 1f;

    private Rigidbody rb;
    private bool jump;
    private bool nearObst;
    private bool onGround = true;
    private Vector3 jumpDirection;

    private void Start()// seems this only runs at start of scene
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
       
        jump = Input.GetButton("Jump");
        Jump(); 
        if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
           // Debug.Log(Vector3.down * Physics.gravity.y * lowJumpMultiplier * Time.deltaTime);
            rb.AddForce(Vector3.up *  Physics.gravity.y * lowJumpMultiplier * Time.deltaTime, ForceMode.Impulse); // use vector3.up instead of down because gravity.y is a negative number
        }

    }


    private void Jump()
    {
        LayerMask layer = 1 << gameObject.layer;
        layer = ~layer; // want it to not check layer that is assigned to player object    
        nearObst = Physics.CheckSphere(transform.position, 1f, layer); // need this so players don't jump on each other

		Debug.Log ("Near:"+ nearObst);
		Debug.Log ("Jump:"+ jump);
		Debug.Log ("Onground:"+ onGround);

		if (jump && onGround){
            rb.AddForce(jumpDirection.normalized * jumpPower, ForceMode.Impulse); //CHECK ON NORMALIZED
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // makes sure jump in right direction when landing since OnCollisionSTay is a bit delayed
        onGround = true;
        jumpDirection = Vector3.zero; // resets it
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
		Debug.Log ("anything!");
        onGround = false;
    }

    private void OnCollisionStay(Collision collision) //checks every object colliding with to give more realistic jumping, need just in case players roll into like a corner or is touching many objects
    {
        onGround = true;
        jumpDirection = Vector3.zero; // resets it
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }
}

/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallJump : MonoBehaviour {

	public string typeOfBall;
	private float jumpPower;
	private float lowJumpMultiplier = 1f;

	private Rigidbody rb;
	private bool jump;
	private bool nearObst;
	private bool onGround = true;
	private Vector3 jumpDirection;

	private void Start()// seems this only runs at start of scene
	{
		rb = GetComponent<Rigidbody>();
		switch (typeOfBall) {
		case "Speedy":
			jumpPower = 10;
			break;
		case "Jumpy":
			jumpPower = 17;
			break;
		case "Average":
			jumpPower = 12;
			break;
		case "Turny":
			jumpPower = 10;
			break;
		}
	}

	private void FixedUpdate()
	{

		jump = Input.GetButton("Jump");
		Jump(); 
		if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			// Debug.Log(Vector3.down * Physics.gravity.y * lowJumpMultiplier * Time.deltaTime);
			rb.AddForce(Vector3.up *  Physics.gravity.y * lowJumpMultiplier * Time.deltaTime, ForceMode.Impulse); //allows for small jumps based on length of jump press
		}

	}


	private void Jump()
	{
		LayerMask layer = 1 << gameObject.layer;
		layer = ~layer; // want it to not check layer that is assigned to player object    
		nearObst = Physics.CheckSphere(transform.position, 1f, layer); // need this so players don't jump on each other

		//Debug.Log ("Near:"+ nearObst);
		//Debug.Log ("Jump:"+ jump);
		//Debug.Log ("Onground:"+ onGround);

		if (jump && onGround){
			rb.AddForce(jumpDirection.normalized * jumpPower, ForceMode.Impulse); //CHECK ON NORMALIZED
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		// makes sure jump in right direction when landing since OnCollisionSTay is a bit delayed
		onGround = true;
		jumpDirection = Vector3.zero; // resets it
		foreach (ContactPoint c in collision.contacts)
		{
			jumpDirection += c.normal;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		//Debug.Log ("anything!");
		onGround = false;
	}

	private void OnCollisionStay(Collision collision) //checks every object colliding with to give more realistic jumping, need just in case players roll into like a corner or is touching many objects
	{
		onGround = true;
		jumpDirection = Vector3.zero; // resets it
		foreach (ContactPoint c in collision.contacts)
		{
			jumpDirection += c.normal;
		}
	}
}
*/