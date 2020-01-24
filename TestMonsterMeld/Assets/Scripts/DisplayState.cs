using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayState : MonoBehaviour
{
    private int stateIndex = 0;
    private SpriteRenderer sprite;
    public Monster monster;

    public Sprite[] icons = new Sprite[7];
    
    void Start() {
	sprite = GetComponent<SpriteRenderer>();
	if(monster.GetGroup()!= null){
	    sprite.flipX = !(monster.GetGroup().IsPlayerGroup);
	}else{
	    sprite.flipX = true;
	}

	monster.OnStatesChanged += StateChanged;
    }

    void Update() {
	transform.LookAt(Camera.main.transform,Camera.main.transform.up);
    }

    void StateChanged(Monster m,MonsterState state,MonsterCombatState combatState){
	int st = (int)state;
	int cst = (int)combatState;
	int stateNum = st == 2 ? st+cst : st;
	int currentState = GetMonsterState();
	if(stateIndex != currentState){
	    //Debug.Log("State change from " + stateIndex.ToString() + " to " + currentState.ToString());
	    stateIndex = currentState;
	    sprite.sprite = icons[stateIndex];
	}
    }

    private int GetMonsterState(){
	int state = (int)monster.GetState();
	int cstate = (int)monster.GetCombatState();
	return state == 2 ? state+cstate : state;
    }
}
