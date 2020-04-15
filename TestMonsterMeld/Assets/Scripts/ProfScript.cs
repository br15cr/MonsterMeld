using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfScript : MonoBehaviour
{
    public GameObject dummyBox;
    private bool boxTaken = false;
    public GameObject dialogue;
    
    void Start()
    {
        dialogue.SetActive(false);
    }


    void TakeBox(Player player){
	if(!boxTaken){
	    boxTaken = true;
	    Destroy(dummyBox);
	    player.GiveFusionBox();
	    dialogue.SetActive(true);
	}
    }

    void OnTriggerEnter(Collider c){
	if(!boxTaken){
	    Player ply = c.GetComponent<Player>();
	    if(ply != null){
		TakeBox(ply);
	    }
	}
    }
}
