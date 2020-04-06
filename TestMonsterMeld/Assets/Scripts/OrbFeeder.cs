using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFeeder : OrbPouch
{
    private const int HEAL_AMOUNT = 25;
    private Monster monster;
    
    void Start(){
        monster = GetComponent<Monster>();
    }

    void Update(){
        
    }

    public override void AddOrb(){
	AttackInfo info = new AttackInfo(null,-HEAL_AMOUNT);
	monster.Damage(info);
    }
}
