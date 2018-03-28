
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

import java.sql.*;

public class Client {

	private static String USER_AGENT = "Mozilla/6.0";

	
	public static void main(String[] args) throws IOException {
		sendGetRequest();

        //Connection db_connection = testRDS();

        System.out.println("Done.");
	}

	public static Connection testRDS()
    {
        String host = "rollaball.cnwycvvueub2.us-east-1.rds.amazonaws.com";
        int port = 3306;
        String dbname = "RollABallDB";
        String username = "james";
        String password = "il73308440b";


        Connection conn = null;
        try {Class.forName("com.mysql.jdbc.Driver");} catch (ClassNotFoundException e) {e.printStackTrace();}
        try {
            conn = DriverManager.getConnection("jdbc:mysql://"+host+":"+port+"/"+dbname, username, password);
            System.out.println("Connected");

        } catch (SQLException e) {
            e.printStackTrace();
        }
        return conn;

    }

	private static void sendGetRequest() throws IOException {
            URL url_obj = new URL("http://34.204.79.29:1299/thiosisthevaluereturned");

            //sends GET request
            HttpURLConnection connection = (HttpURLConnection)(url_obj.openConnection());
            connection.setRequestMethod("GET");
            connection.setRequestProperty("User-Agent", USER_AGENT);

            //gets response from GET request
            int response_code = connection.getResponseCode();

            System.out.println("GET Response: " + response_code);

            //if successful response
            //read response
            if (response_code == HttpURLConnection.HTTP_OK)
            {
                //for reading response's byte stream
                BufferedReader in = new BufferedReader(new InputStreamReader(connection.getInputStream()));


                //stores response
                StringBuffer response = new StringBuffer();

                //reads response
                String inputLine;
                while ((inputLine = in.readLine()) != null) {
                        response.append(inputLine);
                }
                in.close();

                //prints response
                System.out.println(response.toString());
            }
            else
                System.out.println("Error");

	}


}