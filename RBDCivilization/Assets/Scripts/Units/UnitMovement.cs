﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitMovement : MonoBehaviour
{
    public Vector3 target;
    public bool reachedTrg, regroup;
    public Hexagon currentHex;
    public Hexagon previousHex;
    //public int startOft;

    [SerializeField] private int moveSpd, startHex;
    //[SerializeField] private UnitSettings stats;
    private CharacterController characterCtr;
    private Transform feet;
    private List<Vector3> path;
    private float offsetHexX, offsetHexZ;
    private Vector3[] offsets;
    //private List<UnitMovement> collided;
    [SerializeField] private UnitMovement[] allies;
    //private LayerMask unitsMsk;


    // Start is called before the first frame update.
    private void Start ()
    {
        Grid grid = GameObject.FindObjectOfType<Grid> ();

        reachedTrg = false;
        regroup = true;
        characterCtr = this.GetComponent<CharacterController> ();
        feet = this.transform.GetChild (0);
        path = new List<Vector3> ();
        target = GameObject.FindGameObjectsWithTag("Hexagon")[startHex].transform.position;
        offsetHexX = grid.hexagonWth / 5;
        offsetHexZ = grid.hexagonHgt / 5;
        offsets = new Vector3[] {Vector3.zero, new Vector3 (+offsetHexX, 0, offsetHexZ), new Vector3 (-offsetHexX, 0, +offsetHexZ), new Vector3 (+offsetHexX, 0, -offsetHexZ), new Vector3 (-offsetHexX, 0, -offsetHexZ)};
        //target += offsets[startOft];
        //collided = new List<CharacterController> ();
        //unitsMsk = LayerMask.GetMask ("Units");
        path.Add (target);
    }


    // Update is called once per frame.
    private void Update ()
    {
        if (reachedTrg == false) 
        {
            characterCtr.Move ((new Vector3 (target.x, 1, target.z) - this.transform.position).normalized * moveSpd * Time.deltaTime);
            //characterCtr.Move ((new Vector3 (target.x, this.transform.position.y, target.z) - this.transform.position).normalized * moveSpd * Time.deltaTime);

            /*RaycastHit hit = new RaycastHit ();

            if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, characterCtr.radius * 2, unitsMsk) == true) 
            {
                UnitMovement foundUnt = hit.transform.GetComponent<UnitMovement> ();

                if (foundUnt.reachedTrg == true) 
                {

                }
            }*/

            if (Vector3.Distance (feet.position, target) < 0.5f)
            {
                path.RemoveAt (0);

                if (path.Count == 0)
                {
                    reachedTrg = true;
                    /*for (int c = 0; c < this.collided.Count; c += 1) 
                    {
                        this.collided[c].enabled = true;
                    }

                    this.collided.Clear ();*/
                    //currentHex.AddUnit (this);
                    allies = currentHex.UnitsPlaced ();
                    foreach (UnitMovement a in allies) 
                    {
                        a.allies = allies;
                    }
                }
                else 
                {
                    this.target = path[0];
                }
            }
        }
    }


    //
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Hexagon")
        {
            //print("Hey");
            previousHex = currentHex;
            currentHex = other.GetComponent<Hexagon> ();
            currentHex.SetVisible(true);
            if (path.Count == 1) 
            {
                print("hey");
                if (regroup == true)
                {
                    target = currentHex.transform.position + offsets[currentHex.presentUnt];
                    regroup = false;

                    currentHex.AddUnit (this, currentHex.presentUnt);
                }
                else 
                {
                    int position = 0;

                    for (int a = 0; a < allies.Length; a += 1) 
                    {
                        if (allies[a] == this) 
                        {
                            position = a;
                        }
                    }
                    currentHex.AddUnit (this, position);
                }
                //currentHex.AddUnit (this);
                //currentHex.SetVisible (true);
            }
            //unitsInHex = currentHex.presentUnt;
        }
    }


    //
    /*private void OnTriggerStay (Collider other)
    {
        if (this.reachedTrg == false && other.tag == "Hexagon" && unitsInHex != currentHex.presentUnt)
        {
            this.target = currentHex.transform.position + offsets[currentHex.presentUnt];

            path.RemoveAt (0);
            path.Add (target);
        }
    }*/


    /*
    private void OnControllerColliderHit (ControllerColliderHit hit)
    {
        if (this.reachedTrg == false && hit.transform.tag == this.tag) 
        {
            
            CharacterController controllerHit = hit.gameObject.GetComponent<CharacterController> ();

            if (hit.moveDirection.x > hit.moveDirection.z)
            {
                controllerHit.Move (new Vector3 (hit.transform.position.x, hit.transform.position.y, hit.moveDirection.x));
            }
            else 
            {
                controllerHit.Move (new Vector3 (hit.moveDirection.z, hit.transform.position.y, hit.transform.position.z));
            }

            if (controllerHit != null) 
            {
                controllerHit.enabled = false;
                collided.Add (controllerHit);
            }
        }
    }*/


    //
    public void FindPathTo (Hexagon hex) 
    {
        path = currentHex.GetPath (hex);

        for (int u = 0; u < allies.Length; u += 1) 
        {
            if (u != 0)
            {
                Vector3 offset = offsets[u];
                List<Vector3> pathAux = new List<Vector3> ();

                for (int p = 0; p < path.Count; p += 1) 
                {
                    pathAux.Add (path[p] + offset);
                }

                allies[u].path = pathAux;
            }
            allies[u].target = allies[u].path[0];
            allies[u].reachedTrg = false;
        }
    }
}