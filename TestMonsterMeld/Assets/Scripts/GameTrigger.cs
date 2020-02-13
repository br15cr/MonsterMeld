using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameTrigger : MonoBehaviour
{
	private GameState gameState;
	public string trigName = "Name";

    // Start is called before the first frame update
    void Start()
    {
        gameState = transform.root.gameObject.GetComponent<GameState>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter (Collider other)
    {
    	gameState.gameTriggerActivated(trigName);
    	Debug.Log ("Object Entered the trigger");

    }

    void OnTriggerStay (Collider other)
    {
    	//Debug.Log ("Object is within the trigger");
    }

    void OnTriggerExit (Collider other)
    {
    	Debug.Log ("Object Exited the trigger");
    }


}
