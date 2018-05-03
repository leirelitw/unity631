using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjColor : MonoBehaviour {

    public Material[] materials;//create a array space for materials

    public Renderer rend;

    private int index = 1;


	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];//default to first material
	}

    private void OnMouseDown()
    {
        if (materials.Length == 0)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            index += 1;//increment index every click
            if(index > materials.Length)//if index reach the end of array, return to the first
            {
                index = 1;
            }

            rend.sharedMaterial = materials[index - 1];
        }
    }





}
