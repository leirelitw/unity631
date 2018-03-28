
import java.sql.*;
import java.lang.Object;


public class MySQLHandler {

    Connection connection = null;
    public MySQLHandler()
    {
        connection = connect();
    }

    public Connection connect()
    {
        String host = "rollaball.cnwycvvueub2.us-east-1.rds.amazonaws.com";
        int port = 3306;
        String dbname = "RollABallDB";
        String username = "james";
        String password = "il73308440b";


        try {
            Class.forName("com.mysql.jdbc.Driver");
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

//        Connection connection = null;
        try {
            connection = DriverManager.getConnection("jdbc:mysql://"+host+":"+port+"/"+dbname, username, password);
            System.out.println("Connected");

        } catch (SQLException e) {
            e.printStackTrace();
        }

        return connection;
    }

    public int createGame()
    {
        int game_id = -1;


        try
        {
            String sql = "INSERT INTO games (players, num_players, map) VALUES (?,?,?)";
            PreparedStatement prepared = connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
            prepared.setString(1, "");
            prepared.setInt(2, 0);
            prepared.setString(3, "default");
            int result = prepared.executeUpdate();

            //if success
            if(result!=0)
            {
                //get the new game's id
                ResultSet rs = prepared.getGeneratedKeys();
                if (rs.next()) {
                    game_id = rs.getInt(1);
                }
            }
            else
                System.out.println("Error creating new game");

        }catch(Exception ex){
            System.out.println("createGame() error: "+ex.toString());
        }

        return game_id;

    }

    //returns player object
    public Player login(String username, String password)
    {
        Player player = null;

        //hashes provided password to compare to hashed passwords in database
        String hashed_password = Security.getHashedPassword(username, password);

        try
        {
            String sql = "SELECT * FROM users WHERE users.username=? AND users.password=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, username);
            prepared.setString(2, hashed_password);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next())
            {
                int user_id = rs.getInt("user_id");

                String session_id = Security.generateSessionID();

                //insert session id into the database
                boolean insert_session_id = insertSessionID(user_id, session_id);

                //if succeeded in adding session_id to database
                if(insert_session_id) {
                    //creates new player object for newly logged in player
                    player = new Player(user_id, username, session_id);
                }
            }
            else
                System.out.println("No results");

        }catch(Exception ex){

        }


        return player;
    }

    //logs user out by removing their session ID, and removing them from active games
    public boolean logout(String session_id)
    {
        try
        {
            //remove session_id from users table
            String statement = "UPDATE users SET users.session_id=? WHERE users.session_id=?";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, "");
            prepared.setString(2, session_id);
            int result = prepared.executeUpdate();

            //if successful removing sesssion_id
            if (result!=0)
            {

                //removes session_id from any active games

                return true;
            }

        }catch(Exception ex){

        }

        return false;
    }

    //returns boolean whether registration was successful
    public boolean register(String username, String email, String password)
    {
        //hashes provided password to compare to hashed passwords in database
        String hashed_password = Security.getHashedPassword(username, password);

        try
        {
            //insert session id into the database
            String statement = "INSERT INTO users (username, email, password) VALUES (?,?,?)";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, username);
            prepared.setString(2, email);
            prepared.setString(3, hashed_password);
            int result = prepared.executeUpdate();

            //if successfully insert row
            if(result!=0)
                return true;
            else
                return false;

        }catch(Exception ex){

        }

        return false;
    }


    //returns boolean whether username is already taken
    public boolean doesUsernameExist(String username)
    {
        try
        {
            String sql = "SELECT user_id FROM users WHERE users.username=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, username);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next())
                return true;
            else
                return false;

        }catch(Exception ex){

        }

        return false;
    }

    //returns boolean whether email is already taken
    public boolean doesEmailExist(String email)
    {
        try
        {
            String sql = "SELECT user_id FROM users WHERE users.email=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, email);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next())
                return true;
            else
                return false;

        }catch(Exception ex){

        }

        return false;
    }

    //updates
    private boolean insertSessionID(int user_id, String session_id)
    {
        try {
            //insert session id into the database
            String statement = "UPDATE users SET users.session_id=? WHERE users.user_id=?";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, session_id);
            prepared.setInt(2, user_id);
            prepared.executeUpdate();

            return true;
        } catch(Exception ex){
            System.out.println("updateSessionID() failed: "+session_id +" | "+ex.toString());
        }

        return false;
    }

}
