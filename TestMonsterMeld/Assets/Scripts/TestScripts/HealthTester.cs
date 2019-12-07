using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTester : MonoBehaviour
{
    private Health health;
    
    void Start(){
        health = GetComponent<Health>();
    }

    void Update(){
        if(Input.GetKeyDown("a")){
	    AttackInfo a = new AttackInfo(8.32f);
	    health.Damage(a);
	}
	
	if(Input.GetKeyDown("s")){
	    AttackInfo a = new AttackInfo(-10);
	    health.Damage(a);
	}
    }

    void OnGUI(){
	GUI.Label(new Rect(10,10,100,100),"Health: "+health.Amount.ToString());
    }

    
}
