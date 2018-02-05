using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;
	private int count;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
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