﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : StatusEffect
{
    private int damage = 2;
    
    protected override void StartEffect(){
	effectName = "Burn";
	//targetMonster = GetComponent<Monster>();
	effectDelay = 0.5f;
	lifeTime = 10.0f;
    }
    
    protected override void EffectBehaviour(){
	AttackInfo info = new AttackInfo(null,damage);
	targetMonster.Damage(info);
    }
}
