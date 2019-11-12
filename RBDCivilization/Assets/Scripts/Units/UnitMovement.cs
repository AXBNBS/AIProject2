
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitMovement : MonoBehaviour
{
    public Vector3 target;
    public bool reachedTrg;
    public Hexagon currentHex;
    public int startOft;

    [SerializeField] private int moveSpd;
    //[SerializeField] private UnitSettings stats;
    private CharacterController characterCtr;
    private Transform feet;
    private List<Vector3> path;
    private float offsetHexX, offsetHexZ;
    private Vector3[] offsets;
    private List<CharacterController> collided;
    private UnitMovement[] allies;


    // Start is called before the first frame update.
    private void Start ()
    {
        Grid grid = GameObject.FindObjectOfType<Grid> ();

        reachedTrg = false;
        characterCtr = this.GetComponent<CharacterController> ();
        feet = this.transform.GetChild (0);
        path = new List<Vector3> ();
        target = GameObject.FindGameObjectWithTag("Hexagon").transform.position;
        offsetHexX = grid.hexagonWth / 4;
        offsetHexZ = grid.hexagonHgt / 4;
        offsets = new Vector3[] {Vector3.zero, new Vector3 (+offsetHexX, 0, offsetHexZ), new Vector3 (-offsetHexX, 0, +offsetHexZ), new Vector3 (+offsetHexX, 0, -offsetHexZ), new Vector3 (-offsetHexX, 0, -offsetHexZ)};
        target += offsets[startOft];
        collided = new List<CharacterController> ();

        path.Add (target);
    }


    // Update is called once per frame.
    private void Update ()
    {
        if (reachedTrg == false) 
        {
            characterCtr.Move ((target - this.transform.position).normalized * moveSpd * Time.deltaTime);
            if (Vector3.Distance (feet.position, target) < 0.5f)
            {
                path.RemoveAt (0);

                if (path.Count == 0)
                {
                    this.reachedTrg = true;
                    for (int c = 0; c < this.collided.Count; c += 1) 
                    {
                        this.collided[c].enabled = true;
                    }

                    this.collided.Clear ();
                    this.currentHex.AddUnit (this);
                    allies = this.currentHex.UnitsPlaced ();
                    foreach (UnitMovement a in allies) 
                    {
                        a.allies = this.allies;
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
            currentHex = other.GetComponent<Hexagon> ();
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


    //
    private void OnControllerColliderHit (ControllerColliderHit hit)
    {
        if (this.reachedTrg == false && hit.transform.tag == this.tag) 
        {
            CharacterController controllerHit = hit.gameObject.GetComponent<CharacterController> ();

            if (controllerHit != null) 
            {
                controllerHit.enabled = false;
                collided.Add (controllerHit);
            }
        }
    }


    //
    public void FindPathTo (Hexagon hex) 
    {
        this.path = this.currentHex.GetPath (hex);

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