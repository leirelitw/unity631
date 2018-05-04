import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.*;


class SimpleServer {

    public GameServer game_server;


    public SimpleServer()
    {

        game_server = new GameServer();
    }

    public void run()
    {
        ServerSocket server_socket;
        //Socket socket = null;


        try {
            server_socket = new ServerSocket(1299);
            System.out.println("Opened server socket " + 1299);
            while (true) {

                // keeps listening for new clients, one at a time
                Socket socket = null;
                try {
                    socket = server_socket.accept(); // waits for client here
                } catch (IOException e) {
                    System.out.println("Error opening socket");
                    System.exit(1);
                }

                //multithreading request handling
                RequestHandler request_handler = new RequestHandler(game_server, socket);
                Thread thread = new Thread(request_handler);
                thread.start();
            }
        } catch (IOException e) {
            System.out.println("Error opening socket");
            System.exit(1);
        }
    }


    public static void main(String[] args) throws IOException {
        SimpleServer server = new SimpleServer();
        server.run();
    }
}


class RequestHandler implements Runnable
{
    private GameServer game_server;
    private Socket socket = null;

    public RequestHandler(GameServer game_server, Socket new_socket)
    {
        this.game_server = game_server;
        this.socket = new_socket;
    }

    @Override
    public void run()
    {
        //System.out.println("Handling request...");

        String resource = null;

        try
        {
            InputStream stream = socket.getInputStream();
            BufferedReader in = new BufferedReader(new InputStreamReader(stream));

            String line = "";
            try {

                // read the first line to get the request method, URI and HTTP version
                String temp_line = "";
                int counter = 0;
                do{
                    line += temp_line;
//                    System.out.println("Line: "+line);
                    temp_line = in.readLine();

                    char character = temp_line.charAt(0); // This gives the character 'a'
                    int ascii = (int) character;
//                    System.out.println("ASCII: "+ascii);

//                    System.out.println(counter + ": "+temp_line);
                } while(temp_line!=null && temp_line!="");

                //System.out.println("----------REQUEST START---------");
                //System.out.println(line);



                //System.out.println("----------REQUEST END---------");
            } catch (Exception ex) {
//                System.out.println("Error reading");
            }

            System.out.println("Request: "+line);

            try {
                //System.out.println("----------RESPONSE START---------");
                BufferedOutputStream out = new BufferedOutputStream(socket.getOutputStream());
                PrintWriter writer = new PrintWriter(out, true);  // char output to the client


                String response = game_server.processRequest(line);
                writer.println(response);
                writer.flush();

                // Body of our response
                //System.out.println("-- Sending out --");
                System.out.println("Response: "+response);


                //System.out.println("----------RESPONSE END---------\n\n");
            } catch (Exception ex) {
                System.out.println("Error writing: " + ex.toString());
            }
        } catch(Exception ex){

        }

        //closes socket connection once done
        try
        {
            socket.close();
        } catch(Exception ex){}
    }
}
