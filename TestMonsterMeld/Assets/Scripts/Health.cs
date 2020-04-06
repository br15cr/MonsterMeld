using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInfo {
    public float baseDamage;
    //public Monster attacker;
    public HealthUser attacker;
    
    public AttackInfo(HealthUser Attacker,float BaseDamage){
	baseDamage = BaseDamage;
	attacker = Attacker;
    }

    public HealthData Damage(HealthData health){
	// Apply damage formula (eg: baseDamage*weight)...etc
	HealthData hd = new HealthData(health.GetHealth()-baseDamage,health.GetMaxHealth());
	return hd;
    }
}

// full info on the attack, including the defender
public struct AttackInstanceInfo {
    //public AttackInfo attackInfo;
    public float baseDamage;
    public HealthUser attacker;
    public HealthUser defender;
    public bool fatalAttack;

    public AttackInstanceInfo(HealthUser Attacker, float BaseDamage, HealthUser Defender){
	//attackInfo = new AttackInfo(Attacker,BaseDamage);
	attacker = Attacker;
	baseDamage = BaseDamage;
	defender = Defender;
	fatalAttack = false;
    }

    public AttackInstanceInfo(AttackInfo prevInfo, HealthUser Defender,bool Died){
	//attackInfo = prevInfo;
	baseDamage = prevInfo.baseDamage;
	attacker = prevInfo.attacker;
	defender = Defender;
	fatalAttack = Died;
    }

    
}

public struct HealthData {
    private float health;
    private float maxHealth;

    public HealthData(float Health,float MaxHealth){
	maxHealth = MaxHealth;
	health = 0;
	SetHealth(Health);
    }

    private void SetHealth(float value){
	if(value < 0){
	    health = 0;
	}else if(value > maxHealth){
	    health = maxHealth;
	}else{
	    health = value;
	}
    }

    public float GetHealth(){
	return health;
    }

    public float GetMaxHealth(){
	return maxHealth;
    }

    public static HealthData operator +(HealthData a,float b){
	HealthData hd = new HealthData(a.GetHealth()+b,a.GetMaxHealth());
	return hd;
    }

    public static HealthData operator -(HealthData a,float b){
	HealthData hd = new HealthData(a.GetHealth()-b,a.GetMaxHealth());
	return hd;
    }

    public static float operator /(HealthData a, float b){
	return (a.GetHealth()/b);
    }

    public static bool operator ==(HealthData a,float b){
	return a.GetHealth() == b;
    }

    public static bool operator !=(HealthData a,float b){
	return a.GetHealth() != b;
    }

    public static HealthData operator -(HealthData a,AttackInfo b){
	//HealthData hd = new HealthData(a.GetHealth()-b.baseDamage,a.GetMaxHealth());
	return b.Damage(a);
    }
}

public delegate void AttackInfoDelegate(AttackInfo info); // add attacker to parameters
public delegate void AttackInstanceInfoDelegate(AttackInstanceInfo info);

public class HealthUser : MonoBehaviour
{
    //private int health;
    //private float health;
    //public float maxHealth = 100;
    protected HealthData health;
    protected bool isDead = false;

    public event AttackInstanceInfoDelegate OnDeath;

    public float startHealth = 100;
    
    public float Amount {
	get { return this.health.GetHealth(); }
    }

    public bool IsDead {
	get { return this.isDead; }
    }
    
    protected void Start(){
        //health = maxHealth;
	health = new HealthData(startHealth,startHealth);
    }

    void Update(){
        
    }

    /*
    private void SubHealth(float amount){
	if(health - amount <= 0){
	    health = 0;
	    // call death event
	}else if(health - amount > maxHealth){
	    health = maxHealth;
	}else{
	    health -= amount;
	}
    }
    */

    public virtual void Damage(AttackInfo attackInfo){
	//SubHealth(attackInfo.baseDamage);
	health -= attackInfo;
	CheckDeath(attackInfo);
    }

    // public void Damage(float amount){
    // 	health -= amount;
    // 	CheckDeath();
    // }

    public virtual MonsterGroup GetGroup(){
	return null;
    }

    private void CheckDeath(AttackInfo attackInfo){
	if(health == 0){
	    if(!isDead){
		isDead = true;
		if(OnDeath != null)
		    OnDeath(new AttackInstanceInfo(attackInfo,this,true));
		Die(attackInfo);
	    }
	}
    }

    protected virtual void Die(AttackInfo finalBlow){
	Destroy(this.gameObject);
    }

    public virtual bool IsMonster(){
	return false;
    }

    public virtual bool IsPlayer(){
	return false;
    }

    /*
    public float GetFraction(){
	return health/maxHealth;
    }
    */
}
