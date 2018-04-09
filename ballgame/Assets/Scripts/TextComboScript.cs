using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextComboScript : MonoBehaviour {


    public Text countText;
    public Text winText;
    public Text velocityText;
    
    public Text pointCollect;

    private Rigidbody rb;
    private int count;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        pointCollect.text = "";
        velocityText.text = "Velocity: " + rb.velocity;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.red;
            DisplayPointCollected("+1");
            count = count + 1; //red
            SetCountText();
        }
        if (other.gameObject.CompareTag("Yellow"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.yellow;
            DisplayPointCollected("+2");
            
            count = count + 2;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Green"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.green;
            DisplayPointCollected("+3");
            count = count + 3;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Pink"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.magenta;
            DisplayPointCollected("+4");
            count = count + 4;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Blue"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.blue;
            DisplayPointCollected("+5");
            count = count + 5;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Purple"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.cyan;
            DisplayPointCollected("+10");
            count = count + 10;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Black"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.black;
            DisplayPointCollected("-10");
            count = count - 10;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("WaterTag"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.white;
            DisplayPointCollected("-1");
            count = count - 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("RainbowCube"))
        {
            other.gameObject.SetActive(false);
            pointCollect.color = Color.red;
            DisplayPointCollected("+30");
            count = count + 30;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 176) //1*9 + 2*12 + 3*15 + 4*12 + 5*8 + 10 = 9 + 24 + 45 + 48 + 40 + 10
        {
            winText.text = "You Win!";
        }
    }

    void DisplayPointCollected(string point)
    {

        pointCollect.text = point;
        StartCoroutine("WaitOneSec");

        //StopCoroutine("WaitOneSec");

    }


    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(1);
        pointCollect.text = "";

    }
}
