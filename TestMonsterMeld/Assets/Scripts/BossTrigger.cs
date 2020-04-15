using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c){
	if(!triggered){
	    Player player = c.GetComponent<Player>();
	    if(player != null){
		triggered = true;
		player.plyCam.OnBossTrigger();
	    }
	}
    }
}
