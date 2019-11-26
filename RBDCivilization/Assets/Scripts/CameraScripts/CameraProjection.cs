
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraProjection : MonoBehaviour
{
    [SerializeField] private LayerMask terrainMsk;
    [SerializeField] private int height;
    private Camera cam;
    private Ray ray1, ray2, ray3, ray4;
    private RaycastHit rayHit1, rayHit2, rayHit3, rayHit4;
    private LineRenderer lineRnd;
    private Vector3 previousPos;
    private Quaternion previousRot;
    private bool firstFrm;


    // Variable initialization.
    private void Start ()
    {
        cam = this.GetComponent<Camera> ();
        lineRnd = this.GetComponent<LineRenderer> ();
        previousPos = this.transform.position;
        previousRot = this.transform.rotation;
        firstFrm = true;
    }


    // If the position or rotation of the camera has changed since last frame, or this is the first frame, we need to update the projection on the minimap.
    private void Update ()
    {
        if (previousPos != this.transform.position || previousRot != this.transform.rotation || firstFrm == true) 
        {
            GetPoints ();
        }

        previousPos = this.transform.position;
        previousRot = this.transform.rotation;
        firstFrm = false;
    }


    // Casts 4 rays that will get the vertices of the camera projection over the terrain, these 4 points's coordinates will be used to set the position of the points used by the line renderer.
    private void GetPoints () 
    {
        ray1 = cam.ViewportPointToRay (new Vector3 (0, 0, 0));
        ray2 = cam.ViewportPointToRay (new Vector3 (1, 0, 0));
        ray3 = cam.ViewportPointToRay (new Vector3 (1, 1, 0));
        ray4 = cam.ViewportPointToRay (new Vector3 (0, 1, 0));

        Physics.Raycast (ray1, out rayHit1, Mathf.Infinity, terrainMsk);
        Physics.Raycast (ray2, out rayHit2, Mathf.Infinity, terrainMsk);
        Physics.Raycast (ray3, out rayHit3, Mathf.Infinity, terrainMsk);
        Physics.Raycast (ray4, out rayHit4, Mathf.Infinity, terrainMsk);
        lineRnd.SetPosition (0, new Vector3 (rayHit1.point.x, height, rayHit1.point.z));
        lineRnd.SetPosition (1, new Vector3 (rayHit2.point.x, height, rayHit2.point.z));
        lineRnd.SetPosition (2, new Vector3 (rayHit3.point.x, height, rayHit3.point.z));
        lineRnd.SetPosition (3, new Vector3 (rayHit4.point.x, height, rayHit4.point.z));
    }
}