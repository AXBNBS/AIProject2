
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FaceCamera : MonoBehaviour
{
    private Transform camera;


    // Start is called before the first frame update.
    private void Start ()
    {
        camera = Camera.main.transform;
    }


    // Update is called once per frame.
    private void Update ()
    {
        this.transform.rotation = Quaternion.LookRotation (camera.position - this.transform.position);
    }
}