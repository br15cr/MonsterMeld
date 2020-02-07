using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTestEnemies : TestEnemies
{
    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
	if(Input.GetKeyDown("p")){
	    group.CreateMonster();
	    //m.agro = true;
	    
	}
        base.Update();
    }
}
