//package simpleserver;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.*;



public class SimpleserverBytes {
    protected Hashtable headers = new Hashtable();
    protected String method = "";
    protected String httpVersion = "";
    protected String URI = "";
    protected byte[] body = new byte[0];
    protected boolean scriptRequest = false;
    protected String pathInfo = "";
    protected String queryString = "";


    private ServerSocket server_socket;
    private Socket socket;
    private int port = 1299;
    public InputStream inputStream; // For use with incoming requests
    private OutputStream outputStream; // For use with outgoing responses
    private DataInputStream dataInputStream; // Stores incoming requests for use
    private ResponseFactory responseFactory;

    public SimpleserverBytes(){
        responseFactory = new ResponseFactory();
    }

    //opens socket
    public void connect(){
        System.out.println("Connecting...");

        try {
            server_socket = new ServerSocket(port);
            System.out.println("Opened socket " + port);
        }
        catch (IOException e) {
            System.out.println("Error opening socket");
            System.exit(1);
        }
    }

    //listens for client
    public void run(){

        try {
            while(true) {
                System.out.println("Listening...");
                socket = server_socket.accept();
                System.out.println("Received request...");

                inputStream = socket.getInputStream();
                outputStream = socket.getOutputStream();
                dataInputStream = new DataInputStream(inputStream);


//                // Extract the size of the package from the data stream
//                short requestLength = DataReader.readShort(dataInputStream);
//
//                if (requestLength > 0) {
//
//                    // Separate the remaining package from the data stream
//                    byte[] buffer = new byte[requestLength];
//                    inputStream.read(buffer, 0, requestLength);
//                    DataInputStream dataInput = new DataInputStream(new ByteArrayInputStream(buffer));
//                    // Extract the request code number
//                    short requestCode = DataReader.readShort(dataInput);
//
//                    System.out.println("Received short: "+Short.toString(requestCode));
//                }
//                else
//                    System.out.println("Request len is 0");






                long lastActivity = System.currentTimeMillis();
                short requestCode = -1;

                boolean isDone = false;

                while (!isDone)
                {
                    try {
                        // Extract the size of the package from the data stream
                        short requestLength = DataReader.readShort(dataInputStream);

                        if (requestLength > 0) {
                            System.out.println("Request len: "+Short.toString(requestLength));

                            byte[] to_return = new byte[2];
                            send(to_return);

                            isDone = true;
                        } else {
                            // If there was no activity for the last moments, exit loop
                            int timeout_seconds = 90;
                            if ((System.currentTimeMillis() - lastActivity) / 1000 >= timeout_seconds) {
                                isDone = true;
                            }
                        }
                    } catch (Exception ex) {
                        System.out.printf("Request [%d] Error:", requestCode);
                        System.out.printf(ex.getMessage());
                        System.out.printf("---");
                        ex.printStackTrace();
                    }
                }




                System.out.println("End of request");
            }

        } catch(Exception ex) {
            System.out.println("Error: "+ex.toString());
        }
    }

    //sends response in bytes
    public void send(byte[] to_send) throws IOException {
        outputStream.write(to_send);
    }



    public static void main(String[] args) {
        SimpleserverBytes simple = new SimpleserverBytes();
        simple.connect();
        simple.run();
    }

}