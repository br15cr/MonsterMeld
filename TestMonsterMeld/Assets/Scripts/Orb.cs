using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalType {
    FIRE
}


public class Orb : MonoBehaviour
{
    private const float RADIUS = 1f;
    private const float DAMP = 0.8f;
    private const float GRAVITY = 0.1f;
    
    private float speed = 3;
    private Transform target;
    private Vector3 velocity;
    private bool onGround = false;
    private SphereCollider col;
    private Rigidbody body;
    
    void Start(){
        col = GetComponent<SphereCollider>();
	body = GetComponent<Rigidbody>();
    }

    void Update(){
        
    }

    void FixedUpdate(){
	if(target != null){
	    body.velocity = (target.position - transform.position)*speed;
	}
	/*
	if(target != null){
	    velocity = (target.position - transform.position)*speed;
	}
	UpdateGround();
	if(!onGround && target == null){
	    velocity += GRAVITY * Vector3.down;
	}
	transform.position += velocity;
	velocity*=DAMP;
	if(target != null){
	    float dist = Vector3.Distance(transform.position,target.position);
	    //speed = ((col.radius-dist)/col.radius)*0.1f;
	    if( dist <= RADIUS){
		// Get Collected
		target.GetComponent<OrbPouch>().AddOrb();
		Destroy(gameObject);
	    }
	}
	*/
	if(target != null){
	    float dist = Vector3.Distance(transform.position,target.position);
	    if( dist <= RADIUS){
		// Get Collected
		if(!(target.GetComponent<Monster>() != null && target.GetComponent<Monster>().GetHealth() == 100)){
		    target.GetComponent<OrbPouch>().AddOrb();
		    Destroy(gameObject);
		}else{
		    target = null;
		}
	    }
	}
    }

    // obsolete
    private void UpdateGround(){
	if(!onGround){
	    RaycastHit hit;
	    onGround = Physics.Raycast(transform.position,-transform.up,out hit,RADIUS);
	    velocity = new Vector3(velocity.x,0,velocity.z);
	}
    }

    public void Jump(){
	float strength = 1;
	velocity = new Vector3(Random.Range(-strength,strength),Random.Range(0,strength),Random.Range(-strength,strength));
    }

    /*
    void OnCollisionEnter(Collision c){
	if(c.collider.transform == target){
	    target.GetComponent<OrbPouch>().AddOrb();
	    Destroy(gameObject);
	}
    }
    */

    void OnTriggerEnter(Collider c){
	Debug.Log(c.transform.name + " Trigger Enter");
	if(target == null){
	    //Player ply = c.GetComponent<Player>();
	    OrbPouch pouch = c.GetComponent<OrbPouch>();
	    if(pouch == null){
		pouch = c.GetComponent<OrbFeeder>();
		return;
	    }
	    if(pouch != null){
		if(!(c.GetComponent<Monster>() != null && c.GetComponent<Monster>().GetHealth() == 100)){
		    target = c.transform;//pouch.transform;
		    Debug.Log("GOT TARGET");
		    gameObject.layer = 9; // Ignore Player
		}
	    }
	}
    }
}
