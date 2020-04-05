using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss : Monster
{
    // the distance the miniboss gives up chasing an enemy
    private float outOfRangeDistance = 5.0f;
    
    protected override void Start(){
        base.Start();
	attackDamage = 30;
	attackDelay = 1.5f;
    }

    public override void TakeDamage(MonsterAttackInfo attackInfo)
    {
	base.TakeDamage(attackInfo);
	if(attackInfo.attacker != null){
	    Monster attacker = attackInfo.attacker;
	    if((transform.position - enemyTarget.transform.position).magnitude > outOfRangeDistance){
		AttackMonster(attacker);
	    }
	}
    }
}
