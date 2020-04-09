using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public Sprite Button_continue_Hover;
    public Sprite Button_continue_Desel;
    public Sprite Button_reload_Hover;
    public Sprite Button_reload_Desel;
    public Sprite Button_exit_Hover;
    public Sprite Button_exit_Desel;

    public Player player;

    public GameObject button_continue;
    public GameObject button_reload;
    public GameObject button_exit;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public int pauseMenu_Pos;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetButtonDown("Pause")){
	    if (GameIsPaused)
	    {
		Resume();
	    } else 
	    {
		Pause();
	    }
        }



	// if (Input.GetKeyDown("down"))
	// {
	//     if(pauseMenu_Pos < 2)
	//     {
	// 	pauseMenu_Pos += 1;
	//     }
	//     else
	//     {
	// 	pauseMenu_Pos = 0;
	//     }
	// }
	// if (Input.GetKeyDown("up"))
	// {
	//     if(pauseMenu_Pos > 0)
	//     {
	// 	pauseMenu_Pos -= 1;
	//     }
	//     else
	//     {
	// 	pauseMenu_Pos = 2;
	//     }

	// }


    	// if (pauseMenu_Pos == 0)
    	// {
	//     if (Input.GetKeyDown("space") || Input.GetKeyDown("return") ) 
	//     {
	// 	Resume ();
	//     }
    		
	//     button_continue.GetComponent<Image> ().sprite = Button_continue_Hover;
	//     button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
	//     button_reload.GetComponent<Image> ().sprite = Button_reload_Desel;

	//     clearSel();
    	// }
    	// else if (pauseMenu_Pos == 1) 
    	// {
	//     if (Input.GetKeyDown("space") || Input.GetKeyDown("return") ) 
	//     {
	// 	LoadMenu ();
	//     }
	//     button_reload.GetComponent<Image> ().sprite = Button_reload_Hover;
	//     button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
	//     button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
	//     clearSel();
    	// }
    	// else if (pauseMenu_Pos == 2) 
    	// {
	//     if (Input.GetKeyDown("space") || Input.GetKeyDown("return") ) 
	//     {
	// 	QuitGame ();
	//     }
	//     button_exit.GetComponent<Image> ().sprite = Button_exit_Hover;
	//     button_reload.GetComponent<Image> ().sprite = Button_reload_Desel;
	//     button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
	//     clearSel();
    	// }






    }

    void clearSel (){
    	//button_exit.GetComponent<Image> ().sprite = Button_exit_Desel;
       	//button_reload.GetComponent<Image> ().sprite = Button_reload_Desel;
        //button_continue.GetComponent<Image> ().sprite = Button_continue_Desel;
    }
    public void Resume (){
    	pauseMenuUI.SetActive(false);
    	Time.timeScale = 1f;
    	GameIsPaused = false;
    }

    public void resetStats (){
    	Time.timeScale = 1f;
    	GameIsPaused = false;
    }

    void Pause (){
    	pauseMenuUI.SetActive(true);
    	Time.timeScale = 0f;
    	GameIsPaused = true;
    	//STOP ALL PLAYER MOVEMENT, Like when hitting buttons at the same time as pressing the pause button.

    	// Reset time scale and is paused when reloading
    }
    
    public void LoadMenu (){
    	//Loading Menu could be added here. 
    	Debug.Log("Loading Menu");

    }

    public void QuitGame (){
    	resetStats();
	
    	SaveGame(player);
    	Debug.Log("Quitting to menu");
    	SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame (Player player){
	//player.GetComponent<player>().SavePlayer();
	Debug.Log("PauseMenu: Is Player Null? " + (player == null).ToString());
    	Debug.Log("Saving..");
    	//SaveSystem.SavePlayer(player);

    }


}
