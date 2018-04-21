using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MattPlayerControllerScript : MonoBehaviour
{

    public Text countText;
    public Text winText;
    public Text velocityText;
    public Text accelerationText;
    public string typeOfBall;
    public Camera camera;

    private float acceleration;
    private float maxSpeed;
    private AudioSource[] AS;
    private int previousCount = 0;

    private Rigidbody rigidBody;
    private int count;

    // put missing object checking

    void Start()
    {
        //Cursor.visible = false; allows removing cursor, though not implemented yet
        rigidBody = GetComponent<Rigidbody>();
        AS = GetComponents<AudioSource>();


        count = 0;
        SetCountText();
        winText.text = "";
        velocityText.text = "Velocity: " + rigidBody.velocity;

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

    }

    private void FixedUpdate()
    {
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
        velocityText.text = "Velocity: " + rigidBody.velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1; //red
            SetCountText();
        }
        if (other.gameObject.CompareTag("Yellow"))
        {
            other.gameObject.SetActive(false);
            count = count + 2;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Green"))
        {
            other.gameObject.SetActive(false);
            count = count + 3;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Pink"))
        {
            other.gameObject.SetActive(false);
            count = count + 4;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Blue"))
        {
            other.gameObject.SetActive(false);
            count = count + 5;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Purple"))
        {
            other.gameObject.SetActive(false);
            count = count + 10;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Black"))
        {
            other.gameObject.SetActive(false);
            count = count - 10;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("WaterTag"))
        {
            other.gameObject.SetActive(false);
            count = count - 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("RainbowCube"))
        {
            other.gameObject.SetActive(false);
            count = count + 30;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count - previousCount >= 10)
        {
            AudioSource Pick10 = AS[6];
            Pick10.Play();
        }
        else if (count - previousCount >= 5)
        {
            AudioSource Pick5 = AS[5];
            Pick5.Play();
        }
        else if (count - previousCount >= 5)
        {
            AudioSource Pick4 = AS[4];
            Pick4.Play();
        }
        else if (count - previousCount >= 3)
        {
            AudioSource Pick3 = AS[3];
            Pick3.Play();
        }
        else if (count - previousCount >= 2)
        {
            AudioSource Pick2 = AS[2];
            Pick2.Play();
        }
        else if (count - previousCount >= 1)
        {
            AudioSource Pick1 = AS[1];
            Pick1.Play();
        }
        else if (count - previousCount <= -1)
        {
            AudioSource PickBad = AS[0];
            PickBad.Play();
        }
        if (count >= 176) //1*9 + 2*12 + 3*15 + 4*12 + 5*8 + 10 = 9 + 24 + 45 + 48 + 40 + 10
        {
            AudioSource PickWin = AS[7];
            PickWin.Play();
            winText.text = "You Win!";
        }
        previousCount = count;
    }

    void changeCursorVisibility()
    {
        Cursor.visible = !(Cursor.visible);
    }
}