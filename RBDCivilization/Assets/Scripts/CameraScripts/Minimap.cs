
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Minimap : MonoBehaviour
{
    private Transform follow;

    
    // We get the transform of the camera rig.
    private void Start ()
    {
        follow = GameObject.FindObjectOfType<CameraController>().transform;
    }


    // We make sure the camera is constantly using the same X and Z coordinates that the camera rig has.
    private void Update ()
    {
        this.transform.position = new Vector3 (follow.position.x, this.transform.position.y, follow.position.z);
    }
}