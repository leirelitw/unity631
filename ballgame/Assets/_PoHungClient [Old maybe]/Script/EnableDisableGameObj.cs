using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableGameObj : MonoBehaviour {


    private bool enable = false;

    public void Enable()
    {
        enable = true;
        gameObject.SetActive(enable);
        //To pause player object by disable scripts relate
        



    }

    // Update is called once per frame
    public void Disable()
    {
        enable = false;

        gameObject.SetActive(enable);
        //enable player object movement
        
    }



    public void EnableDisableObj()
    {

        

        if (enable)
        {
            Disable();
            
        }
        else
        {
            Enable();
        }
    }
}
