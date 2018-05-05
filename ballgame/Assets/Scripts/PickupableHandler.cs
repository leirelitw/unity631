using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableHandler : MonoBehaviour {

    public int pickupable_id;

    private bool has_been_pickedup = false;

	// Use this for initialization
	void Start () {
        Debug.Log("PickupableHander started");
		
	}

    //returns true if pickupable has been picked up
    private bool check_if_pickedup()
    {
        //gets global list of pickupables

        List<int> picked_ups = Constants.picked_up_pickupables;

        //if this object was already picked up
        if (picked_ups.Contains(pickupable_id))
            return true;

        return false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (has_been_pickedup)
        {
            Debug.Log("Has been picked up!");
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
        else
        {
            has_been_pickedup = check_if_pickedup();
        }
    }
}
