﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RecipeItem {
    ORB
}

public struct RecipeIngredient {
    public RecipeItem item;
    public int amount;
    public RecipeIngredient(RecipeItem Item,int Amount){
	item = Item;
	amount = Amount;
    }
}

public struct Recipe {
    public List<RecipeIngredient> ingredients;
    public Recipe(params RecipeIngredient[] Ingredients){
	ingredients = new List<RecipeIngredient>();
	for(int i = 0;i < Ingredients.Length;i++){
	    ingredients.Add(Ingredients[i]);
	}
    }
}


public class FusionBox : MonoBehaviour
{
    
    private MonsterGroup group;
    private Recipe monsterRecipe;

    public float useRadius = 1.0f;
    public Player player;
    public GameObject monsterPrefab;
    public Vector3 spawnOffset;

    void Start(){
        group = player.GetComponent<MonsterGroup>();
	monsterRecipe = new Recipe(new RecipeIngredient(RecipeItem.ORB,10));
    }

    void Update(){
	if(Input.GetButtonDown("debug_spawn")){
	    if(Vector3.Distance(transform.position,player.transform.position) <= useRadius){
		if(player.CanMakeRecipe(monsterRecipe)){
		    player.TakeRecipe(monsterRecipe);
		    Debug.Log("Created Monster");
		    CreateMonster();
		}else{
		    Debug.Log("Don't Have the Ingredients");
		}
	    }
	}
    }

    void CreateMonster(){
	// Monster monster = GameObject.Instantiate(monsterPrefab).GetComponent<Monster>();
	// monster.transform.position = transform.position+spawnOffset;
	// Debug.Log(spawnOffset);
	// group.AddMonster(monster);
	group.CreateMonster(monsterPrefab,transform.position+spawnOffset);
	//monster.Warp(transform.position+spawnOffset);
    }
}
