using UnityEngine;
using System.Collections;

public class CamRotation : MonoBehaviour {

	public float turnSpeed = 2.0f;
	public Transform player;

	private Vector3 offset;

	void Start () {
		//offset = new Vector3(player.position.x, player.position.y, player.position.z);
		offset = transform.position - player.transform.position;
	}

	void LateUpdate()
	{
		offset = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
		transform.position = player.position + offset; 
		transform.LookAt(player.position);
	}
}