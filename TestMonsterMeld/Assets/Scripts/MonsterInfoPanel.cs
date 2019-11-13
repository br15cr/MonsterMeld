using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPanel : MonoBehaviour
{
    private Text nameText;
    private Text enemyText;
    private Text stateText;
    private RectTransform rect;
    private Image healthBar;
    private float healthBarWidth;
    private Monster monster;
    private Color color;

    public Monster Monster {
	get { return this.monster; }
    }

    public bool followMonster = true;
    
    
    void Start() {
	color = monster.GetGroup().groupColor;
	rect = GetComponent<RectTransform>();
        nameText = transform.Find("NAME_TEXT").GetComponent<Text>();
	nameText.color = color;
	enemyText = transform.Find("DEBUG/ENEMY_TARGET").GetComponent<Text>();
	stateText = transform.Find("DEBUG/STATE_TEXT").GetComponent<Text>();
	healthBar = transform.Find("HEALTH_BACKGROUND/HEALTH_BAR").GetComponent<Image>();
	healthBarWidth = healthBar.GetComponent<RectTransform>().sizeDelta.x;

    }

    void Update() {
        if(monster != null){
	    if(followMonster)
		transform.position = Camera.main.WorldToScreenPoint(monster.transform.position) + Vector3.up*2;
	    healthBar.GetComponent<RectTransform>().sizeDelta = Vector2.right*healthBarWidth*(monster.GetHealth()/100.0f);
	    nameText.text = monster.name;
	    Monster enemy = monster.GetEnemy();
	    if(enemy != null && enemy.GetGroup() != null){
		enemyText.text = enemy.ToString();
		enemyText.color = enemy.GetGroup().groupColor;
	    }else{
		enemyText.text = "null";
		enemyText.color = Color.white;
	    }
	    if(!monster.IsDead){
		if(monster.GetState() == MonsterState.ATTACK){
		    stateText.text = "A: " + monster.GetCombatState().ToString();
		}else{
		    stateText.text = monster.GetState().ToString();
		}
	    }else{
		stateText.text = "DEAD";
	    }
	}
    }

    public void SetMonster(Monster monst){
	monster = monst;
    }
    
}
