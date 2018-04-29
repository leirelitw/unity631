

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallJump : MonoBehaviour {

    public float jumpPower = 5f;
    public float lowJumpMultiplier = 1f;

    private Rigidbody rigidBody;
    private bool jump;
    private bool nearObst;
    private Vector3 jumpDirection;
    //public float jumpHigh = 20f;

    private void Start()// seems this only runs at start of scene
    {
        rigidBody = GetComponent<Rigidbody>();
        jumpDirection = Vector3.up;
    }

    private void FixedUpdate()
    {

        jump = Input.GetButton("Jump");
        if (jump)
        {
            Jump();
        }
        if(rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))// allows for low jumps
        {
            rigidBody.AddForce(Vector3.up *  Physics.gravity.y * lowJumpMultiplier * Time.deltaTime, ForceMode.Impulse); // use vector3.up instead of down because gravity.y is a negative number
        }

    }


    private void Jump()
    {
        // LayerMask layer = 1 << gameObject.layer;
        //  layer = ~layer; // want it to not check layer that is assigned to player object    
        //  nearObst = Physics.CheckSphere(transform.position, 1f, layer); // need this so players don't jump on each other
        // not even using nearObst  maybe implement


        if (Physics.Raycast(transform.position, -Vector3.up, 3) && jump)// && onGround onGround causes it to not jump as well. Like on non terrain objects
        {
            rigidBody.AddForce(jumpDirection.normalized * jumpPower, ForceMode.Impulse);
            // rigidBody.velocity = new Vector3(rb.velocity.x, jumpHigh, rb.velocity.z);   // PoHung's version
        }
    }


    //Below code allows for jumping base on angle of surface jumped on. So jump off at 45 degree surface, go at a 45 degree angel
    /*
    private void OnCollisionEnter(Collision collision)
    {
        // makes sure jump in right direction when landing since OnCollisionSTay is a bit delayed
        jumpDirection = Vector3.zero; // resets it
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnCollisionStay(Collision collision) //checks every object colliding with to give more realistic jumping, need just in case players roll into like a corner or is touching many objects
    {
        jumpDirection = Vector3.zero; // resets it
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }
    */
}
