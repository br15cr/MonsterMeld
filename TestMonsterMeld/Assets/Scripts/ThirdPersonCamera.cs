using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Player target;
    public Vector3 offset;
    void Start() {
        
    }

    void Update() {
        transform.position = target.transform.position + offset;
    }
}
