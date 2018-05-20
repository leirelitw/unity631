using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SinglePlayerController_JoyStick : MonoBehaviour
{


    //NOTE TO CHANGE FRICTION SHOULD MESS WITH ANGULAR DRAG IN RIGIDBODY. TEST IT OUT


    public string typeOfBall;
    private float acceleration;
    private float maxSpeed;

    public Camera cameraObj;
    public float speedup = 1f;

    //JoyStick
    public VirtualJoyStick joystick;


    private Rigidbody rigidBody;

    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();

    

        switch (typeOfBall)
        {
            case "Speedy":
                acceleration = 12;
                maxSpeed = 25;
                break;
            case "Jumpy":
                acceleration = 10;
                maxSpeed = 15;
                break;
            case "Average":
                acceleration = 12;
                maxSpeed = 18;
                break;
            case "Turny":
                acceleration = 20;
                maxSpeed = 14;
                break;
        }
        acceleration = acceleration * speedup;
        maxSpeed = maxSpeed * speedup;

    }
    



    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))//Removes/Shows mouse in game when pressed. However could still click things when invisible due to need it for rotation.
        {
            changeCursorVisibility();
        }

        //float moveHorizontal = Input.GetAxis("Horizontal"); // for x axis   a and d keys    -1 and 1
        //float moveVertical = Input.GetAxis("Vertical"); // for z axis  s and w keys   -1 and 1
        moveHorizontal = joystick.Horizontal(); // for x axis   a and d keys    -1 and 1
        moveVertical = joystick.Vertical(); // for z axis  s and w keys   -1 and 1

        //Code below will make it more clear why this format.
        Vector3 verticalDirection = new Vector3(moveVertical, 0.0f, moveVertical);//Since we are rotating camera, want x and z to have values no matter key press.
        Vector3 horizontalDirection = new Vector3(moveHorizontal, 0.0f, moveHorizontal);

        //gets directions of camera passed to script
        Vector3 cameraForwardDirection = cameraObj.transform.forward;
        Vector3 cameraRightDirection = cameraObj.transform.right;//there is no left

        Vector3 forwardDirection = Vector3.Scale(cameraForwardDirection, verticalDirection);// multiplies the two vectors
        Vector3 sidewaysDirection = Vector3.Scale(cameraRightDirection, horizontalDirection);// should be like forwards but if we press d we go sideways

        Vector3 movement = (forwardDirection * acceleration) + (sidewaysDirection * acceleration);// allows moving with wasd normally with camera facing any direction.

        rigidBody.AddForce(movement, ForceMode.Acceleration);//adds force to ball so it can move      
        //accelerationText.text = "Accelration: " + movement; // remove in actual game, used for testing


        //relativeForce doesn't work for ball due to rotation.


        // prevents objectsthis script is attached from going beyond a certain velocity.
        rigidBody.velocity = new Vector3
            (
                Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed),

                rigidBody.velocity.y,

                Mathf.Clamp(rigidBody.velocity.z, -maxSpeed, maxSpeed)
            );
        
    }




    void changeCursorVisibility()
    {
        Cursor.visible = !(Cursor.visible);
    }
}