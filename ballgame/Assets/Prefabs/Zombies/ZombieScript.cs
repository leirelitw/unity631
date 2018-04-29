using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour {


    Animator animator;
    public GameObject explosion;
    private Vector3 lastPosition;
    private Vector3 explosionPosition;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Note, didn't check when zombie is just walking into wall or terrain itself.

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("ZombieWalk"))// moves zombie when its walking
        {
            transform.position += transform.forward * Time.deltaTime * 5; //computes how fast zombie will change position
    
        }
        else //will be true during other animations
        {
            animator.SetBool("AtPoint", false);
        }
        if(Vector3.Distance(transform.position, lastPosition) > 20) // how far zombie will go before stopping and proceeding with other animations.
        {
            animator.SetBool("AtPoint", true);
            lastPosition = transform.position;
        }
        explosionPosition = transform.position;
        explosionPosition.y += 5;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, explosionPosition, transform.rotation);
        }
    }
}
