using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{


    private GameObject main;
    private ConnectionManager con_man;
    private PlayerHandler player_handler;
    private PickupableHandler pickupable_handler;

    //on-screen objects
    public Text countText;
    public Text winText;
    public Text pointCollect;

    private AudioSource[] AS;

    private Rigidbody rb;
    private int count;

    // Use this for initialization
    void Start()
    {
        // should check for if error occurs
        countText = GameObject.Find("countText").GetComponent<Text>();
        winText = GameObject.Find("winText").GetComponent<Text>();
        pointCollect = GameObject.Find("pointCounterText").GetComponent<Text>();


        main = GameObject.Find("MainObject");
        con_man = main.GetComponent<ConnectionManager>();
        player_handler = main.GetComponent<PlayerHandler>();


        rb = GetComponent<Rigidbody>();
        count = 0;
        //SetCountText();
        countText.text = "Count: " + count.ToString();
        winText.text = "";
        pointCollect.text = "";
        AS = GetComponents<AudioSource>();

    }


    //Note reason this is run when it looks like your diving into water is because the ocean collider is small. Could fix by making it larger.
    private void OnTriggerExit(Collider other)
    {
        // for when user enters water
        if (other.gameObject.CompareTag("WaterTag")) // don't want to delete ocean so thats why its here plus want it on exit.
        {
            pointCollect.color = Color.white;
            DisplayPointCollected("-1");
            count = count - 1;
            AudioSource PickBad = AS[0];
            PickBad.Play();
            countText.text = "Count: " + count.ToString();
        }
        countText.text = "Count: " + count.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        //gets access to pickupablehandler connected to object player collided with
        pickupable_handler = obj.GetComponent<PickupableHandler>();


        //item tags colliding with
        string[] tags = new string[] { "Red", "Yellow", "Green", "Pink", "Blue", "Purple", "Black", "Zombie", "RainbowCube" };
        //colors of the text popup
        Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.magenta, Color.blue, Color.cyan, Color.black, Color.black, Color.white, Color.red };
        //points to add
        int[] points = new int[] { 1, 2, 3, 4, 5, 10, -10, -10, -1, 30 };
        //Audio sources to play
        AudioSource[] audio = new AudioSource[] { AS[1], AS[2], AS[3], AS[4], AS[5], AS[6], AS[0], AS[0], AS[0], AS[7] };


        for (int x = 0; x < tags.Length; x++)
        {
            if (other.gameObject.CompareTag(tags[x]))
            {
                other.gameObject.SetActive(false);
                pointCollect.color = colors[x];

                if (points[x] > 0)
                    DisplayPointCollected("+" + points[x]);
                else
                    DisplayPointCollected("" + points[x]);
                count = count + points[x];

                AudioSource Pick1 = audio[x];
                Pick1.Play();
                // SetCountText();

                //sends pickupable to server
                string url = "/pickedup?session_id=" + player_handler.getSessionID() + "&game_id=" + player_handler.getGameID() + "&pickupable_id=" + pickupable_handler.pickupable_id;
                Debug.Log(url);

                try
                {
                    con_man.send(url, Constants.response_pickedup, ResponsePickedup);
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception: " + ex.ToString());
                }
            }
        }

        countText.text = "Count: " + count.ToString();
    }


    //handles receiving player coordinates
    public IEnumerator ResponsePickedup(Response response)
    {
        Debug.Log("ResponsePickedup(): " + response.response);




        yield return 0;
    }

    void DisplayPointCollected(string point)
    {
        pointCollect.text = point;
        StartCoroutine("WaitOneSec");
    }


    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(1);
        pointCollect.text = "";

    }

}
