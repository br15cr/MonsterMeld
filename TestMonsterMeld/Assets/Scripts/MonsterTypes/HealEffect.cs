﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : StatusEffect
{
    protected override void StartEffect(){
	effectName = "Heal";
	//targetMonster = GetComponent<Monster>();
	effectDelay = 1.0f;
    }

    protected override void EffectBehaviour(){
	AttackInfo info = new AttackInfo(null,-1);
	targetMonster.Damage(info);
	if(targetMonster.GetHealth() == 100){
	    EndEffect();
	}
    }
}
