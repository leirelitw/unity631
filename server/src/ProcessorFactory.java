

public class ProcessorFactory {

    public static Processor getProcessor(String resource) {
        Processor processor = new Processor("error", "");

        System.out.println(resource);

        String endpoint;
        String queryString = null;
        String endpointParts[] = resource.split("\\?");

        //separates endpoint from parameters
        endpoint = endpointParts[0];
        if (endpointParts.length == 2) {
            queryString = endpointParts[1];
        }

        System.out.println("Endpoint: "+endpoint);
        System.out.println("queryString: "+queryString);

        //processes the request for different endpoints
        try {
            processor = new Processor(endpoint.toLowerCase(), queryString);
        } catch(Exception ex){

        }

        return processor;
    }
}