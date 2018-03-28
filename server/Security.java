import java.security.*;
import java.security.MessageDigest;
import java.math.*;


public class Security {

    public Security()
    {

    }

    public static String getHashedPassword(String username, String password)
    {
        String md5_hash = "";
        try {
            String to_hash = username + "randomsecurestring2050";
            MessageDigest md5 = java.security.MessageDigest.getInstance("MD5");

            byte[] md5sum = md5.digest(to_hash.getBytes());
            md5_hash = String.format("%032X", new BigInteger(1, md5sum));
        } catch(Exception ex){
            System.out.println("Error with md5 hash, should never happen: "+ex.toString());
        }

        //gets the first 22 characters of the md5 hash
        String len22 = md5_hash.substring(0, 22);
        //$2a$XX$ where XX is the number of rounds. 12+ is better
        String salt = "$2a$12$"+len22;

//        System.out.println("Salt: "+salt);

        // the work factor is 2**log_rounds, and the default is 10
        String hashed_password = BCrypt.hashpw(password, salt);

        return hashed_password;
    }

    //generates random md5 hash
    public static String generateSessionID()
    {
        String md5_hash = "ifyoucanseethiscallforhelp";
        try {
            int random = (int )(Math.random() * 10000 + 1);

            //hashes current epoch time, random integer, and hardcoded string
            String to_hash = Long.toString(System.currentTimeMillis()) + Integer.toString(random) + "notrandomsecuritystring2049";
            MessageDigest md5 = java.security.MessageDigest.getInstance("MD5");

            byte[] md5sum = md5.digest(to_hash.getBytes());
            md5_hash = String.format("%032X", new BigInteger(1, md5sum));
        } catch(Exception ex){
            System.out.println("Error with md5 hash, should never happen: "+ex.toString());
        }

        return md5_hash;
    }
}
