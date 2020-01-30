using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Button1 : MonoBehaviour
{
	public bool button_hover;
    // Start is called before the first frame update
    void Start()
    {
        button_hover = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
    	button_hover = true;
    	Debug.Log("HELLO WORLD");
    }
}
