
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitMovement : MonoBehaviour
{
    public Vector3 target;
    public bool reachedTrg, regroup;
    public Hexagon currentHex;
    public Hexagon previousHex;
    public Hexagon targetHex;
    public UnitSettings stats;
    //public int startOft;
    public GameObject interf;

    private ConfirmationScript confirmationScript;

    [SerializeField] private int moveSpd, startHex;
    private CharacterController characterCtr;
    private Transform feet;
    private List<Vector3> path;
    private float offsetHexX, offsetHexZ;
    private Vector3[] offsets;
    //private List<UnitMovement> collided;
    private UnitMovement[] allies;
    //private LayerMask unitsMsk;
    private bool visibleTarget = false;
    private int moveLmt;


    // Just some variable initialization.
    private void Start ()
    {
        Grid grid = GameObject.FindObjectOfType<Grid> ();

        reachedTrg = false;
        regroup = true;
        characterCtr = this.GetComponent<CharacterController> ();
        feet = this.transform.GetChild (0);
        path = new List<Vector3> ();
        target = GameObject.FindGameObjectsWithTag("Hexagon")[startHex].transform.position;
        interf = GameObject.FindGameObjectWithTag ("Interface");
        offsetHexX = grid.hexagonWth / 5;
        offsetHexZ = grid.hexagonHgt / 5;
        offsets = new Vector3[] {Vector3.zero, new Vector3 (+offsetHexX, 0, offsetHexZ), new Vector3 (-offsetHexX, 0, +offsetHexZ), new Vector3 (+offsetHexX, 0, -offsetHexZ), new Vector3 (-offsetHexX, 0, -offsetHexZ)};
        //target += offsets[startOft];
        //collided = new List<CharacterController> ();
        //unitsMsk = LayerMask.GetMask ("Units");
        confirmationScript = interf.GetComponentInChildren<ConfirmationScript> ();
        allies = new UnitMovement[5];
        moveLmt = (int) stats.speed + 1;

        path.Add (target);
    }


    // In case the current target has not been reached, we move towards it and check if the unit has arrived afterwards. If this last condition has been fullfilled, the next target in the path is defined as its next destination; but if the 
    //previously assigned target was the last one, we indicate the unit has reached its destination and assign its new allies.
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
                    if (targetHex != null && Vector3.Distance(path[0], targetHex.transform.position)<0.5f)
                    {
                        UnitMovement[] units = targetHex.UnitsPlaced();
                        if (units != null) {
                            for (int i = 0; i < units.Length;i++)
                            {
                                if(units[i] != null && units[i].tag == "Enemy")
                                {
                                    foreach (UnitMovement al in allies)
                                    {
                                        al.reachedTrg = true;
                                    }
                                    for (int x = 0; x< allies.Length; x++)
                                    {
                                        if (allies[x] != null)
                                            currentHex.AddUnit(allies[x], currentHex.presentUnt);
                                    }
                                    
                                    if (visibleTarget)
                                    {
                                        this.SendMessage("Fight", targetHex);
                                    }
                                    else
                                    {
                                        confirmationScript.askCombat(this);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (targetHex != null && Vector3.Distance(path[0], targetHex.transform.position) < 0.5f)
                    {
                        
                        if (targetHex.GetHexagonType() == -1 || targetHex.GetHexagonType() == -2)
                        {
                            foreach (UnitMovement al in allies)
                            {
                                al.reachedTrg = true;
                            }
                            for (int x = 0; x < allies.Length; x++)
                            {
                                if (allies[x] != null)
                                    currentHex.AddUnit(allies[x], currentHex.presentUnt);
                            }
                        }
                    }
                    this.target = path[0];
                }
            }
        }
    }


    public void LookForCombat ()
    {
        this.SendMessage ("Fight", targetHex);
    }


    // If we reach a new hexagon, we update the new and previous hexagons, we make sure the current one is visible. Also if this was the last hexagon of the path, we regroup the units accordingly if there were more awaiting at the destination; or 
    //we just make sure they keep the same structure if that was not the case, we also update the units the current hexagon has assigned either way.
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Hexagon")
        {
            //print("Hey");
            previousHex = currentHex;
            currentHex = other.GetComponent<Hexagon> ();
            moveLmt -= 1;

            for(int i = 0; i < currentHex.neighbours.Length; i++)
            {
                if (currentHex.neighbours[i] != null)
                {
                    currentHex.neighbours[i].SetVisible(true);
                    if (stats.occupation == "Explorer")
                    {
                        for(int j=0; j < currentHex.neighbours[i].neighbours.Length; j++)
                        {
                            if (currentHex.neighbours[i].neighbours[j] != null)
                            {
                                currentHex.neighbours[i].neighbours[j].SetVisible(true);
                            }
                        }
                    }
                }
            }
            currentHex.SetVisible (true);

            if (path.Count == 1) 
            {
                //print("hey");
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

                            break;
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


    // We get the path to the indicated hexagon. We also make sure that our current allies keep the same alignment while the path is being traversed.
    public void FindPathTo (Hexagon hex) 
    {
        path = currentHex.GetPath (hex);
        if (moveLmt >= path.Count) 
        {
            visibleTarget = hex.GetVisible ();

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

            if (stats.occupation == "Worker" || stats.occupation == "Collector") 
            {
                if (stats.occupation == "Worker")
                {
                    Builder builder = this.GetComponent<Builder>();

                    if (builder.working == true)
                    {
                        builder.StopBuilding ();
                    }
                }
                else 
                {
                    this.GetComponent<Collector>().working = false;
                }
            }
        }
    }


    public UnitMovement[] GetAllies()
    {
        return allies;
    }


    // After each turn, the movement limit of the character is reset.
    public void ResetMovement () 
    {
        moveLmt = (int) stats.speed;
    }

    public void SetMovement(int m)
    {
        moveLmt = m;
    }


    // Returns the value of the movement limit variable or 0, if the value is negative.
    public int GetMovementLimit () 
    {
        if (moveLmt < 0)
        {
            return 0;
        }
        else 
        {
            return moveLmt;
        }
    }
}