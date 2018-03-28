import java.util.*;
import java.util.concurrent.TimeUnit;


public class GameServer {

    public ArrayList<Player> players;
    public ArrayList<Game> games;

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

        System.out.println("Endpoint: "+endpoint);
        System.out.println("queryString: "+queryString);


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

            //processes the request for different endpoints
            switch(endpoint.toLowerCase()){
                case "/login":
                    to_return = login(values[0], values[1]);
                    break;
                case "/register":
                    to_return = register(values[0], values[1], values[2]);
                    break;
                case "/newgame":
                    to_return = newGame();
                    break;
                case "/getgames":
                    to_return = getGames();
                    break;
                case "/joinGame":
//                    to_return = joinGame(values[0]);
                    break;
                default:
                    break;
            }
        } catch(Exception ex){
            System.out.println("Error: "+ex.toString());
        }

        return to_return;
    }

    public void manageGames()
    {
        //creates new thread to manage games so that it doesn't interfere with requests
        gameThread = new Thread() {

            public void run() {

                loadGames();

                int seconds = 0;
                while (true) {

                    System.out.println("Manage Games second: " + seconds);


                    seconds++;

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

    //loads current games from sql database
    public void loadGames()
    {

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

    public String newGame()
    {
        System.out.println("newGame()");
        int game_id = sql_handler.createGame();
        System.out.println("newGmaeID: "+game_id);

        Game new_game = new Game(game_id);
        System.out.println("Created new game object");
        games.add(new_game);
        System.out.println("added new game to games");

        return Integer.toString(game_id);
    }


    //logs in
    public String login(String username, String password)
    {
        System.out.println("Logging in user...");

        Player player = sql_handler.login(username, password);


        //if login successful
        if(player!=null) {
            System.out.println(player.toString());

            //adds newly logged in player to list of active players
            players.add(player);

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
            return "Error: Username already exists";

        if (email_exists)
            return "Error: Email already exists";


        boolean success = sql_handler.register(username, email, password);

        if(success)
            return login(username, password);
        else
            return "Error: Couldn't register";
    }
}
