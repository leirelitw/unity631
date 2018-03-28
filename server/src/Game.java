import java.util.*;
import java.util.concurrent.TimeUnit;


public class Game {

    private int game_id;

    //number of players currently playing in the game
    private int num_players = 0;
    //minimum number of players for a game to start
    private int min_num_players = 2;
    //maximum number of players in a game
    private int max_num_players = 6;
    private Player[] players = new Player[max_num_players];
    //true if game has started
    private boolean started = false;
    //true if game has ended
    private boolean finished = false;
    //number of seconds left in play
    private int seconds_left = 300;
    //number of seconds to wait for game to start
    private int time_to_wait = 60;

    private Thread runThread;



    public Game(int game_id)
    {
        this.game_id = game_id;
    }

    //runs the game in its own thread
    public void run()
    {
        runThread = new Thread() {

            public void run() {
                int seconds = 0;
                while (true) {

                    System.out.println("Game: "+game_id);
                    System.out.println("Num players: "+num_players);
                    System.out.println();


                    seconds++;

                    try {
                        TimeUnit.SECONDS.sleep(1);
                    } catch (Exception ex) {
                        System.out.println("Timeout ERROR: " + ex.toString());
                    }
                }
            }

        };
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
                if (players[x].getSessionID() == player.getSessionID())
                    player_already_exists = true;
            }

            //if player already isn't in game, add them
            if (player_already_exists == false) {
                players[num_players] = player;
                num_players++;
            } else
                return false;
        }
        else
            return false;

        return true;
    }


    //returns array of all players in the game
    public Player[] getPlayers()
    {
        return this.players;
    }

    public int getGameID()
    {
        return this.game_id;
    }

    public String toString()
    {
        String to_print = "-- Game --" + System.lineSeparator();
        to_print += "----";

        return to_print;
    }
}
