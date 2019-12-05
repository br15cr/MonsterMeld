using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalType {
    FIRE
}


public class Orb : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity;
    private const float SPEED = 0.1f;
    private const float RADIUS = 0.5f;
    
    void Start(){
        
    }

    void Update(){
        
    }

    void FixedUpdate(){
	if(target != null){
	    velocity = (target.position - transform.position)*SPEED;
	}
	transform.position += velocity;
	if(target != null){
	    if(Vector3.Distance(transform.position,target.position) <= RADIUS){
		// Get Collected
		target.GetComponent<OrbPouch>().AddOrb();
		Destroy(gameObject);
	    }
	}
    }

    void OnTriggerEnter(Collider c){
	if(target == null){
	    //Player ply = c.GetComponent<Player>();
	    OrbPouch pouch = c.GetComponent<OrbPouch>();
	    if(pouch != null){
		target = pouch.transform;
		Debug.Log("GOT TARGET");
	    }
	}
    }
}
