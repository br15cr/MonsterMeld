using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTester : MonoBehaviour
{
    
    private Health health;

    private bool dead = false;
    
    void Start(){
        health = GetComponent<Health>();
	health.OnDeath += Death;
    }

    void Update(){
        if(Input.GetKeyDown("a")){
	    health.Damage(10);
	}
	
	if(Input.GetKeyDown("s")){
	    health.Damage(-10);
	}
    }

    void Death(){
	dead = true;
    }

    void OnGUI(){
	GUI.color = Color.white;
	GUI.Label(new Rect(10,10,100,100),"Health: "+health.Amount.ToString());
	if(dead){
	    GUI.color = Color.red;
	    GUI.Label(new Rect(Screen.width/2,Screen.height/2,100,100),"You Died");
	}
    }

    
}
