// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameState : MonoBehaviour
// {

//     public int gs_begin;
//     //Sequence_WalkOutOfHouse
//     public gameobject location_house;
//     public gameobject gs_convo_mom_FromGameObject;
//     public gameobject gs_convo_mom_ToGameObject;
//     public int gs_convo_mom;
//     public int gs_tutorial_Basics;
//     public int tutorial_test;
//     public int gs_professorsLab;
//     public int gs_tutorial_Crafting;
//     public int gs_boss_begin;
//     public int gs_boss_phase1;
//     public int gs_boss_phase2;
//     public int gs_boss_defeated;
//     public int gs_player_defeated;

//     public gameobject gate_1; //village Fence
//     public gameobject gate_2;
//     public gameobject gate_3;



//     public void Sequence_WalkOutOfHouse(){
// 	//port player to 
// 	//location_house;

// 	//Camera port to pan pos.

// 	//Dialog UI from file? "Mom convo"

// 	//Camera pan of Village
// 	//camera.pan(gs_convo_mom_FromGameObject, gs_convo_mom_ToGameObject);
// 	gs_convo_mom = true;
//     }
//     public void Tutorial_Basics(){
// 	// Display and test player on the controls
// 	// Once they have commanded a minion to attack an object tutorial complete.
// 	// Explain Auto Checkpoint
// 	if (tutorial_test){
// 	    gs_tutorial_Basics = true;
// 	}
	
//     }

//     public void Sequence_ProfessorsLab(){
// 	//Dialog
// 	//Camera pan of ruins
// 	// give items AKA the bool of this convo is used to check if you have unlocked run speed.
// 	// TO DO: add int on player for, CanRun and CanDash
// 	// UI minimap arrow for the assigned hint, "look for the cave"

// 	gs_professorsLab =  true;
//     }

//     public void Tutorial_Crafting(){
// 	if (tutorial_test){
// 	    gs_tutorial_Crafting = true;
// 	}
//     }

//     public void Sequence_BossEncounter(){
// 	//pan to animation of prof under attack
// 	gs_boss_begin;
// 	// phase one attack patterns
// 	gs_boss_phase1;

// 	//camera pan to boss moving into a second area
// 	gs_boss_phase2;

// 	if(bossHP <= 0){
// 	    gs_boss_defeated;
// 	}
// 	if(PlayerHP <= 0){
// 	    gs_player_defeated;
// 	}


//     }

//     public void GateControl(prefabName){
// 	//Set this prefab group of clipboxes to inactive
// 	//Pan camera toward this object to hint the player
//     }
//     public void SaveState(){

//     }
//     public void loadState(){

//     }




//     // Start is called before the first frame update
//     void Start()
//     {



//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //BEGIN DEMO
//         Sequence_WalkOutOfHouse();
//         //OnTriggerEnter tutorialZone
//         Tutorial_Basics();
//         //Gate: fence to the professors lab unlocked
//         gateControl(gate_1);

//         //BEGIN QUEST
//         //OnTriggerEnter labZone
//         Sequence_ProfessorsLab();
//         //Gate: Village doors opened
//         gateControl(gate_2);

//         //FIND CAVE AND BOX
//         Tutorial_Crafting();
//         //complete quest

//         //Gate: Party Size Requirement to climb up you minions
//         gateControl(gate_3);

//         //BOSS ENCOUNTER
//         //Reset on player death, ports player back to right before dropping into the boss fight arena.
//         SaveState();
//         //lock into boss fight arena

//         //PHASE 1 of Boss encounter      
//         gateControl(gate_4); //Enable: locked in from both sides
//         Sequence_BossEncounter();
//         gateControl(gate_5);


//         if (gs_player_defeated){
// 	    loadState();
//         }
//     }
// }
