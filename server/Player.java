import java.util.*;


public class Player {

    private int user_id;
    private String username = "";
    private String session_id;

    public Player(int user_id, String username, String session_id)
    {
        this.user_id = user_id;
        this.username = username;
        this.session_id = session_id;
    }

    public void setUsername(String username)
    {
        this.username = username;
    }

    public String getUsername()
    {
        return this.username;
    }

    public int getUserID()
    {
        return user_id;
    }

    public void setUserID(int new_user_id)
    {
        this.user_id = new_user_id;
    }

    public String getSessionID()
    {
        return session_id;
    }

    public void setSessionID(String new_session_id)
    {
        this.session_id = new_session_id;
    }

    public String toString()
    {
        String to_print = "-- Player --" + System.lineSeparator();
        to_print += "user_id: " + user_id + System.lineSeparator();
        to_print += "username: "+ username + System.lineSeparator();
        to_print += "session_id: "+session_id + System.lineSeparator();
        to_print += "----";

        return to_print;
    }
}
