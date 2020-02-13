using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayerClips : MonoBehaviour
{

	public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
         ForeachLoop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ForeachLoop()
    {
        foreach (Transform child in transform){
            rend = GetComponent<Renderer>();
        	rend.enabled = false;
        }
    }
}
