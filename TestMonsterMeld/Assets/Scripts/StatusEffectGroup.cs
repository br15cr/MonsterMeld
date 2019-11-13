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
	// otherwise add it
    }


    void Update() {
        
    }
}
