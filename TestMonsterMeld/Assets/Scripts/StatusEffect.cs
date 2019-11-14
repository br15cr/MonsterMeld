using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StatusEffectDelegate(StatusEffect effect);

public class StatusEffect : MonoBehaviour
{

    protected float startTime;
    protected float lastEffectTime;
    

    public string effectName;
    public float lifeTime;
    public float effectDelay;
    public Monster targetMonster;

    public event StatusEffectDelegate OnEnd;

    void Start() {}

    public void Begin(){ // a controlled way for the StatusEffectGroup to start the effect
	startTime = Time.time;
	StartEffect();
    }

    protected virtual void StartEffect(){
	
    }

    protected void Update()
    {
	if(Time.time >= startTime + lifeTime){
	    EndEffect();
	}

	if(Time.time >= lastEffectTime + effectDelay){
	    RunEffect(); //EffectBehaviour();
	}
    }

    public void ResetLifetime(){
	startTime = Time.time;
    }

    protected virtual void EndEffect(){
	if(OnEnd != null)
	    OnEnd(this);
	Destroy(this);
	// destroy this effect
    }

    private void RunEffect(){
	EffectBehaviour();
	//Debug.Log(effectName + ": EFFECT!");
	lastEffectTime = Time.time;
    }

    protected virtual void EffectBehaviour(){
    }

    public void SetTarget(Monster monster){
	targetMonster = monster;
    }

    
}
