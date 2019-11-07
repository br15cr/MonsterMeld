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
    
    protected void Start()
    {
	startTime = Time.time;
    }

    protected void Update()
    {
	if(Time.time >= startTime + lifeTime){
	    EndEffect();
	}

	if(Time.time >= lastEffectTime + effectDelay){
	    EffectBehaviour();
	}
    }

    protected virtual void EndEffect(){
	if(OnEnd != null)
	    OnEnd(this);
	// destroy this effect
    }

    protected virtual void EffectBehaviour(){
	Debug.Log(effectName + ": EFFECT!");
	lastEffectTime = Time.time;
    }

    
}
