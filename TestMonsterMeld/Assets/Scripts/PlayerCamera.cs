using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float maxDistance = 1;
    public float moveSpeed = 1;
    public Vector3 targetOffset = new Vector3(0, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, Time.deltaTime * moveSpeed);
    }
}
