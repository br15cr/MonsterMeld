using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float maxDistance = 1;
    public float moveSpeed = 1;
    public Vector3 targetOffset = new Vector3(0, 1, 1);
    public float lookSpeed = 1;

    private Vector3 lookTarget = Vector3.zero;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            //transform.LookAt(target);
	    transform.LookAt(lookTarget);
        }
	lookTarget = Vector3.MoveTowards(lookTarget,target.position,Time.deltaTime*lookSpeed);
        transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, Time.deltaTime * moveSpeed);
    }

    public Vector2 GetPlayerScreenPosition(){
	return cam.WorldToScreenPoint(target.transform.position);
    }
}
