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
	sprite.flipX = !(monster.GetGroup().IsPlayerGroup);
	if(monster.GetGroup().IsPlayerGroup){
	    Debug.Log("SPRITE FOR PLAYER GROUP!!!! " + sprite.flipX.ToString());
	}
    }

    void Update() {
	int currentState = GetMonsterState();
	if(stateIndex != currentState){
	    //Debug.Log("State change from " + stateIndex.ToString() + " to " + currentState.ToString());
	    stateIndex = currentState;
	    sprite.sprite = icons[stateIndex];
	}
	transform.LookAt(Camera.main.transform,Camera.main.transform.up);
    }

    private int GetMonsterState(){
	int state = (int)monster.GetState();
	int cstate = (int)monster.GetCombatState();
	return state == 2 ? state+cstate : state;
    }
}
