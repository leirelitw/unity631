import java.util.*;
import java.util.concurrent.TimeUnit;




public class Game {

    //Coordinate object for tracking coordinates
    public class Coordinate{
        public float x;
        public float y;
        public float z;
        public float rotate_x;
        public float rotate_y;
        public float rotate_z;

        public Coordinate(){
            x = 0;
            y = 0;
            z = 0;
            rotate_x = 0;
            rotate_y = 0;
            rotate_z = 0;
        }

        public Coordinate(float x, float y, float z, float rotate_x, float rotate_y, float rotate_z){
            setCoordinate(x, y, z, rotate_x, rotate_y, rotate_z);
        }

        public void setCoordinate(float new_x, float new_y, float new_z, float new_rotate_x, float new_rotate_y, float new_rotate_z){
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.rotate_x = new_rotate_x;
            this.rotate_y = new_rotate_y;
            this.rotate_z = new_rotate_z;
        }

        public String toString()
        {
            return Float.toString(x)+","+Float.toString(y)+","+Float.toString(z)+","+Float.toString(rotate_x)
                    +","+Float.toString(rotate_y)+","+Float.toString(rotate_z);
        }
    }




    private int game_id;

    public String map_name;         //tracks currently playing map



    //private Player[] players = new Player[max_num_players];
    private ArrayList<Player> players;
    private ArrayList<Coordinate> player_coordinates;
    private ArrayList<Integer> picked_up_pickupables;       //keeps track of the objects that have been collected by storing session_ids
    private HashMap<String, ArrayList<Integer>> players_to_remind;   //stores arrays of pickupable_ids that need to be sent to session_ids as the keys



    //number of players currently playing in the game
    private int num_players = 0;
    private final int min_num_players = 2;  //minimum number of players for a game to start
    private final int max_num_players = 6;  //maximum number of players in a game
    private final int game_length = 300;    //length of a game in seconds
    private final int time_wait = 10;       //length of waiting for game to start in seconds


    //number of seconds left in play
    private int seconds_left = game_length;
    //number of seconds to wait for game to start
    private int time_to_wait = time_wait;



    //the names of all available maps
    public String[] map_names = {"beachScene", "ChocolateScene", "CityScene", "forestScene", "HalloweenScene", "legoScene", "RacingScene",  "WinterScene"};
    public int[] num_collectables = {10, 10, 10, 10, 10, 10, 10, 10}; //number of collectables in all available maps. Corresponds to map_names
    //stores spawn coordinates for all maps so that when player first joins, they can update their coordinates
    public Coordinate[][] spawn_coordinates;





    //true if game has started
    private boolean started = false;
    //true if game has ended
    private boolean finished = false;



    //Threads
    private Thread waitThread;
    private Thread runThread;









    public Game(int game_id)
    {
        this.game_id = game_id;

        initializeSpawnCoordinates();


        players = new ArrayList<Player>();
        picked_up_pickupables = new ArrayList<Integer>();
        player_coordinates = new ArrayList<Coordinate>();
        players_to_remind = new HashMap<String, ArrayList<Integer>>();
    }

    //starts the wait for the game to start
    public void startWait()
    {
        System.out.println("startWait()");
        waitThread = new Thread() {

            public void run() {


                try {



                    while (time_to_wait > 0) {


                        //waits for 2 players to be in game before it starts count down
                        while(num_players<min_num_players){
                            //System.out.println("Waiting for enough players to join");

                            //resets timer
                            time_to_wait = time_wait;

                            TimeUnit.SECONDS.sleep(1);

                        }

                        time_to_wait--;
                        TimeUnit.SECONDS.sleep(1);
                    }

                    //starts game after 3 seconds to give the client time to load
                    TimeUnit.SECONDS.sleep(3);

                } catch (Exception ex) {
                    System.out.println("Timeout ERROR: " + ex.toString());
                }



                startGame();
            }

        };

        waitThread.start();
    }

    //runs the game in its own thread
    public void startGame()
    {
        runThread = new Thread() {

            public void run() {
            started = true;

            try {
                while (seconds_left > 0) {

//                    System.out.println("Game: "+game_id);
//                    System.out.println("Num players: "+num_players);
//                    System.out.println();



//                    //prints players coordinates
//                    for(int x = 0; x < num_players; x++)
//                    {
//                        System.out.println(player_coordinates.get(x).toString());
//                    }


                    seconds_left--;
                    TimeUnit.SECONDS.sleep(1);

                }


                //compile winners
                compile_winners();

                //waits 5 seconds before resettings the game
                TimeUnit.SECONDS.sleep(5);


                //game has ended
                finished = true;

            } catch (Exception ex) {
                System.out.println("Timeout ERROR: " + ex.toString());
            }


            }

        };

        runThread.start();


    }

    //updates coordinates of player specified by session_id
    public void updatePlayerCoordinates(String session_id, String coordinate)
    {
        //gets the player associated with the session_id
        int player_index = getPlayerIndex(session_id);

        //invalid session_id, so stop
        if(player_index==-1)
            return;

        Coordinate coor = player_coordinates.get(player_index);

        //in a try catch because don't trust user input data
        try {
            //gets array of the
            String[] split_str = coordinate.split(",");
            float x = Float.parseFloat(split_str[0]);
            float y = Float.parseFloat(split_str[1]);
            float z = Float.parseFloat(split_str[2]);
            float rotate_x = Float.parseFloat(split_str[3]);
            float rotate_y = Float.parseFloat(split_str[4]);
            float rotate_z = Float.parseFloat(split_str[5]);

            coor.setCoordinate(x, y, z, rotate_x, rotate_y, rotate_z);
        } catch(Exception ex){

        }
    }

    //returns all player coordinates except for specified player
    public String getPlayerCoordinatesExcept(String session_id)
    {
        int cur_player_index = getPlayerIndex(session_id);

        String to_return = "";
        String delimiter = "|";
        for(int x = 0; x < player_coordinates.size(); x++)
        {
            //skips specified player
            if(x==cur_player_index)
                continue;

            //gets string version of the coordinates
            String str_coor = player_coordinates.get(x).toString();

            to_return += str_coor + delimiter;
        }

        //removes last delimiter
        if(to_return.length() > 0)
            to_return = to_return.substring(0, to_return.length() - 1);

        return to_return;
    }

    //returns all player coordinates for every player
    //FOR TESTING
    public String getAllPlayerCoordinates()
    {
        String to_return = "";
        String delimiter = "|";
        for(int x = 0; x < player_coordinates.size(); x++)
        {
            //gets string version of the coordinates
            String str_coor = player_coordinates.get(x).toString();


            //if at last index, don't add delimiter
            if(x==player_coordinates.size()-1)
                delimiter = "";

            to_return += str_coor + delimiter;
        }

        return to_return;
    }

    //returns the player's spawn coordinates in string format
    public String getSpawnCoordinates(String session_id)
    {
        int cur_player_index = getPlayerIndex(session_id);

        //gets map index
        int map_index = -1;
        for(int x = 0; x < map_names.length; x++)
        {
            if(map_name.equals(map_names[x])) {
                map_index = x;
                break;
            }

        }

        //couldn't find map, so stop
        if(map_index==-1)
            return "";

        //gets string version of the coordinates
        System.out.println(spawn_coordinates[map_index][cur_player_index]);
        String str_coor = spawn_coordinates[map_index][cur_player_index].toString();

        return str_coor;
    }


    public void addPickupable(String session_id, int pickupable_id)
    {
        picked_up_pickupables.add(pickupable_id);

        //adds session_ids to update
        for(int x = 0; x < num_players; x++)
        {
            String cur_sess_id = players.get(x).getSessionID();

            //skip if is current player
            if(cur_sess_id == session_id)
                continue;


            //if player doesn't already have items to be told about, create new list
            if(players_to_remind.get(cur_sess_id) == null) {
                players_to_remind.put(cur_sess_id, new ArrayList<Integer>());
            }


            players_to_remind.get(cur_sess_id).add(pickupable_id);
        }
    }

    //returns integers of pickupable ids that have been picked up since specified player has last checked
    public ArrayList<Integer> getNewPickupablesForPlayer(String session_id)
    {
        return players_to_remind.get(session_id);
    }

    public void removeNewPickupablesForPlayer(String session_id)
    {
        players_to_remind.remove(session_id);
    }



    //returns whether or not player was added successfully
    public boolean addPlayer(Player player)
    {
        //if there's still room
        if(num_players < max_num_players)
        {
            //checks if player is already in the game
            boolean player_already_exists = false;
            for (int x = 0; x < num_players; x++) {
                if (players.get(x).getSessionID() == player.getSessionID())
                    player_already_exists = true;
            }

            //if player already isn't in game, add them
            if (player_already_exists == false) {
                players.add(player);
                player_coordinates.add(new Coordinate());
                num_players++;
            } else
                return false;
        }
        else
            return false;

        return true;
    }

    //remove player from the game
    public void removePlayer(String session_id)
    {
        for(int x = 0; x < players.size(); x++)
        {
            if(players.get(x).getSessionID().equals(session_id)) {
                players.remove(x);
                num_players--;
                break;
            }
        }
    }

    //compile scores of top players
    public void compile_winners()
    {

    }


    //returns true if player is in the game if given a string session_id
    public boolean isPlayerInGame(String session_id)
    {
        //if a valid player object was found with the session_id, then player is part of the game
        if(getPlayerIndex(session_id) != -1)
            return true;
        else
            return false;
    }




    //returns array of all players in the game
    public ArrayList<Player> getPlayers()
    {
        return this.players;
    }

    public int getGameID()
    {
        return this.game_id;
    }

    public int getNumPlayers() { return this.num_players; }

    public int getMaxPlayers() { return this.max_num_players; }

    public int getSecondsLeft() { return this.seconds_left; }

    public int getWaitLeft() { return this.time_to_wait; }

    public boolean hasGameStarted() { return this.started; }

    public boolean hasFinished() { return this.finished; }



    //keeps all players in the game, just changes the map
    public void restartGame()
    {
        time_to_wait = time_wait;
        seconds_left = game_length;

        //generates random map that isn't current map
        int map_index = (int)Math.floor(Math.random() * map_names.length);
        map_name = map_names[map_index];


        //starts wait again
        //startWait();
    }

    //returns the index of the player in the players global variable
    public int getPlayerIndex(String session_id)
    {
        for(int x = 0; x < players.size(); x++)
        {
            if(players.get(x).getSessionID().equals(session_id))
                return x;

        }
        return -1;
    }


    private void initializeSpawnCoordinates()
    {
        int[][] start_coordinates = new int[map_names.length][3];
        //beachScene
        start_coordinates[0] = new int[]{712, 60, 703};
        //ChocolateScene
        start_coordinates[1] = new int[]{712, 80, 703};
        //CityScene
        start_coordinates[2] = new int[]{297, 50, 179};
        //forestScene
        start_coordinates[3] = new int[]{443, 60, 967};
        //HalloweenScene
        start_coordinates[4] = new int[]{778, 60, 465};
        //legoScene
        start_coordinates[5] = new int[]{843, 50, 787};
        //RacingScene
        start_coordinates[6] = new int[]{-375, 100, -430};
        //WinterScene
        start_coordinates[7] = new int[]{355, 100, 58};

        //initializes initial spawning positions
        spawn_coordinates = new Coordinate[map_names.length][max_num_players];



        //map 1 spawn coordinate initialization

        int x = 0;
        int y = 0;
        int z = 0;
        for(int i = 0; i < map_names.length; i++)
        {
            Coordinate[] map = new Coordinate[max_num_players];
            x = start_coordinates[i][0];
            y = start_coordinates[i][1];
            z = start_coordinates[i][2];
            for(int j = 0; j < max_num_players; j++) {
                map[j] = new Coordinate(x, y, z, 0, 0, 0);
                x += 10;
            }
            System.out.println("Added spawn coordinates for map"+i);
            spawn_coordinates[i] = map;
        }

    }


    public String toString()
    {
        String to_print = "-- Game --" + System.lineSeparator();
        to_print += "Game_id: "+game_id + System.lineSeparator();
        to_print += "Current map: "+map_name + System.lineSeparator();
        to_print += "Time to wait: "+ time_to_wait +  System.lineSeparator();
        to_print += "Seconds left: "+ seconds_left + System.lineSeparator();


        to_print += "Players: " + System.lineSeparator();
        for(int x = 0; x < players.size(); x++)
        {
            String session_id = players.get(x).getSessionID();
            String coordinates = player_coordinates.get(x).toString();
            to_print += "  "+(x+1)+": " + session_id + ";"+coordinates + System.lineSeparator();
        }

        to_print += "-- End Game --";

        return to_print;
    }
}
