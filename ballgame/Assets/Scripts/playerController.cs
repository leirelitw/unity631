using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class playerController : MonoBehaviour
{
    // public Text countText;
    // public Text winText;
    public Text velocityText;
    public Text accelerationText;
    public string typeOfBall;

    public Camera camera;

    Scene currentScene;
    private float acceleration;
    private float maxSpeed;
    private AudioSource[] AS;
    private int previousCount = 0;
    private string sceneName;

    private Rigidbody rigidBody;
    private int count;

    private bool allowed_movement = true;

    public float jumpPower = 5f;// need to change value in BallJump class


    private void Awake()
    {
        // Set up the reference.

    }
    void Start()
    {
        //Cursor.visible = false; allows removing cursor, though not implemented yet
        rigidBody = GetComponent<Rigidbody>();
        AS = GetComponents<AudioSource>();
        
        //define which scene we are playing with.
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        count = 0;
        //SetCountText();
        // winText.text = "";
        velocityText.text = "Velocity: " + rigidBody.velocity;

        switch (typeOfBall)
        {
            case "Speedy":
                acceleration = 25;
                jumpPower = 10;
                maxSpeed = 25;
                break;
            case "Jumpy":
                acceleration = 10;
                jumpPower = 10;
                maxSpeed = 15;
                break;
            case "Average":
                acceleration = 12;
                jumpPower = 10;
                maxSpeed = 18;
                break;
            case "Turny":
                acceleration = 20;
                jumpPower = 10;
                maxSpeed = 14;
                break;
        }
    }

    private void FixedUpdate()
    {
        //if player isn't allowed to move, return without doing anything
        if (allowed_movement == false)
            return;

        if (Input.GetKeyDown(KeyCode.P))//Removes/Shows mouse in game when pressed. However could still click things when invisible due to need it for rotation.
        {
            changeCursorVisibility();
        }

        float moveHorizontal = Input.GetAxis("Horizontal"); // for x axis   a and d keys    -1 and 1
        float moveVertical = Input.GetAxis("Vertical"); // for z axis  s and w keys   -1 and 1

        //Code below will make it more clear why this format.
        Vector3 verticalDirection = new Vector3(moveVertical, 0.0f, moveVertical);//Since we are rotating camera, want x and z to have values no matter key press.
        Vector3 horizontalDirection = new Vector3(moveHorizontal, 0.0f, moveHorizontal);

        //gets directions of camera passed to script
        Vector3 cameraForwardDirection = camera.transform.forward;
        Vector3 cameraRightDirection = camera.transform.right;//there is no left

        Vector3 forwardDirection = Vector3.Scale(cameraForwardDirection, verticalDirection);// multiplies the two vectors
        Vector3 sidewaysDirection = Vector3.Scale(cameraRightDirection, horizontalDirection);// should be like forwards but if we press d we go sideways

        Vector3 movement = (forwardDirection * acceleration) + (sidewaysDirection * acceleration);// allows moving with wasd normally with camera facing any direction.

        rigidBody.AddForce(movement, ForceMode.Acceleration);//adds force to ball so it can move      
        accelerationText.text = "Accelration: " + movement; // remove in actual game, used for testing


        //relativeForce doesn't work for ball due to rotation.


        // prevents objectsthis script is attached from going beyond a certain velocity.
        rigidBody.velocity = new Vector3
            (
                Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed),

                rigidBody.velocity.y,

                Mathf.Clamp(rigidBody.velocity.z, -maxSpeed, maxSpeed)
            );
        velocityText.text = "Velocity: " + rigidBody.velocity.magnitude;
        if (rigidBody.velocity.magnitude >= 1 && rigidBody.velocity.y == 0)
        {
            switch (sceneName)
            {
                case "ForestScene":
                    if (!AS[8].isPlaying)
                    {
                        AS[8].Play();
                    }
                    break;
                case "WinterScene":
                    if (!AS[9].isPlaying)
                    {
                        AS[9].Play();
                    }
                    break;
                case "BeachScene":
                    if (!AS[10].isPlaying)
                    {
                        AS[10].Play();
                    }
                    break;
            }
        }
    }


    //sets whether player is allowed to move
    public void allowMovement(bool movement)
    {
        allowed_movement = movement;
    }

    void changeCursorVisibility()
    {
        Cursor.visible = !(Cursor.visible);
    }
}