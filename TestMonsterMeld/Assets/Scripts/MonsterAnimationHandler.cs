using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationHandler : MonoBehaviour
{
    public DisplayState display;



    public void StartHit(){
	if(display != null){
	    display.StartHit();
	}
    }

    public void StopHit(){
	if(display != null)
	    display.StopHit();
    }
}
