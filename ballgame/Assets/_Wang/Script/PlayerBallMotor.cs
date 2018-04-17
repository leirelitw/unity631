using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBallMotor : MonoBehaviour {


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 thrusterForce = Vector3.zero; //for jump

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    //Run every physics iteratioin
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Get a force vector for our thrusters, for jump
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    //Gets a rotation vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    void PerformRotation()
    {
        
        rb.MoveRotation(rb.rotation *Quaternion.Euler(rotation));
        
    }

    
}
