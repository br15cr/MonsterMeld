using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private List<string> logs = new List<string>();
    
    void Start() {
        
    }

    void Update() {
        
    }

    public void Log(string message){
	Debug.Log(message);
	logs.Add(message);
    }
}
