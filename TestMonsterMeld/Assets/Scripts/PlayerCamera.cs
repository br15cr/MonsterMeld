using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float maxDistance = 1;

    public float moveSpeedRamp = 2.0f;
    public float moveSpeed = 2.0f;
    private float moveSpeedDefault;
    public float rotaSpeedV = 2.0f;
    public float rotaSpeedH = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Camera cam;


    public Vector3 targetOffset = new Vector3(0, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        moveSpeedDefault = moveSpeed;
	cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Pan the camera in movement direction
        yaw += rotaSpeedH * Input.GetAxis("Mouse X");
        pitch += rotaSpeedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if(target != null)
        {
            transform.LookAt(target);
        }

 
        transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, Time.deltaTime * moveSpeed);


       
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

    public Vector2 GetPlayerScreenPosition(){
	return cam.WorldToScreenPoint(target.transform.position);
    }
}
