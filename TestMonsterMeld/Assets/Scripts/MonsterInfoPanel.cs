using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPanel : MonoBehaviour
{
    private Text nameText;
    private RectTransform rect;
    private Image healthBar;
    private float healthBarWidth;
    private Monster monster;
    private Color color;

    public Monster Monster {
	get { return this.monster; }
    }
    
    
    void Start() {
	color = monster.GetGroup().groupColor;
	rect = GetComponent<RectTransform>();
        nameText = transform.Find("NAME_TEXT").GetComponent<Text>();
	nameText.color = color;
	healthBar = transform.Find("HEALTH_BACKGROUND/HEALTH_BAR").GetComponent<Image>();
	healthBarWidth = healthBar.GetComponent<RectTransform>().sizeDelta.x;

    }

    void Update() {
        if(monster != null){
	    transform.position = Camera.main.WorldToScreenPoint(monster.transform.position) + Vector3.up*2;
	    healthBar.GetComponent<RectTransform>().sizeDelta = Vector2.right*healthBarWidth*(monster.GetHealth()/100.0f);
	    nameText.text = monster.name;
	}
    }

    public void SetMonster(Monster monst){
	monster = monst;
    }
    
}
