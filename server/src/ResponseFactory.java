
import java.io.File;
import java.io.IOException;

public class ResponseFactory 
{   
    static String baseFilepath = "/";
    public ResponseFactory ()
    {
        
    }

    public Response getResponse(String resource) throws IOException {
            //return responseAfterWebServerFlow (resource);
        return createResponse(resource);
    }

    private Response createResponse(String body)
    {
        Response r = new Response();
        r.putHeaders("Content-type", "text/html");
        r.putHeaders("Connection", "close");

        r.setMessage(body);

        r.generateResponseString();
        return r;
    }

    //returns contents of a file, html and whatnot
    private Response fileGet(String file_path)
    {
        Response r = new Response();
        r.putHeaders("Content-type", "text/html");
        r.putHeaders("Connection", "close");
        try
        {
            if(file_path == null)
            {

                String filepath= baseFilepath+ "index.html";
                File f = new File(filepath);
                if(f.exists())
                {

                    r.getFileContent(filepath);
                    r.generateResponseString();
                    return r;
                }

            }
            else
            {
                if(file_path.contains(".")){
                    String extension = file_path.split("\\.")[1];

                    String filepath= baseFilepath+ file_path;
                    File f = new File(filepath);
                    if(f.exists())
                    {
                        if((extension.equals("png"))||(extension.equals("jpg"))||(extension.equals("jpeg")))
                        {
                            r.putHeaders("Content-type", "image/"+extension);
                        }
                        else if(extension.equals("zip"))
                        {
                            r.putHeaders("Content-type", "application/"+extension);
                        }

                        r.getFileContent(filepath);
                        r.generateResponseString();
                        return r;
                    }
                }

            }

        }

        catch(Exception ex)
        {
            return new Response();
        }

        r.setMessage("<p>404 - FILE NOT FOUND  </p>");

        r.generateResponseString();
        return r;
    }

    private static Response responseAfterWebServerFlow (String resource) throws IOException
    {
        Response r = new Response();
        r.putHeaders("Content-type", "text/html");
        r.putHeaders("Connection", "close");
        try
        {
            if(resource == null)
            {
                
                String filepath= baseFilepath+ "index.html";
                File f = new File(filepath);
                if(f.exists())
                {
                 
                    r.getFileContent(filepath);  
                    r.generateResponseString();
                    return r;
                }
               
            }
            else
            {
                if(resource.contains(".")){
                String extension = resource.split("\\.")[1];
               
                        String filepath= baseFilepath+ resource;
                        File f = new File(filepath);
                        if(f.exists())
                        {
                             if((extension.equals("png"))||(extension.equals("jpg"))||(extension.equals("jpeg")))
                             {
                                  r.putHeaders("Content-type", "image/"+extension);
                             }
                             else if(extension.equals("zip"))
                             {
                                  r.putHeaders("Content-type", "application/"+extension);
                             }
                           
                            r.getFileContent(filepath); 
                            r.generateResponseString();
                            return r;
                        }
                }
                        
            }

        }
       
        catch(Exception ex)
        {
            return new Response();
        }

        r.setMessage("<p>404 - FILE NOT FOUND  </p>");

        r.generateResponseString();
        return r;
    }
    
}