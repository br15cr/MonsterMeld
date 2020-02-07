using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProgressState {
    TUTORIAL_BEGIN,
    TUTORIAL_END,
};


public class ProgressManager : MonoBehaviour
{
    public ProgressState prog;

    public delegate void ProgressStateChangeDelegate(ProgressState state);
    public ProgressStateChangeDelegate OnProgressStateChange;
    
    void Start() {
        prog = ProgressState.TUTORIAL_BEGIN;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetProgress(ProgressState prg){
	// only increase progression, not other way around
	if(prg == prog+1){
	    prog = prg;
	    OnProgressStateChange(prog);
	}
    }
}
