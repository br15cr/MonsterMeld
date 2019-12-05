using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalType {
    FIRE
}


public class Orb : MonoBehaviour
{
    private const float SPEED = 0.1f;
    private const float RADIUS = 0.5f;
    private const float DAMP = 0.8f;
    private const float GRAVITY = 0.1f;
    
    private Transform target;
    private Vector3 velocity;
    private bool onGround = false;
    
    void Start(){
        
    }

    void Update(){
        
    }

    void FixedUpdate(){
	if(target != null){
	    velocity = (target.position - transform.position)*SPEED;
	}
	UpdateGround();
	if(!onGround){
	    velocity += GRAVITY * Vector3.down;
	}
	transform.position += velocity;
	velocity*=DAMP;
	if(target != null){
	    if(Vector3.Distance(transform.position,target.position) <= RADIUS){
		// Get Collected
		target.GetComponent<OrbPouch>().AddOrb();
		Destroy(gameObject);
	    }
	}
    }

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
