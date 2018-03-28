/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
//import java.time.ZoneId;
//import java.time.ZonedDateTime;

//import java.time.format.DateTimeFormatter;
//import java.time.format.DateTimeFormatter;
//import java.time.LocalDateTime;
//import java.time.*;
//import java.time.format.*;
import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author anu
 */
public class Response {
    protected String statusCode;
    protected String reasonPhrase;
    protected HashMap<String,String> headers = new HashMap<>(); 
    protected String message;
    protected String version;
    public String response;
    private int contentLength = 0;
    public byte[] body;
    
    public Response()
    {
        version = "HTTP/1.1";
//        headers.put("Date", DateTimeFormatter.RFC_1123_DATE_TIME.format(ZonedDateTime.now(ZoneId.of("GMT"))));
//        System.out.println(DateTimeFormatter.RFC_1123_DATE_TIME.format(ZonedDateTime.now(ZoneId.of("GMT"))));

        //Mon, 12 Mar 2018 20:46:43 GMT

//        LocalDate date = LocalDate.now();
//        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy/MM/dd HH:mm:ss");
        headers.put("Date", "Fuck the date");
        headers.put("Server", "TEST");
        statusCode = "200";
        reasonPhrase = "OK";
    }
    
    public void putHeaders(String key, String value){
         headers.put(key, value);
    }
    
    public void getFileContent(String filePath) throws IOException
    {
        body = Files.readAllBytes(Paths.get(filePath));
        contentLength = body.length;
    }
    
    public void setMessage(String message){
        this.message = message;
    }
    
     public void generateResponseString()
    {
        
        response = "";
        response += version;
        response += " " + statusCode;
        response += " " + reasonPhrase;
        response += "\r\n";
        if(headers != null)
        {
            for(Map.Entry<String, String> entry : headers.entrySet())
            {
                response += entry.getKey()+ ":" + entry.getValue() + "\r\n";
            }
        }
        response += "\r\n";
        if(message != null)
        {
            response += message;
        }
       
    }

}
