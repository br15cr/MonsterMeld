using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemies : MonoBehaviour
{
    protected MonsterGroup group;

    protected virtual void Start() {
        group = GetComponent<MonsterGroup>();
	//Debug.Log("GROUP IS NULL: " + (group == null) + " CHILD COUNT " + transform.childCount.ToString());
        if (group != null) {
            for (int i = 0; i < this.transform.childCount; i++) {
		GameObject child = transform.GetChild(i).gameObject;
		Monster monst = child.GetComponent<Monster>();
		//Debug.Log(this.name +" Group: "+ group+" Child: "+ child + "\t Monst: " + monst + " "+(i+1).ToString()+ "/" + this.transform.childCount.ToString());
                group.AddMonster(monst);
		if(monst.GetGroup() == null){
		    Debug.LogError("GROUP IS NULL");
		}
            }
        } else {
            Debug.LogError("GROUP IS NULL!");
        }
    }

    protected virtual void Update() {
        
    }
}
