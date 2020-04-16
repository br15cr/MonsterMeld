using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Player player;
    private Vector2 leftStick;
    private PlayerCamera cam;
    //public GameState gameState;
    private bool gameplay = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
	cam = Camera.main.GetComponent<PlayerCamera>();

    }

    // Update is called once per frame
    void Update()
    {
	if(!player.IsDead && Time.timeScale > 0)
	    Controls();
    }

    private void Controls()
    {
    	//gameState = cam.GetComponent<GameState>();
    	//gameplay = gameState.gameplayBegun;

    	//if(gameplay){
	Vector2 mouse = Input.mousePosition;
	leftStick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	//player.Move(leftStick);
	//player.Move(leftStick,new Vector2(mouse.x-Screen.width/2,mouse.y-Screen.height/2).normalized);
	Vector2 plyPos = cam.GetPlayerScreenPosition();
	player.Move(leftStick,new Vector2(mouse.x-plyPos.x,mouse.y-plyPos.y).normalized);

	int attackCall = (int)Input.GetAxis("AttackCall");

	
			
	if (Input.GetButtonDown("Call") || attackCall == -1)
	{
	    Debug.Log("Calling Monsters");
	    player.CallMonsters();
	}

	if(Input.GetButtonDown("Jump")){
	    Debug.Log("JUMPING");
	    player.Jump();
	}

	if(Input.GetButtonDown("PlayerAttack")){
	    Debug.Log("Attack Pressed");
	    player.Attack();
	}

	if(Input.GetButtonDown("Box")){
	    player.GrabDropBox();
	}

	if(Input.GetButtonDown("Dash")){
	    //Debug.Log("DASHING");
	    player.Dash();
	}
			
	// if(Input.GetButtonDown("RunWalkToggle")){
	//     player.WalkToggle();
	//     Debug.Log("Run-Walk Toggled");
	// }
		
	// if (Input.GetButtonDown("debug_spawn"))
	// {
	//     player.SpawnMonster();
	// }

	if (Input.GetButtonDown("Attack") || attackCall == 1)
	{
	    player.AttackMonsters();
	}
	//}	
    }
}


