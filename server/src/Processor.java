
import java.util.*;

public class Processor {

    private String type;
    private String args;

//    private ArrayList<String> argNames = new ArrayList<String>();
    private ArrayList<String> argValues = new ArrayList<String>();

    public Processor(String type, String args) {
        this.type = type;
        this.args = args;

        splitArgs();
    }

    //splits up arguments
    public void splitArgs()
    {
        try {

            String[] argParts = this.args.split("&");

            for (int i = 0; i < argParts.length; i++) {
                String[] argSplit = argParts[i].split("=");
//            argNames.add(argSplit[0]);
                argValues.add(argSplit[1]);
            }
        } catch(Exception ex){

        }
    }

    public String process()
    {
        String to_return = "";
        switch(type){
            case "/login":
                to_return = login();
                break;
            case "/register":
                to_return = register();
                break;
            default:
                break;
        }

        return to_return;
    }

    //logs in
    public String login()
    {
        System.out.println("Logging in user...");

        String username = argValues.get(0);
        String password = argValues.get(1);

        System.out.println("Username: "+username);
        System.out.println("Password: "+password);

        MySQLHandler sql_handler = new MySQLHandler();
        Player player = sql_handler.login(username, password);

        System.out.println(player.toString());


        //returns session id
        return player.getSessionID();
    }

    public String register()
    {
        System.out.println("Registering user...");
        String username = argValues.get(0);
        String password = argValues.get(1);


        System.out.println("Username: "+username);
        System.out.println("Password: "+password);

        return login();
    }

}
