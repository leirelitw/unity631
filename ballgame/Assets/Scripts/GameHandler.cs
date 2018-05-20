using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameHandler : MonoBehaviour {

    private GameObject main;
    private ConnectionManager con_man;
    private PlayerHandler player_handler;
    

    private GameObject canvas;
    public Text wait_timer;
    public Text game_timer;

    public GameObject player;
    private GameObject player_ball;
    private playerController player_controller;
    private List<GameObject> other_players;
    private AudioSource[] AS;


    long frame_counter = 0;
    
    //these are here because the socket closes up if too many requests are sent without waiting for a response
    //these make sure the client waits for a response before sending another request of its kind
    private Boolean trying_to_join_game = false;
    private Boolean trying_send_coordinates = false;
    private Boolean trying_get_game_timer = false;

    public string map_name;

    private Boolean game_started = false;
    


    void Awake()
    {
        main = GameObject.Find("MainObject");

        wait_timer.text = "";
        game_timer.text = "";
    }

    // Use this for initialization
    void Start () {
        Debug.Log("GameHandler start()");

        con_man = main.GetComponent<ConnectionManager>();
        player_handler = main.GetComponent<PlayerHandler>();
        AS = GetComponents<AudioSource>();

        //gets sphere, the one the client controls
        //player = GameObject.Find("Player");

        player_ball = player.transform.GetChild(0).gameObject;
        player_controller = player_ball.GetComponent<playerController>();

        //gets other players
        other_players = new List<GameObject>();
        while(true)
        {
            GameObject other_player = GameObject.Find("other_player" + (other_players.Count + 1));
            if (other_player != null)
            {
                other_players.Add(other_player);
                //hide them to begin with
                other_player.SetActive(false);
            }
            else
                break;
        }

        canvas = GameObject.Find("Canvas");
        //wait_timer = canvas.transform.Find("StartTimer").gameObject;
        //game_timer = canvas.transform.Find("Timer").gameObject;

    }

    public IEnumerator ResponseJoinGame(Response response)
    {
        Debug.Log("ResponseJoin(): " + response.response);

        string[] parameters = splitParameters(response.response);

        int game_id = Int32.Parse(parameters[0]);

        Debug.Log("Spawn Coordinates: " + parameters[1]);

        //sets player's coordinates since they're spawning
        float[] spawn_coordinates = splitCoordinate(parameters[1]);
        float new_x = spawn_coordinates[0];
        float new_y = spawn_coordinates[1];
        float new_z = spawn_coordinates[2];
        player.transform.position = new Vector3(new_x, new_y, new_z);


        player_handler.setGameID(game_id);

        yield return 0;
    }

    public IEnumerator ResponseTimer(Response response)
    {
        Debug.Log("ResponseTimer(): " + response.response);

        string[] parameters = splitParameters(response.response);

        int wait_time = Int32.Parse(parameters[0]);
        int game_time = Int32.Parse(parameters[1]);

        //while waiting for game to start, player isn't allowed to move
        if (wait_time > 0)
        {
            game_timer.text = "";
            player_controller.allowMovement(false);
            //Text waitTimer = wait_timer.transform.GetComponent<Text>();
            wait_timer.text = wait_time.ToString();
            if (wait_time == 1 && !AS[0].isPlaying)
            {
                AS[0].Play();
            }
        }
        //game has started, and call this ONCE
        else if(game_started==false)
        {
            //game_timer.text = game_time.ToString();
            game_timer.text = convertTime(game_time);
            game_started = true;
            player_controller.allowMovement(true);
        }
        //game has started
        else
        {
            wait_timer.text = "";
            game_timer.text = convertTime(game_time);
            

            //game has ended
            if (game_time == 0)
                gameOver();
        }

        trying_get_game_timer = false;

        yield return 0;
    }

    public String convertTime(int time_left)
    {
        //if there's time left
        if (time_left > 0)
        {
            int minutes = time_left / 60;
            int seconds = time_left % 60;

            if (seconds > 9)
                return "0" + minutes.ToString() + ":" + seconds.ToString();
            else
                return "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        //if there's no time left
        else if (time_left <= 0)
            return "Time's up!";

        return "";
        
    }

    //handles receiving player coordinates
    public IEnumerator ResponseCoordinates(Response response)
    {
        Debug.Log("ResponseCoordinates(): " + response.response);

        //separates coordinates from pickupable ids
        string[] temp =  response.response.Split('&');

        string coordinates_string = temp[0];

        //if server returned pickupable ids, add them to ones picked up since they were picked up by other players
        if (temp.Length >= 2)
        {
            string pickupable_ids = temp[1];

            //removes pickupables
            float[] pickupables = splitCoordinate(pickupable_ids);
            for (int x = 0; x < pickupables.Length; x++)
            {
                add_pickupable_pickup((int)pickupables[x]);
            }
        }


        //Response response;
        float[][] coordinates = splitCoordinatesPlayers(coordinates_string);


        //iterates through other players
        for (int player_index = 0; player_index < coordinates.Length; player_index++)
        {
            //gets other player
            GameObject other_player = other_players[player_index];

            //if other player has been hidden (meaning they are a new player), then unhide them
            if (other_player.activeSelf == false)
                other_player.SetActive(true);

            float x = coordinates[player_index][0];
            float y = coordinates[player_index][1];
            float z = coordinates[player_index][2];
            //float rotate_x = coordinates[player_index][3];
            //float rotate_y = coordinates[player_index][4];
            //float rotate_z = coordinates[player_index][5];



            other_player.transform.position = new Vector3(x, y, z);
            //other_player.transform.eulerAngles = new Vector3(rotate_x, rotate_y, rotate_z);
        }

        trying_send_coordinates = false;


        


        yield return 0;
    }

    //adds pickupable_id to the global list of picked up pickupables
    private void add_pickupable_pickup(int pickupable_id)
    {
        //if hadn't already been picked up
        if (Constants.picked_up_pickupables.Contains(pickupable_id) == false)
            Constants.picked_up_pickupables.Add(pickupable_id);
            
    }
    





    

    // Update is called once per frame
    void Update ()
    {
        //if player isn't part of a game
        if (player_handler.getGameID() == -1)
        {
            //if player hasn't already requested to join a game, send request
            if(trying_to_join_game == false)
                joinGame();
        }
        //if player is part of a game
        else
        {

            

            //every second
            if (frame_counter % 3 == 0)
            {
                //sends player coordinates
                //if (game_started == true && trying_send_coordinates == false)
                if(game_started==true)
                    sendCoordinates();

                //retrieves game timer data every second
                if (frame_counter%24==0)
                    getGameTimer();
            }
        }



        frame_counter++;
    }








    //retrieves game data like amount of seconds left to wait, seconds left in the game, etc. 
    private void getGameTimer()
    {
        trying_get_game_timer = true;

        string url = "/gametimer?session_id=" + player_handler.getSessionID() + "&game_id=" + player_handler.getGameID();
        
        try
        {
            con_man.send(url, Constants.response_gametimer, ResponseTimer);
        }
        catch (Exception ex) { }

        
    }

    
    //sends current player's coordinates to the server
    private void sendCoordinates()
    {
        trying_send_coordinates = true;
        
        float[] player_ball_coor = new float[6];

        float x = (float)Math.Round(player_ball.transform.position.x, 2);
        float y = (float)Math.Round(player_ball.transform.position.y, 2);
        float z = (float)Math.Round(player_ball.transform.position.z, 2);
        //float rotate_x = (float)Math.Round(player_ball.transform.eulerAngles.x, 0);
        //float rotate_y = (float)Math.Round(player_ball.transform.eulerAngles.y, 0);
        //float rotate_z = (float)Math.Round(player_ball.transform.eulerAngles.z, 0);


        //string to_send = x + "," + y + "," + z+ "," + rotate_x+ "," + rotate_y + "," + rotate_z;
        string to_send = x + "," + y + "," + z;

        string url = "/getcoor?s_id=" + player_handler.getSessionID() + "&game_id=" + player_handler.getGameID() + "&coor=" + to_send;


        try {
            con_man.send(url, Constants.response_getcoor, ResponseCoordinates);
        }
        catch (Exception ex) { }
    }

    //joins a game of this map
    private void joinGame()
    {
        trying_to_join_game = true;

        string url = "/joinGame?session_id=" + player_handler.getSessionID() + "&map_name=" + map_name;
        Debug.Log("joinGame(): " + url);
        try
        {
            con_man.send(url, Constants.response_joingame, ResponseJoinGame);
        }
        catch (Exception ex) { }
        
        
    }

    private void gameOver()
    {
       SceneManager.LoadScene("Ranking");
        
    }






    string[] splitParameters(string parameter_string)
    {
        return parameter_string.Split('|');
    }

    //splits many coordinates for many players
    float[][] splitCoordinatesPlayers(string coordinates)
    {
        try
        {
            //splits by players
            string[] players_coordinates = splitParameters(coordinates);

            float[][] to_return = new float[players_coordinates.Length][];
            for (int i = 0; i < players_coordinates.Length; i++)
            {
                to_return[i] = splitCoordinate(players_coordinates[i]);
            }

            return to_return;
        }
        catch (Exception ex)
        {
            return new float[0][];
        }
    }

    //splits up a single coordinate object by x,y,z,rotate_x,rotate_y,rotate_z
    float[] splitCoordinate(string coordinate)
    {
        //splits coordinates
        string[] str_coor = coordinate.Split(',');

        float[] to_return = new float[6];
        for (int j = 0; j < str_coor.Length; j++)
        {
            if (str_coor[j].Equals("") == false)
                to_return[j] = float.Parse(str_coor[j]);
            else
                to_return[j] = 0;
        }

        return to_return;
    }
}
