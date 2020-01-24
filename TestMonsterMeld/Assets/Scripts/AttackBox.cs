using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private MonsterAttackInfo info;
    private Monster attacker;

    private bool hasInfo = false;
    private bool attacking = false;

    // lifetime

    private float timeStart;

    private const float LIFE_TIME = 0.25f;
    
    void Start() {
        //info = new MonsterAttackInfo((attacker != null ? attacker : null),20);
	timeStart = Time.time;
    }

    void Update() {
        if(Time.time > timeStart + LIFE_TIME)
	    Destroy(this.gameObject);
    }

    public void SetAttacker(Monster a){
	attacker = a;
    }

    public void SetInfo(MonsterAttackInfo i){
	this.GetComponent<Collider>().enabled = true;
	info = i;
	hasInfo = true;
    }

    // disable this collision until enabled by user?
    void OnTriggerEnter(Collider c){
	// don't attempt to attack without knowing the attack info
	attacking = true;
	if(!hasInfo){
	    attacking = false;
	    return;
	}
	
	Monster monster = c.GetComponent<Monster>();
	//Debug.Log(c.transform.name + " ENTERED TRIGGER!");
	// damage any monsters that isn't the attacker or on the attacker's team
	
	if(monster != null){
	    // damage monster
	    // destroy self
	    if(monster.Equals(info.attacker)){
		Debug.Log(monster.name + " tried to attack themself!");
		attacking = false;
		return;
	    }
	    if(info.attacker != null && info.attacker.GetGroup() != null){
		if(info.attacker.GetGroup().Equals(monster.GetGroup())){
		    Debug.Log(monster.name + " tried to attack their teammates!");
		    attacking = false;
		    return;
		}
	    }
	    Debug.Log("MONSTER ENTERED TRIGGER");
	    monster.TakeDamage(info);
	    Destroy(this.gameObject);
	}
    }
}
