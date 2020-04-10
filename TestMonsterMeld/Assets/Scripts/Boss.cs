using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Miniboss
{
    void Start()
    {
	base.Start();
	attackDamage = 50;
	attackDelay = 3.0f;
    }
}
