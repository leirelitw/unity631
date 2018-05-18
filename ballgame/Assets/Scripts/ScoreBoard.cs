using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

    public Text firstPlace;
    public Text secondPlace;
    public Text thirdPlace;
    public Text fourthPlace;
    public Text fifthPlace;
    public Text sixthPlace;

    private int framesToSkip;

    // Use this for initialization
    Dictionary<string, Dictionary<string, int>> playerScores =  new Dictionary<string, Dictionary<string, int>>();
    void Start () {
        // should check for if error occurs
        firstPlace = GameObject.Find("1Place").GetComponent<Text>();
        secondPlace = GameObject.Find("2Place").GetComponent<Text>();
        thirdPlace = GameObject.Find("3Place").GetComponent<Text>();
        fourthPlace = GameObject.Find("4Place").GetComponent<Text>();
        fifthPlace = GameObject.Find("5Place").GetComponent<Text>();
        sixthPlace = GameObject.Find("6Place").GetComponent<Text>();


        firstPlace.text = "1. " + "Just keep";
        secondPlace.text = "2. " + "Rolling";
        thirdPlace.text = "3. " + "Rolling";
        fourthPlace.text = "4. " + "Rolling";
        fifthPlace.text = "5. " + "Rolling";
        sixthPlace.text = "6. " + "Rolling";

        // just for testing. Delete when implemented actual scores
        SetScore("Leire", "points", 321);
        SetScore("Bobby", "points", 31);
        SetScore("Leirdddde", "points", 21);
        SetScore("Leivvvvvre", "points", 3321);

        framesToSkip = 20;// so code in update doesn't always get run causing lag. Possibly reduce number if needed,

    }


    // Update is called once per frame
    void Update () {

        if (framesToSkip > 0)
        {
            framesToSkip--;
        }
        else
        {
            List<KeyValuePair<string, int>> sortedScores = FormatAndSort(playerScores); //is sorted from least to greatest

            sortedScores.Reverse();// is sorted from least to greatest  so want to reverse that
            firstPlace.text = "1. " + sortedScores[0].Key + "   " + sortedScores[0].Value;
            secondPlace.text = "2. " + sortedScores[1].Key + "   " + sortedScores[1].Value;
            thirdPlace.text = "3. " + sortedScores[2].Key + "   " + sortedScores[2].Value;
            fourthPlace.text = "4. " + sortedScores[3].Key + "   " + sortedScores[3].Value;
            fifthPlace.text = "5. " + sortedScores[4].Key + "   " + sortedScores[4].Value;
            sixthPlace.text = "6. " + sortedScores[5].Key + "   " + sortedScores[5].Value;

            framesToSkip = 20;
        }
     
    }




    public void SetScore(string username, string scoreType, int value)
    {

        if (playerScores.ContainsKey(username) == false)
        {
            playerScores[username] = new Dictionary<string, int>();
        }

        playerScores[username][scoreType] = value;
    }




    private List<KeyValuePair<string, int>> FormatAndSort(Dictionary<string, Dictionary<string, int>> playerScores)
    {
        List<KeyValuePair<string, int>> playerAndScore = new List<KeyValuePair<string, int>>();

        // converts dictionary parameter to preffered format of just <string,in> where string is main key of dictionary
        foreach (KeyValuePair<string, Dictionary<string, int>> temp in playerScores)
        {
            Dictionary<string, int> playerValues = temp.Value;
            String playerName = temp.Key;

            foreach (KeyValuePair<string, int> kvp in playerValues)
            {
                int playerScore = kvp.Value;
                playerAndScore.Add(new KeyValuePair<string, int>(playerName, playerScore));

            }

        }

        playerAndScore.Sort(// sorts list by values
            delegate (KeyValuePair<string, int> firstPair,
            KeyValuePair<string, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
        );

        return playerAndScore;
    }

}
