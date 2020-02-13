using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
	public bool cutscene;
	public bool cutsceneEnd;
    public bool panZoom;
    public bool moveFor;
	public Transform target;
	private float timeStart;
	private float cutsceneLength;
    private PlayerCamera playerCamera;
           

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cutscene){
        	//Disable Player and Camera Controls
            // Debug.Log("cutscene is playing");
        	//Debug.Log("End " + timeStart);
        	//Debug.Log(timeStart + cutsceneLength);
	        if(Time.time > timeStart + cutsceneLength){
	            	cutscene = false;
	            	cutsceneEnd = true;
	            	Debug.Log("Cutscene Ended?" );
	        }
        }
    }

    public void triggerCutscene(GameObject cutscene_target, float cutLength, bool cameraPanZoom, bool cameraMoveFor){
        // Look from current cam Pos to first target
        // OR pan from target 1 to target 2
        playerCamera.setCamPos();

        cutscene = true;
        target = cutscene_target.transform;
        panZoom = cameraPanZoom;
        moveFor = cameraMoveFor;
        cutsceneLength = cutLength;
        timeStart = Time.time;
        //Debug.Log("Srt " + timeStart);
        //Debug.Log("Cut Scene Length Passed?: " + cutsceneLength);

         //   if(target != null)
           // {
           //     transform.LookAt(target);
           // }
        //Debug.Log("Camera Cutscene Activated: Target " );

    }

}
