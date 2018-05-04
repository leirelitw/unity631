import java.util.*;
import java.util.concurrent.TimeUnit;


public class GameServer {

    public ArrayList<Player> players;
    public ArrayList<Game> games;

    //tracks the last time a player has requested anything in epoch
    //private ArrayList<Long> player_last_request = new ArrayList<Long>();
    private HashMap<String, Long> player_last_request = new HashMap<String, Long>();

    //keeps track of game_ids
    private int game_counter = 0;

    String param_delimiter = "|";

    MySQLHandler sql_handler;
    Thread gameThread;

    public GameServer()
    {
        players = new ArrayList<Player>();
        games = new ArrayList<Game>();

        sql_handler = new MySQLHandler();
        System.out.println("Initialized SQL handler");


        manageGames();

    }

    //receives request in form of uri, and returns response in form of string
    public String processRequest(String resourceString)
    {
        String endpoint;
        String queryString = null;
        String endpointParts[] = resourceString.split("\\?");

        //separates endpoint from parameters
        endpoint = endpointParts[0];
        if (endpointParts.length == 2) {
            queryString = endpointParts[1];
        }

        //System.out.println("Endpoint: "+endpoint);
        //System.out.println("queryString: "+queryString);


        //response to return to client
        String to_return = "";

        //separates parameters
        String[] parameters = new String[0];
        String[] values = new String[0];
        try {
            try
            {
                parameters = queryString.split("&");
                values = new String[parameters.length];
                for (int i = 0; i < parameters.length; i++) {
                    String[] argSplit = parameters[i].split("=");
                    values[i] = argSplit[1];
                }
            } catch(Exception ex){}

            //System.out.println(endpoint.toLowerCase());



            //processes the request for different endpoints
            switch(endpoint.toLowerCase()){
                case "/login":
                    //[0] = username
                    //[1] = password
                    to_return = "101"+login(values[0], values[1]);
                    break;
                case "/register":
                    //[0] = username
                    //[1] = email
                    //[2] = password
                    to_return = "102"+register(values[0], values[1], values[2]);
                    break;
//                case "/newgame":
//                    to_return = newGame();
//                    break;
                case "/getgames":
                    to_return = "103"+getGames();
                    break;
                case "/joingame":
                    //[0] = session_id
                    //[1] = map name
                    to_return = "104"+joinGame(values[0], values[1]);
                    break;
                case "/gametimer":
                    //[0] = session_id
                    //[1] = game_id
                    to_return = "105"+getGameTimer(values[0], Integer.parseInt(values[1]));
                    break;
                case "/getcoor":
                    //[0] = session_id
                    //[1] = game_id
                    //[2] = player's current coordinates as string
                    to_return = "106"+getCoordinates(values[0], Integer.parseInt(values[1]), values[2]);
                default:
                    break;
            }
        } catch(Exception ex){
            System.out.println("Error: "+ex.toString());
        }

        //returns newline if error so that client doesn't freeze
        if(to_return.equals(""))
            to_return="\n";

        return to_return;
    }

    public void manageGames()
    {
        //creates new thread to manage games so that it doesn't interfere with requests
        gameThread = new Thread() {

            public void run() {

                //loadGames();

                int seconds = 0;
                while (true) {

                    //System.out.println("Manage Games second: " + seconds);


                    //prints out currently running games
                    if(games.size()>0) {
                        System.out.println("-- Current games --");
                        int counter = 0;
                        while(counter < games.size())
                        {
                            Game game = games.get(counter);
                            System.out.println("game "+game.getGameID()+": ["+game.map_name+"] ["+game.getNumPlayers()+"] ["+game.getWaitLeft()+"|"+game.getSecondsLeft()+"]");

                            //removes game if no one is playing
                            if(game.getNumPlayers()==0) {
                                games.remove(counter);
                                continue;
                            }
                            else
                                counter++;

                            //if game finished, then restart it with new map
                            if(game.hasFinished())
                                game.restartGame();
                        }
                    }


//                    if(games.size()>0) {
//                        System.out.println("## Current Games ##");
//
//                        //prints the current games
//                        for (int x = 0; x < games.size(); x++)
//                            System.out.println(games.get(x).toString());
//                    }




                    //handles players and disconnects ones that haven't sent request for a while
                    //checks every minute
                    if(seconds%60==0) {
                        long cur_milliseconds = System.currentTimeMillis();
                        int counter = 0;
                        while (counter < players.size()) {
                            String session_id = players.get(counter).getSessionID();

                            double seconds_passed = (cur_milliseconds - player_last_request.get(session_id)) / 1000;

                            //disconnect after 1 day
                            if (seconds_passed >= 86400)
                                removePlayer(players.get(counter).getSessionID());
                            else
                                counter++;
                        }
                    }

                    seconds++;

                    //System.out.println();
                    try {
                        TimeUnit.SECONDS.sleep(1);
                    } catch (Exception ex) {
                        System.out.println("Timeout ERROR: " + ex.toString());
                    }
                }
            }

        };

        gameThread.start();
    }

//    //loads current games from sql database
//    public void loadGames()
//    {
//
//    }


    //returns coordinates of all players in a game, and sends current player's coordinates
    public String getCoordinates(String session_id, int game_id, String my_coordinates)
    {
        updatePlayerRequestTime(session_id);

        //retrieves game associated with game_id
        Game game = getGameById(game_id);

        //invalid game id
        if(game==null)
            return "Invalid game ID";

        //if player is not in the game, then stop
        if(game.isPlayerInGame(session_id) == false)
            return "Player not in game";
        //if gotten here, then player is in the game
        //update player's coordinates and returns the coordinates of all other players

        game.updatePlayerCoordinates(session_id, my_coordinates);

        //gets all coordinates in string format except for the current player
        return game.getPlayerCoordinatesExcept(session_id);
    }

    //returns the amount of time left to wait and amount of time left in game
    public String getGameTimer(String session_id, int game_id)
    {
        //update last request time for current player
        updatePlayerRequestTime(session_id);

        Game game = getGameById(game_id);

        String to_return = "";
        to_return += game.getWaitLeft() + param_delimiter;
        to_return += game.getSecondsLeft();

        return to_return;
    }

    //joins a game of map_name
    public String joinGame(String session_id, String map_name)
    {
        System.out.println("joinGame()");

        //update last request time for current player
        updatePlayerRequestTime(session_id);


        //gets player associated with provided session_id
        Player player = getPlayer(session_id);

        //if no player exists with session_id, don't join game
        if(player==null)
            return "";

        System.out.println(session_id+" is joining "+map_name);

        int game_index = -1;
        boolean found_game = false;
        for(int x = 0; x < games.size(); x++)
        {
            Game game = games.get(x);

            //if map name matches, and game hasn't started, and the game isn't full, then join
            if(game.map_name.equals(map_name) && game.hasGameStarted()==false && game.getNumPlayers()<game.getMaxPlayers())
            {
                game_index = x;
                found_game = true;
                break;
            }
        }


        Game game;

        //if found a game to join, then join it
        if(found_game)
        {
            System.out.println("Game found");
            game = games.get(game_index);
            game.addPlayer(player);
        }
        //if no found game, then create it
        else
        {
            System.out.println("Game not found, so create one");
            game = new Game(game_counter);
            game_counter++;


            game.map_name = map_name;
            game.addPlayer(player);
            game.startWait();

            games.add(game);
        }

        String to_return = Integer.toString(game.getGameID());

        to_return += param_delimiter;

        System.out.println("to_return: "+to_return);
        System.out.println("session_id: "+player.getSessionID());

        //returns new spawn coordinates
        to_return += game.getSpawnCoordinates(player.getSessionID());

        System.out.println("joinGame() returning: "+to_return);

        return to_return;
    }


    //returns the game ids of all currently running games
    public String getGames()
    {
        System.out.println("getGames()");

        String to_return = "";

        ArrayList<Integer> game_ids = new ArrayList<Integer>();
        for(int x = 0; x < games.size(); x++)
            game_ids.add(games.get(x).getGameID());

        Integer[] int_array = game_ids.toArray(new Integer[game_ids.size()]);

        return Arrays.toString(int_array);
    }


    //logs in
    public String login(String username, String password)
    {
        System.out.println("Logging in user...");

        //checks if player isn't already logged in
        //if already logged in, just give them the current player object
        for(int x = 0; x < players.size(); x++)
        {
            Player player = players.get(x);
            if(player.getUsername().equals(username))
            {
                String session_id = player.getSessionID();
                updatePlayerRequestTime(session_id);
                return session_id;
            }
        }

        Player player = sql_handler.login(username, password);

        //Player player = null;
        // //player unit testing with consistent session id
        //if(username.equals("james"))
        //    player = new Player(1, "james", "5F99BE1F0C41906DEF92F2859BFF7F7D");
        //else if(username.equals("other_player1"))
        //    player = new Player(2, "other_player1", "12345E1F0C41906DEF92F2859B54321");


        //if login successful
        if(player!=null) {

            addPlayer(player);

            //returns session id
            return player.getSessionID();
        }
        else
            return "error";
    }

    //logs out
    public boolean logout(String session_id)
    {
        System.out.println("Logging out user...");


        boolean logged_out = sql_handler.logout(session_id);


        //if login successful
        if(logged_out) {
            System.out.println("Logged out session id: "+session_id);
            removePlayer(session_id);
            return true;
        }

        return false;
    }


    //registers, and then logs in
    public String register(String username, String email, String password)
    {
        System.out.println("Registering user...");

        boolean user_exists = sql_handler.doesUsernameExist(username);
        boolean email_exists = sql_handler.doesEmailExist(email);

        if(user_exists)
            return "error";

        if (email_exists)
            return "error";


        boolean success = sql_handler.register(username, email, password);

        if(success)
            return login(username, password);
        else
            return "error";
    }

    //returns true if successful, which means if the player doesn't already exist
    public boolean addPlayer(Player player)
    {
        //checks that player doesn't already exist
        if(getPlayerIndex(player.getSessionID()) == -1)
        {
            //adds newly logged in player to list of active players
            players.add(player);
            //sets player's last request time
            player_last_request.put(player.getSessionID(), System.currentTimeMillis());

            return true;
        }
        else
            return false;


    }

    //removes player from the game server and any game they are part of
    public boolean removePlayer(String session_id)
    {
        int index = getPlayerIndex(session_id);


        //checks that player doesn't already exist
        if(index != -1)
        {
            players.remove(index);
            //removes player's last request time
            player_last_request.remove(index);

            //removes player from any games
            for(int x = 0; x < games.size(); x++)
            {
                Game game = games.get(x);
                game.removePlayer(session_id);
            }

            return true;
        }

        return false;
    }

    //returns a game based on its int id
    public Game getGameById(int game_id)
    {
        for(int x = 0; x < games.size(); x++)
        {
            if(games.get(x).getGameID() == game_id)
                return games.get(x);
        }

        return null;
    }

    //returns player object that corresponds to the session_id
    public Player getPlayer(String session_id)
    {
        int index = getPlayerIndex(session_id);
        if(index!=-1)
            return players.get(index);
        else
            return null;
    }

    //update last request time for current player
    public void updatePlayerRequestTime(String session_id)
    {
        player_last_request.put(session_id, System.currentTimeMillis());
        //player_last_request.set(getPlayerIndex(session_id), System.currentTimeMillis());
    }

    //returns index of player in players global variable
    public int getPlayerIndex(String session_id)
    {
        for(int x = 0; x < players.size(); x++)
        {
            String cur_session_id = players.get(x).getSessionID();
            if(cur_session_id.equals(session_id)) {
                return x;
            }
        }

        return -1;
    }
}
