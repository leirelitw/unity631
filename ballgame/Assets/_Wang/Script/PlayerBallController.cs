using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(PlayerBallMotor))]
public class PlayerBallController : MonoBehaviour {

    public float speed = 5f;
    public float lookSensitivity = 3f;

    public float thrusterForce = 1000f; //for jump

    private PlayerBallMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerBallMotor>();
    }

    private void Update()
    {
        //calculate movment velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);

        //calculate rotation as a 3D vector(turning)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //calculate thruster force, for jump
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
        }

        //apply the thruster force, for jump
        motor.ApplyThruster(_thrusterForce);

    }


}
