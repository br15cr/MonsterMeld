using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Sprite Button_newGame_Hover;
    public Sprite Button_newGame_Desel;
    public Sprite Button_continue_Hover;
    public Sprite Button_continue_Desel;
    public Sprite Button_option_Hover;
    public Sprite Button_option_Desel;
    public Sprite Button_exit_Hover;
    public Sprite Button_exit_Desel;

   	public GameObject button_newGame;
   	public GameObject button_option;
   	public GameObject button_continue;
   	public GameObject button_exit;

	public string newGameScene;
	public int menu_Pos;
    // Start is called before the first frame update
    void Start()
    {
		button_newGame.GetComponent<Image> ().sprite = Button_newGame_Hover;
    }

//DO THIS TODAY!! add sub menu for saves and options, and add ui in game
// store menu position to an int and then set that button to selected.
// on enter button hit use if statement to execute the correct menu function.



    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("down"))
		{
			if(menu_Pos < 3)
			{
				menu_Pos += 1;
			}
			else
			{
				menu_Pos = 0;
			}
		}
		if (Input.GetKeyDown("up"))
		{
			if(menu_Pos > 0)
			{
				menu_Pos -= 1;
			}
			else
			{
				menu_Pos = 3;
			}

		}


    	if (menu_Pos == 0)
    	{
    		    if (Input.GetKeyDown("space") || Input.GetKeyDown("return") ) 
        		{
           			Continue();
        		}
    		
    		button_continue.GetComponent<Image> ().sprite = Button_continue_Hover;
    		button_newGame.GetComponent<Image> ().sprite = Button_newGame_Desel;
        	button_option.GetComponent<Image> ().sprite = Button_option_Desel;
         	button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
    	}
    	else if (menu_Pos == 1) 
    	{
    		    if (Input.GetKeyDown("space") || Input.GetKeyDown("return") ) 
        		{
           			NewGame();
        		}
			button_newGame.GetComponent<Image> ().sprite = Button_newGame_Hover;
        	button_option.GetComponent<Image> ().sprite = Button_option_Desel;
         	button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
         	button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
    	}

    	else if (menu_Pos == 2) 
    	{
			button_option.GetComponent<Image> ().sprite = Button_option_Hover;
			button_newGame.GetComponent<Image> ().sprite = Button_newGame_Desel;
         	button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
         	button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
    	}
    	else if (menu_Pos == 3) 
    	{
			button_exit.GetComponent<Image> ().sprite = Button_exit_Hover;
			button_newGame.GetComponent<Image> ().sprite = Button_newGame_Desel;
        	button_option.GetComponent<Image> ().sprite = Button_option_Desel;
         	button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
    	}


    	Debug.Log(menu_Pos);
		Debug.Log("Menu: Continue");


      // bool isHover = button_newGame.GetComponent<MainMenu_Button1>().button_hover;

    //  if(isHover){
     //  	button_newGame.GetComponent<Image> ().sprite = Button_newGame_Hover;
      // 	button_option.GetComponent<Image> ().sprite = Button_option_Hover;
      // 	button_continue.GetComponent<Image> ().sprite = Button_continue_Hover;
     //  	button_exit.GetComponent<Image> ().sprite = Button_exit_Hover;
      // }

    //   Debug.Log(isHover);
    }


    public void Continue()
    {
    	//call the save method from some sort of persistent game object between scenes
    	SceneManager.LoadScene(newGameScene);
    }
    public void NewGame()
    {
    	SceneManager.LoadScene(newGameScene);
    }
    public void QuitGame()
    {
    	Application.Quit();
    }
    public void openOptions()
    {

    }
    public void closeOptions()
    {

    }

}
