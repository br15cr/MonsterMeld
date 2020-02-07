using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTrigger : MonoBehaviour {
    public ProgressState state;
    void Start() {
	ProgressManager manager = GameObject.Find("Player").GetComponent<ProgressManager>();
	manager.OnProgressStateChange += ProgressStateChange;
    }

    void Update() {
        
    }

    protected virtual void ProgressStateChange(ProgressState trigState){
	if(trigState == state){
	    Debug.Log("TRIGGERED");
	}
    }
    
}
