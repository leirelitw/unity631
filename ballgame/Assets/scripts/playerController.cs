using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;
	public Text velocityText;
	private float maxSpeed = 10f;

	private Rigidbody rb;
	private int count;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
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
		if (other.gameObject.CompareTag ( "Pick Up"))
		{
			other.gameObject.SetActive (false);
			count = count + 1; //red
			SetCountText ();
		}
		if (other.gameObject.CompareTag ( "Yellow"))
		{
			other.gameObject.SetActive (false);
			count = count + 2;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "Green"))
		{
			other.gameObject.SetActive (false);
			count = count + 3;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "Pink"))
		{
			other.gameObject.SetActive (false);
			count = count + 4;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "Blue"))
		{
			other.gameObject.SetActive (false);
			count = count + 5;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "Purple"))
		{
			other.gameObject.SetActive (false);
			count = count + 10;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "Black"))
		{
			other.gameObject.SetActive (false);
			count = count - 10;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "WaterTag"))
		{
			other.gameObject.SetActive (false);
			count = count - 1;
			SetCountText ();
		}
		else if (other.gameObject.CompareTag ( "RainbowCube"))
		{
			other.gameObject.SetActive (false);
			count = count + 30;
			SetCountText ();
		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= 176) //1*9 + 2*12 + 3*15 + 4*12 + 5*8 + 10 = 9 + 24 + 45 + 48 + 40 + 10
		{
			winText.text = "You Win!";
		}
	}
}