using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Transform mainCam;
    private GameObject cam;
    private CamScript camScript;
    private bool cameraPanZoom;
    private bool cameraMoveFor;

    public float introTime = 1f;
    private float timeS = 1f;
    private float introStart = 1f;
    public bool introPlayed;

    public float cutTime = 1f;
    private float cutTimeStore = 1f;
    private float cutStart = 1f;
    public bool timeTested = false;
    public float checkedTime = 1f;



    //Sequence_00
    public bool gameplayBegun;
    public bool Sequence_00_Ended = false;
    //public float Sequence_00_length = 1f;
    
    //

    //Sequence_01_WalkOutOfHouse
    public bool Sequence_01_Began = false;
    public GameObject Sequence_01_target;
	public bool Sequence_01_Ended = false;
	public float Sequence_01_length = 1f;
    //

    //TEST SEQUENCE 
    //Sequence_02_Tutorial_1
    public bool Sequence_02_Began = false;
	public GameObject Sequence_02_clipgate;
    public GameObject Sequence_02_target;
    public bool Sequence_02_Ended = false;
    public float Sequence_02_length = 1f;

    public Monster gateMonster1;
    //

    //Sequence_03_Professor
    public bool Sequence_03_Began = false;
    public GameObject Sequence_03_clipgate;
    public GameObject Sequence_03_target;
	public bool Sequence_03_Ended = false;
	public float Sequence_03_length = 1f;
    //

    //Sequence_04_Tutorial_2
    public bool Sequence_04_Began = false;
    public GameObject Sequence_04_clipgate;
    public GameObject Sequence_04_target;
	public bool Sequence_04_Ended = false;
	public float Sequence_04_length = 1f;


    public int monsterCountReq =1;
    public Player player;
    //

    //Sequence_05_BOSS
    public bool Sequence_05_Began = false;
    public GameObject Sequence_05_clipgate;
    public GameObject Sequence_05_clipgate_2;
    public GameObject Sequence_05_clipgate_3;
    public GameObject Sequence_05_target;
	public bool Sequence_05_Ended = false;
	public float Sequence_05_length = 1f;

    public Monster bossMonster;
    //



    void Start()
    {
    	// Use GetComponent to access the camera
        //Camera cam = gameObject.GetComponent<Camera>();
        //camScript = cam.Z_testScript;
        camScript = mainCam.GetComponent<CamScript>();
    }

    void Update(){

    	//INTRO TIMER
    	//timeS = Time.time;
    	if(!Sequence_00_Ended)
		{
			introTime -= Time.deltaTime;
			Sequence_00_Begin();
		}
		//Debug.Log("Time Started: " + timeS + " Intro Start: " + introStart + " Intro Time: " + introTime );
		
		if(Sequence_01_Began)
		{
			Sequence_01_House();
		}

        Sequence_02_Tutorial_1();

        if (Sequence_03_Began)
		{
			Sequence_03_Prof();
		}
		if(Sequence_03_Began)
		{
			Sequence_04_Tutorial_2();
		}
		if(Sequence_05_Began)
		{
			Sequence_05_Boss();
		}

		cutStart += Time.deltaTime;
		//Debug.Log("Cutscene Timer: " + cutStart);

    	
    }
	public void setTimers(){

    	//cutStart = Time.time;
    	cutStart = 0f;
	}

	public bool timeCheck(float cutTime){

		if(cutStart >= cutTime){
		Debug.Log("Checked Time" + checkedTime);

			return true;
		}else {
			return false;
		}
	}

	public void gameTriggerActivated(string trigName){
		//Debug.Log ("Game Event Triggered: " + trigName);
		triggerFilter(trigName);
	}

	public void triggerFilter(string trigName){
		//Debug.Log("Trigger filter: " + trigName);
		if (trigName == "Gate_1"){
			Gated_1();
		}
		if (trigName == "Gate_2"){
			Gated_2();
		}
		if (trigName == "Gate_3"){
			Gated_3();
		}
		if (trigName == "Gate_4"){
            Sequence_BossLocked();

            Gated_4();
		}
		if (trigName == "Gate_5"){
			//Gated_5();
		}


		if (trigName == "Sequence_00_trig"){
			Sequence_00_Begin();
		}
		if (trigName == "Sequence_01_trig"){
			Sequence_01_House();
		}
		if (trigName == "Sequence_02_trig"){
			Sequence_02_Tutorial_1();
		}
		if (trigName == "Sequence_03_trig"){
			Sequence_03_Prof();
		}
		if (trigName == "Sequence_04_trig"){
			Sequence_04_Tutorial_2();
		}
		if (trigName == "Sequence_05_trig"){
			Sequence_05_Boss();
		}


	}

	public void Gated_1(){
		Debug.Log("You must complete all activites in this area before moving forward.");
	}
	public void Gated_2(){
		Debug.Log("You must complete all activites in this area before moving forward.");
	}
	public void Gated_3(){
		Debug.Log("You must complete all activites in this area before moving forward.");
	}
	public void Gated_4(){
		Debug.Log("You must complete all activites in this area before moving forward.");
	}

	/* public void Sequence_00_Begin(){

		if(!Sequence_01_Began)
		{
			setTimers();
		}
		Sequence_01_Began = true;
					    				
		if(!Sequence_00_Ended)
		{		
			Debug.Log("Welcome to Monster Meld");
			gameplayBegun = true;
			Sequence_00_Ended = true;
		}

	}*/
	public void Sequence_00_Begin(){
			if(!Sequence_00_Ended)
			{

				if (!introPlayed){
					//setTimers();
					if (introTime <= 0)
        			{
						introPlayed = true;

    				}
/*
    				
    				if(timeS > introStart + introTime){
						introPlayed = true;
    				}*/
				}else{

					Debug.Log("Welcome to Monster Meld");

					gameplayBegun = true;
					Sequence_00_Ended = true;
				}

			}

		}
	public void Sequence_01_House(){
		if(!Sequence_01_Began)
		{
			setTimers();
		}
		Sequence_01_Began = true;

		if(!Sequence_01_Ended)
		{

			gameplayBegun = false;
			timeTested = timeCheck(Sequence_01_length);
			//Debug.Log("time tested" + timeTested);
			if (timeTested)
        	{
				Sequence_01_Ended = true;
				timeTested = false;
    		}
			cameraPanZoom = false;
			cameraMoveFor = false;
			//camScript.triggerCutscene();
			camScript.triggerCutscene(Sequence_01_target, Sequence_01_length, cameraPanZoom, cameraMoveFor);
			//cutscene_test_clipgate.SetActive(false);

			//Debug.Log("Execute Sequence");
		}else{

			//Debug.Log("Welcome to Monster Meld");

			gameplayBegun = true;
		}
	}


	public void Sequence_02_Tutorial_1(){
        //CHECK IF Player beat tutorial monster

        if (gateMonster1.IsDead)
        { 
        
		    if(!Sequence_02_Began)
		    {
			    setTimers();
		    }
		    Sequence_02_Began = true;

		    cameraPanZoom = true;
		    //cutsceneLength = 5f;

		    //Camera pan of Village
		    //camera.pan(gs_convo_mom_FromGameObject, gs_convo_mom_ToGameObject);

		    //Has Cutscene played already?
		    if(!Sequence_02_Ended)
		    {
			    //camScript.triggerCutscene();
			    camScript.triggerCutscene(Sequence_02_target, Sequence_02_length, cameraPanZoom, cameraMoveFor);
			    Sequence_02_clipgate.SetActive(false);
			    timeTested = timeCheck(Sequence_02_length);
			    if (timeTested)
        	    {
				    Sequence_02_Ended = true;
				    timeTested = false;
    		    }

                //Debug.Log("Execute Sequence");
            }
        }
	}

	public void Sequence_03_Prof(){
		if(!Sequence_03_Began)
		{
			setTimers();
		}
		Sequence_03_Began = true;

		if(!Sequence_03_Ended)
		{

			gameplayBegun = false;
			timeTested = timeCheck(Sequence_03_length);
			//Debug.Log("time tested" + timeTested);
			if (timeTested)
        	{
				Sequence_03_Ended = true;
				timeTested = false;
    		}
			cameraPanZoom = false;
			cameraMoveFor = false;
			//camScript.triggerCutscene();
			camScript.triggerCutscene(Sequence_03_target, Sequence_03_length, cameraPanZoom, cameraMoveFor);
			Sequence_03_clipgate.SetActive(false);

			//Debug.Log("Execute Sequence");
		}else{

			//Debug.Log("Welcome to Monster Meld");

			gameplayBegun = true;
		}
	}

    public void Sequence_04_Tutorial_2()
    {
        int monsterCount = player.GetComponent<MonsterGroup>().Count;
        //CHECK player party size
        if (monsterCount > monsterCountReq) { 
            if (!Sequence_04_Began)
            {
                setTimers();
            }
            Sequence_04_Began = true;

            if (!Sequence_04_Ended)
            {

                gameplayBegun = false;
                timeTested = timeCheck(Sequence_04_length);
                //Debug.Log("time tested" + timeTested);
                if (timeTested)
                {
                    Sequence_04_Ended = true;
                    timeTested = false;
                }
                cameraPanZoom = false;
                //camScript.triggerCutscene();
                camScript.triggerCutscene(Sequence_04_target, Sequence_04_length, cameraPanZoom, cameraMoveFor);

                //TO DO: ADD a test outside of this function,to check for party size;
                Sequence_04_clipgate.SetActive(false);

                //Debug.Log("Execute Sequence");
            }
            else
            {

                //Debug.Log("Welcome to Monster Meld");

                gameplayBegun = true;
            }
        }
	}

    public void Sequence_BossLocked()

    {
        if (!Sequence_05_Began)
        {
            //TO DO: lock player and monster in once th ebattle has begun, teleport the entire monster party to player? Terrain height -60.3
            //Sequence_05_clipgate.SetActive(true);
        }
    }


        public void Sequence_05_Boss(){




        if (bossMonster.IsDead)
        {



            if (!Sequence_05_Began)
            {
                setTimers();

            }
            Sequence_05_Began = true;

            if (!Sequence_05_Ended)
            {

                gameplayBegun = false;
                timeTested = timeCheck(Sequence_05_length);
                //Debug.Log("time tested" + timeTested);
                if (timeTested)
                {
                    Sequence_05_Ended = true;
                    timeTested = false;
                }
                cameraPanZoom = false;
                //camScript.triggerCutscene();
                camScript.triggerCutscene(Sequence_05_target, Sequence_05_length, cameraPanZoom, cameraMoveFor);
                //cutscene_test_clipgate.SetActive(false);
                Sequence_05_clipgate.SetActive(false);
                Sequence_05_clipgate_2.SetActive(false);
                Sequence_05_clipgate_3.SetActive(false);
                //Debug.Log("Execute Sequence");
            }

            else
            {

                //Debug.Log("Welcome to Monster Meld");

                gameplayBegun = true;
            }
        }
	}
}
