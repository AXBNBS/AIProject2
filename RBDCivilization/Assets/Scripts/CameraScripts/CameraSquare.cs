
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraSquare : MonoBehaviour
{
    private Camera cam;
    private Ray ray1, ray2, ray3, ray4;
    private RaycastHit rayHit1, rayHit2, rayHit3, rayHit4;


    // Start is called before the first frame update.
    private void Start ()
    {
        cam = this.GetComponent<Camera> ();
        
    }


    // Update is called once per frame.
    private void Update ()
    {
        ray1 = cam.ViewportPointToRay (new Vector3 (0, 0, 0));
        ray2 = cam.ViewportPointToRay (new Vector3 (1, 0, 0));
        ray3 = cam.ViewportPointToRay (new Vector3 (0, 1, 0));
        ray4 = cam.ViewportPointToRay (new Vector3 (1, 1, 0));

        Physics.Raycast (ray1, out rayHit1);
        Physics.Raycast (ray2, out rayHit2);
        Physics.Raycast (ray3, out rayHit3);
        Physics.Raycast (ray4, out rayHit4);
    }


    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine (ray1.origin, rayHit1.point);
        Gizmos.DrawLine (ray2.origin, rayHit2.point);
        Gizmos.DrawLine (ray3.origin, rayHit3.point);
        Gizmos.DrawLine (ray4.origin, rayHit4.point);
    }
}