using UnityEngine;
using System.Collections;

public class CamRotation2 : MonoBehaviour {

	public float turnSpeed = 2.0f;
	public Transform player;
    public float cameraCenterOffset= 5f;

	private Vector3 offset;
    private Vector3 viewCenterOffset;

	void Start () {
		//offset = new Vector3(player.position.x, player.position.y, player.position.z);
		offset = transform.position - player.transform.position;
        viewCenterOffset = new Vector3(0, cameraCenterOffset, 0);

    }

	void LateUpdate()
	{
		offset = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
		transform.position = player.position + offset;

        //transform.LookAt(player.position); this will keep the play in the center of the view
        
        transform.LookAt(player.position + viewCenterOffset);

    }
}