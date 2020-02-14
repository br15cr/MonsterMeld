using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    private Transform playerTarget;
    public float maxDistance = 1;



    public float moveSpeedRamp = 2.0f;
    public float moveSpeed = 2.0f;
    private float moveSpeedDefault;
    public float rotaSpeedV = 2.0f;
    public float rotaSpeedH = 2.0f;

    private bool panZoom;
    private bool moveFor;
    public float zoomSpeed = 2.0f;
    public float panSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Camera cam;

    private CamScript camScript;

    private bool cutscene;
    private bool cutsceneEnd;

    public Vector3 targetOffset = new Vector3(0, 1, 1);
    private Vector3 initialPlaycamPos;
    private Vector3 initialPlaycamRot;

    // Start is called before the first frame update
    void Start()
    {
        camScript = GetComponent<CamScript>();
        playerTarget = target;
        moveSpeedDefault = moveSpeed;
	    cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

            cutscene = camScript.cutscene;
            cutsceneEnd = camScript.cutsceneEnd;
            panZoom = camScript.panZoom;
            moveFor = camScript.moveFor;
            //Pan the camera in movement direction
            if(cutsceneEnd){ 
                target = playerTarget;
                //restore Intial Camera Position

                if(panZoom){
                    transform.position = initialPlaycamPos;
                    transform.eulerAngles = initialPlaycamRot;  
                }


                Debug.Log("Camera Pos  end" + initialPlaycamPos);
                Debug.Log("Camera Rot  end" + initialPlaycamRot);
                cutsceneEnd = false;
                camScript.cutsceneEnd = cutsceneEnd;

            }

            if(cutscene){ 

                target = camScript.target;
                //Save Intial Camera Position



                if(panZoom){
                    //Zooming
                    Vector3 move = target.position.y * zoomSpeed * transform.forward;
                    transform.Translate(move, Space.World);
                    //Panning
                    Vector3 pan = new Vector3(target.position.x * panSpeed, target.position.y * panSpeed, 0);
                    transform.Translate(pan, Space.Self);

                    transform.LookAt(target);
                }
                else{
                    float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * zoomSpeed);
                    if(moveFor){            
                        //IS THIS THE REASON cutscenes end but the camera doesnt switch back until an extra few seconds have passed
                        
                        transform.position = Vector3.Lerp(transform.position, target.position, smooth);
                    


                            // Calculate a rotation a step closer to the target and applies rotation to this object
                            // Determine which direction to rotate towards
                    Vector3 targetDirection = target.position - transform.position;
                    float singleStep = zoomSpeed * Time.deltaTime;
                           // Rotate the forward vector towards the target direction by one step
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                    transform.rotation = Quaternion.LookRotation(newDirection);
                    }else{
                    Vector3 targetDirection = target.position;
                            // The step size is equal to speed times frame time.
                    float singleStep = zoomSpeed * Time.deltaTime;
                           // Rotate the forward vector towards the target direction by one step
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                    transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }


                //return to initial target after time

            }else{

            //yaw += rotaSpeedH * Input.GetAxis("Mouse X");
            //pitch += rotaSpeedV * Input.GetAxis("Mouse Y");
            //transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if(target != null)
            {
                transform.LookAt(target);
                //Debug.Log("Cam: Looking at Target;");
            }


                transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, Time.deltaTime * moveSpeed);
                //Debug.Log("Cam: Moving toward Target;");

          


           
            //transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, Time.deltaTime * moveSpeed);

           // Debug.Log(transform.position.x-target.position.x);
            //Ramp camera move speed vs distance
            float dist = Vector3.Distance(target.position, transform.position);
            //print("Camera Distance to Player: " + dist);

            if(dist > maxDistance)
            {
                moveSpeed += moveSpeedRamp * Time.deltaTime;
            }
            else{
            moveSpeed = moveSpeedDefault;
            }
        }
    }

    public void setCamPos(){
        initialPlaycamPos = transform.position;
        initialPlaycamRot = transform.eulerAngles;
        Debug.Log("Camera Position" + initialPlaycamPos);
        Debug.Log("Camera Rotation" + initialPlaycamRot);
    }


    public Vector2 GetPlayerScreenPosition(){
	return cam.WorldToScreenPoint(target.transform.position);
    }
}
