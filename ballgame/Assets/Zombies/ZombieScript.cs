using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour {


    Animator animator;
    private Vector3 lastPosition;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 movement = new Vector3(0.0f, 0.0f, 1);// don't need

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("ZombieWalk"))// moves zombie when its walking
        {
            transform.position += transform.forward * Time.deltaTime * 5; // EXPERIMENT
    
        }
        else //will be true during other animations
        {
            animator.SetBool("AtPoint", false);
        }
        if(Vector3.Distance(transform.position, lastPosition) > 20) // how far zombie will go before stopping/proceed with other animations
        {
            animator.SetBool("AtPoint", true);
            lastPosition = transform.position;
        }

    }
}
