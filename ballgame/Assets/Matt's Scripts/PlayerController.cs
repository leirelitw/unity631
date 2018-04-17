using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    public Text countText;
    public Text winText;
    public Text velocityText;

    private Rigidbody rb;
    private int count;
  //  private bool jump;
  //  private bool isGrounded;

  //  private bool onGround = true;

     private float maxSpeed = 10f;
   // private float jumpPower = 5f;
   // private Vector3 jumpDirection;

    private void Start()// seems this only runs at start of scene
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        velocityText.text = "Velocity: " + rb.velocity;

    }


    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");// think helps with getting input from 

     //   jump = Input.GetButton("Jump");
      //  Jump(); // maybe move down

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * maxSpeed); //maybe see about doing AddRelativeForce
        rb.velocity = new Vector3
          (
         Mathf.Clamp(rb.velocity.x, -maxSpeed , maxSpeed), //MAYBE due x +z velocity or something. Like if going fast on x, will be going slow accelrating to z
         //OR MAYBE INCREASE BALL MASS SINCE ADD FORCE ACCOUNTS FOR THAT. HOWEVER THAT WOULD STILL MEAN SPEEDY BALL CAN ROTATE REASONABLLY QUICK
         // maybe experiment so speedy balls turny is what also what bouncy has but its max speed is lower
         // then turny ball has a low mass but low maxspeed and jump height. MAYBE CALL TURNY, QUICK ACCELRATION INSTEAD, since turning is changing velocity directy and need accelration for that 
         // plus since be light ball, would be able to get up to its max speed faster

         // SO TRY ABOVE out, also had idea of turny special ability allowing it to imeditly do 90 degree

         rb.velocity.y,

         Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
         );
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f);
        velocityText.text = "Velocity: " + rb.velocity; // see if should just have this in update
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        countText.text = "Counter: " + count.ToString();
        if (count >= 12)
        {
            winText.text = "You Win!";
        }
    }
    /*
    private void Jump()
    {
        LayerMask layer = 1 << gameObject.layer;
        layer = ~layer; // want it to not check layer that is assigned to player object    
        isGrounded = Physics.CheckSphere(transform.position, 1f, layer); // need this so players don't jump on each other
        if (jump && isGrounded && onGround)
        {
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
        onGround = false;
    }

    private void OnCollisionStay(Collision collision) //checks every object colliding with to give more realistic jumping
    {
        onGround = true;
        jumpDirection = Vector3.zero; // resets it
        foreach(ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }
    */
}
