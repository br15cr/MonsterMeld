using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPouch : MonoBehaviour
{
    private int orbs; // just a test

    public int Count {
	get { return this.orbs; }
    }
    
    void Start(){
        
    }

    void Update(){
        
    }

    public void AddOrb(){ // also just a test
	orbs++;
    }

    public void AddOrbs(int amount){
	orbs+=amount;
    }

    public void TakeOrbs(int amount){
	orbs-=amount;
    }
}
