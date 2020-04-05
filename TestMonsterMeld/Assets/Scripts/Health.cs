using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInfo {
    public float baseDamage;
    //public Monster attacker;
    public HealthUser attacker;
    public AttackInfo(float BaseDamage,HealthUser Attacker){
	baseDamage = BaseDamage;
	attacker = Attacker;
    }

    public HealthData Damage(HealthData health){
	// Apply damage formula (eg: baseDamage*weight)...etc
	HealthData hd = new HealthData(health.GetHealth()-baseDamage,health.GetMaxHealth());
	return hd;
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

public delegate void AttackInfoDelegate(); // add attacker to parameters

public class HealthUser : MonoBehaviour
{
    //private int health;
    //private float health;
    //public float maxHealth = 100;
    protected HealthData health;
    protected bool isDead = false;

    public event AttackInfoDelegate OnDeath;

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

    public void Damage(AttackInfo attackInfo){
	//SubHealth(attackInfo.baseDamage);
	health -= attackInfo;
	CheckDeath();
    }

    public void Damage(float amount){
	health -= amount;
	CheckDeath();
    }

    private void CheckDeath(){
	if(health == 0){
	    if(!isDead){
		isDead = true;
		if(OnDeath != null)
		    OnDeath();
	    }
	}
    }

    /*
    public float GetFraction(){
	return health/maxHealth;
    }
    */
}
