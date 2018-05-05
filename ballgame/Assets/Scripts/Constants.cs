using System.Collections.Generic;

public class Constants {
	
	// Constants
	public static readonly string CLIENT_VERSION = "1.00";
    //public static readonly string REMOTE_HOST = "localhost";
    public static readonly string REMOTE_HOST = "34.196.150.205";
	public static readonly int REMOTE_PORT = 1299;

    //true if player is in multiplayer, and scenes will act differently
    public bool is_in_multiplayer = false;
	
	// Request (1xx) + Response (2xx)
	public static readonly short response_login = 101;
    public static readonly short response_register = 102;
    public static readonly short response_getgames = 103;
    public static readonly short response_joingame = 104;
    public static readonly short response_gametimer = 105;
    public static readonly short response_getcoor = 106;
    public static readonly short response_pickedup = 107;
    
	
	// Other
	public static readonly string IMAGE_RESOURCES_PATH = "Images/";
	public static readonly string PREFAB_RESOURCES_PATH = "Prefabs/";
	public static readonly string TEXTURE_RESOURCES_PATH = "Textures/";



    //keeps a list of picked up pickupables
    public static List<int> picked_up_pickupables = new List<int>();


    // GUI Window IDs
    public enum GUI_ID {
		Login
	};

	public static int USER_ID = -1;
}