using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayState : MonoBehaviour
{
    private int stateIndex = 0;
    private SpriteRenderer sprite;
    public Monster monster;

    public Sprite[] icons = new Sprite[8];

    private bool hitting = false; // show the icon for a hit
    
    void Start() {
	sprite = GetComponent<SpriteRenderer>();
	if(monster.GetGroup()!= null){
	    sprite.flipX = !(monster.GetGroup().IsPlayerGroup);
	}else{
	    sprite.flipX = true;
	}

	if(monster.agro){
	    //StateChanged(monster,monster.GetState(),monster.GetCombatState());
	    StateChanged(monster,monster.GetState());
	}
	monster.OnStatesChanged += StateChanged;
    }

    void Update() {
	transform.LookAt(Camera.main.transform,Camera.main.transform.up);
	if(hitting){
	    int currentState = (int)monster.GetState();
	    if(currentState < 4 || currentState > 5)
		StopHit();
	}
    }

    void StateChanged(Monster m,MonsterState state)
    {
	int st = (int)state;
	//int cst = (int)combatState;
	//int stateNum = st == 2 ? st+cst : st;
	int stateNum = st;
	int currentState = GetMonsterState();




	if(!hitting){
	    if(stateIndex != currentState){
		stateIndex = currentState;
		try {
		    int index = stateIndex;
		    if(index == 4)
			index = 5;
		    sprite.sprite = icons[index];
		} catch (System.IndexOutOfRangeException e){
		    Debug.LogError("stateIndex out of Sprite array range ("+icons.Length.ToString()+"). stateIndex is " + stateIndex.ToString()+".\nMonster State: " + state.ToString(),this);
		}
	    }
	}
    }

    public void StartHit(){
	hitting = true;
	sprite.sprite = icons[4];
	StateChanged(monster,monster.GetState());
    }

    public void StopHit(){
	hitting = false;
	stateIndex = -1;
	StateChanged(monster,monster.GetState());
    }

    private int GetMonsterState(){
	int state = (int)monster.GetState();
	// int cstate = (int)monster.GetCombatState();
	// if(state == 3)
	//     return 7;
	// return state == 2 ? state+cstate : state;
	return state;
    }
}
