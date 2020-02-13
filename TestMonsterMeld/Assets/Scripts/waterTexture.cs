using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterTexture : MonoBehaviour
{
   // Scroll main texture based on time

    public float scrollSpeed = 0.5f;
    public bool waterDirection_1 = false;
    public bool waterDirection_2 = false;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer> ();
    }

    void Update()
    {
    	if(waterDirection_1){
	        float offset = Time.time * scrollSpeed;
	        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));    		
    	}else if(waterDirection_1){
	        float offset = Time.time * scrollSpeed;
	        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));    		
    	}
    	else{    		
	        float offset = Time.time * scrollSpeed;
	        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));   
    	}

    }
}