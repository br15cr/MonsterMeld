using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private MonsterAttackInfo info;
    private Monster attacker;

    // lifetime
    
    void Start() {
        info = new MonsterAttackInfo((attacker != null ? attacker : null),20);
    }

    void Update() {
        
    }

    public void SetAttacker(Monster a){
	attacker = a;
    }

    // disable this collision until enabled by user?
    void OnTriggerEnter(Collider c){
	Monster monster = c.GetComponent<Monster>();
	//Debug.Log(c.transform.name + " ENTERED TRIGGER!");
	// damage any monsters that isn't the attacker or on the attacker's team
	if(monster != null){
	    // damage monster
	    // destroy self
	    if(attacker != null){
		if(monster == attacker){
		    Debug.Log(monster.name + " tried to attack themself!");
		    return;
		}
		if(attacker.GetGroup() == monster.GetGroup()){
		    Debug.Log(monster.name + " tried to attack their teammates!");
		    return;
		}
	    }
	    Debug.Log("MONSTER ENTERED TRIGGER");
	    monster.TakeDamage(info);
	    Destroy(this.gameObject);
	}
    }
}
