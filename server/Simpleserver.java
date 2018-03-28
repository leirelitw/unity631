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
        Socket socket = null;
        String resource = null;

        String mainRequestLine = "";

        try {
            server_socket = new ServerSocket(1299);
            System.out.println("Opened socket " + 1299);
            while (true) {

                // keeps listening for new clients, one at a time
                try {
                    socket = server_socket.accept(); // waits for client here
                } catch (IOException e) {
                    System.out.println("Error opening socket");
                    System.exit(1);
                }

                InputStream stream = socket.getInputStream();
                BufferedReader in = new BufferedReader(new InputStreamReader(stream));
                try {

                    // read the first line to get the request method, URI and HTTP version
                    String line = in.readLine();
                    System.out.println("----------REQUEST START---------");
                    System.out.println(line);

                    mainRequestLine = line;

                    System.out.println("----------REQUEST END---------\n\n");
                } catch (Exception ex) {
                    System.out.println("Error reading");
                    //System.exit(1);
                }

                try {
                    BufferedOutputStream out = new BufferedOutputStream(socket.getOutputStream());
                    PrintWriter writer = new PrintWriter(out, true);  // char output to the client

//                    // every response will always have the status-line, date, and server name
//                    writer.println("HTTP/1.1 200 OK");
//                    writer.println("Server: TEST");
//                    writer.println("Connection: close");
//                    writer.println("Content-type: text/plain");
//                    writer.println("");

                    String linePartsarray[] = mainRequestLine.split(" ");
                    //just the uri, so "/User"
                    String resourceString = linePartsarray[1];
                    System.out.println("Resource String: " + resourceString);

//                Processor processor = ProcessorFactory.getProcessor(resourceString);

                    String response = game_server.processRequest(resourceString);

                    // Body of our response
//                writer.println(processor.process());

                    writer.println(response);
                } catch(Exception ex){
                    System.out.println("Error writing: "+ex.toString());
                }

                socket.close();
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
