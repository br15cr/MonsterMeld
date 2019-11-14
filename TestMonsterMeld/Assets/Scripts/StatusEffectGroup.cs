using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectGroup : MonoBehaviour
{
    private List<StatusEffect> effects = new List<StatusEffect>();
    
    void Start() {
        
    }

    public void AddEffect(StatusEffect effect){
	// check if effects already contains the effect, if it does just reset the lifetime
	// subscribe to the OnEnd event
	// otherwise add it
	effect.SetTarget(this.GetComponent<Monster>());
	effect.OnEnd += EffectEnd;
	effects.Add(effect);
	effect.Begin();
    }


    void Update() {
        
    }

    private void EffectEnd(StatusEffect effect){
	// remove effect from the list
	effects.Remove(effect);
    }
}
