using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Player player;
    private Vector2 leftStick;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        leftStick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        player.Move(leftStick);

        if (Input.GetButtonDown("Call"))
        {
            Debug.Log("Calling Monsters");
            player.CallMonsters();
        }

	if(Input.GetButtonDown("Jump")){
		Debug.Log("JUMPING");
		player.Jump();
	}

	if(Input.GetButtonDown("Box")){
	    player.GrabDropBox();
	}
	
        // if (Input.GetButtonDown("debug_spawn"))
        // {
        //     player.SpawnMonster();
        // }
        if (Input.GetButtonDown("Attack"))
        {
            player.AttackMonsters();
        }
    }
}
