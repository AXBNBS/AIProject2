
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

    [SerializeField] private int moveSpd;
    private CharacterController characterCtr;
    private Transform feet;
    private float offsetHexX, offsetHexZ;
    public List<Vector3> path;
    private Vector3[] offsets;
    //private List<UnitMovement> collided;
    private UnitMovement[] allies;
    //private LayerMask unitsMsk;
    private bool visibleTarget = false;
    private int moveLmt;

    private Hexagon nextHexagon = null;

    // Just some variable initialization.
    private void Start ()
    {
        Grid grid = GameObject.FindObjectOfType<Grid> ();

        reachedTrg = false;
        regroup = false;
        characterCtr = this.GetComponent<CharacterController> ();
        feet = this.transform.GetChild (0);
        path = new List<Vector3> ();
        //target = GameObject.FindGameObjectsWithTag("Hexagon")[startHex].transform.position;
        //target = currentHex.transform.position;
        interf = GameObject.FindGameObjectWithTag ("Interface");
        offsetHexX = grid.hexagonWth / 6;
        offsetHexZ = grid.hexagonHgt / 6;
        offsets = new Vector3[] {Vector3.zero, new Vector3 (+offsetHexX, 0, +offsetHexZ), new Vector3 (-offsetHexX, 0, +offsetHexZ), new Vector3 (+offsetHexX, 0, -offsetHexZ), new Vector3 (-offsetHexX, 0, -offsetHexZ)};
        //target += offsets[startOft];
        //collided = new List<CharacterController> ();
        //unitsMsk = LayerMask.GetMask ("Units");
        confirmationScript = interf.GetComponentInChildren<ConfirmationScript> ();
        allies = new UnitMovement[1];
        allies[0] = this;
        moveLmt = (int) stats.speed + 1;

        path.Add (target);
    }


    // In case the current target has not been reached, we move towards it and check if the unit has arrived afterwards. If this last condition has been fullfilled, the next target in the path is defined as its next destination; but if the 
    //previously assigned target was the last one, we indicate the unit has reached its destination and assign its new allies.
    private void FixedUpdate ()
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
                if(path.Count!=0)
                    path.RemoveAt (0);

                if (path.Count == 0)
                {
                    int moveLmtMin = 5;

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
                        if (a != null) 
                        {
                            a.allies = allies;
                            if (a.GetMovementLimit () < moveLmtMin) 
                            {
                                moveLmtMin = a.GetMovementLimit ();
                            }
                        } 
                    }

                    foreach (UnitMovement a in allies) 
                    {
                        a.SetMovement (moveLmtMin);
                    }

                    if (currentHex.GetHexagonType () == 0)
                    {
                        bool reachedAll = true;

                        foreach (UnitMovement a in allies) 
                        {
                            if (a.reachedTrg == false) 
                            {
                                reachedAll = false;

                                break;
                            }
                        }

                        if (reachedAll == true) 
                        {
                            foreach (UnitMovement a in allies)
                            {
                                a.SetMovement (a.GetMovementLimit () + 1);
                            }
                        }
                    }
                }
                else 
                {
                    Collider[] hitColliders = Physics.OverlapSphere(path[0], 0.5f);
                    int z = 0;
                    while (z < hitColliders.Length)
                    {
                        if (hitColliders[z].tag == "Hexagon")
                        {
                            nextHexagon = hitColliders[z].GetComponent<Hexagon>();
                            break;
                        }
                        z++;
                    }

                    if (this.tag=="Ally" && nextHexagon != null)
                    {
                        UnitMovement[] units = nextHexagon.UnitsPlaced ();

                        if (units != null) 
                        {
                            for (int i = 0; i < units.Length; i++)
                            {
                                if (units[i] != null && units[i].tag == "Enemy")
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
                                        this.SendMessage("Fight", nextHexagon);
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
                        if (targetHex.GetHexagonType () == -1 || targetHex.GetHexagonType () == -2)
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
                    //Este if es cuando la unidad que se mueve es enemiga
                    if (this.tag == "Enemy" && nextHexagon != null)
                    {
                        UnitMovement[] units = nextHexagon.UnitsPlaced();
                        if (units != null)
                        {
                            for (int i = 0; i < units.Length; i++)
                            {
                                if (units[i] != null && units[i].tag == "Ally")
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
                                    this.SendMessage("Fight", nextHexagon);
                                    break;
                                }
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
        this.SendMessage ("Fight", nextHexagon);
    }


    // If we reach a new hexagon, we update the new and previous hexagons, we make sure the current one is visible. Also if this was the last hexagon of the path, we regroup the units accordingly if there were more awaiting at the destination; or 
    //we just make sure they keep the same structure if that was not the case, we also update the units the current hexagon has assigned either way.
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Hexagon")
        {
            previousHex = currentHex;
            currentHex = other.GetComponent<Hexagon> ();
            //if(moveLmt<0)
            moveLmt -= 1;

            if (this.tag == "Ally")
            {
                for (int i = 0; i < currentHex.neighbours.Length; i += 1)
                {
                    if (currentHex.neighbours[i] != null)
                    {
                        currentHex.neighbours[i].SetVisible(true);
                        if (stats.occupation == "Explorer")
                        {
                            for (int j = 0; j < currentHex.neighbours[i].neighbours.Length; j += 1)
                            {
                                if (currentHex.neighbours[i].neighbours[j] != null)
                                {
                                    currentHex.neighbours[i].neighbours[j].SetVisible(true);
                                }
                            }
                        }
                    }
                }
                currentHex.SetVisible(true);
            }

            if (path.Count == 1) 
            {
                //if (currentHex.presentUnt != 0)
                //{
                if (currentHex.presentUnt < 5)
                {
                    target = currentHex.transform.position + offsets[currentHex.presentUnt];
                    currentHex.AddUnit(this, currentHex.presentUnt);
                }
                // }
                // else 
                // {
                /*int position = 0;

                for (int a = 0; a < allies.Length; a += 1) 
                {
                    if (allies[a] == this) 
                    {
                        position = a;

                        break;
                    }
                }
                print(position);

                currentHex.AddUnit (this, position);*/
                // }
            }
            /*if (path.Count == 1) 
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
            }*/
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
    public int FindPathTo (Hexagon hex) 
    {
        /*int cost = currentHex.GetPath ((int) stats.speed, hex);

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
                Builder builder = this.GetComponent<Builder> ();

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

        return (moveLmt - cost);*/

        if (moveLmt < 1)
        {
            path = new List<Vector3> ();
        }
        else 
        {
            if (this.tag == "Ally")
            {
                path = currentHex.GetPath (hex);
            }
            else 
            {
                path = currentHex.GetTacticalPath (hex);
            }

            int longitud = path.Count;

            for (int i = longitud - 1; i >= moveLmt; i -= 1)
            {
                path.RemoveAt (i);
            }

            targetHex = Physics.OverlapSphere(path[path.Count - 1], 0.1f, GameManager.instance.terrainMsk, QueryTriggerInteraction.Collide)[0].GetComponent<Hexagon> ();
        }

        if (this.tag == "Ally")
        {
            while (targetHex != null && targetHex.UnitsPlaced().Length != 0 && targetHex.UnitsPlaced()[0].tag == "Ally" && (allies.Length > targetHex.GetCapacity () || targetHex.UnitsPlaced()[0].stats.race != stats.race)) 
            {
                path.RemoveAt (path.Count - 1);

                if (path.Count == 0)
                {
                    targetHex = null;
                }
                else 
                {
                    targetHex = Physics.OverlapSphere(path[path.Count - 1], 0.1f, GameManager.instance.terrainMsk, QueryTriggerInteraction.Collide)[0].GetComponent<Hexagon> ();
                }
            }
        }
        else 
        {
            if (targetHex != null) 
            {
                bool sameTrg = false;

                foreach (UnitMovement u in GameManager.instance.AIUnt)
                {
                    if (u != this && u.targetHex == targetHex && (u.allies.Length + allies.Length > 5 || u.stats.race != stats.race))
                    {
                        sameTrg = true;

                        break;
                    }
                }

                while (sameTrg == true || (targetHex != null && targetHex.UnitsPlaced().Length != 0 && targetHex.UnitsPlaced()[0].tag == "Enemy" && (allies.Length > targetHex.GetCapacity () || targetHex.UnitsPlaced()[0].stats.race != stats.race)))
                {
                    path.RemoveAt (path.Count - 1);

                    sameTrg = false;
                    if (path.Count == 0)
                    {
                        targetHex = null;
                    }
                    else
                    {
                        targetHex = Physics.OverlapSphere(path[path.Count - 1], 0.1f, GameManager.instance.terrainMsk, QueryTriggerInteraction.Collide)[0].GetComponent<Hexagon> ();
                        print ("The target hexagon has been changed to " + targetHex.name);
                    }
                    if (targetHex != null)
                    {
                        foreach (UnitMovement u in GameManager.instance.AIUnt)
                        {
                            if (u != this && u.targetHex == targetHex && (u.allies.Length + allies.Length > 5 || u.stats.race != stats.race))
                            {
                                sameTrg = true;

                                break;
                            }
                        }
                    }
                }
            }
        }

        /*if (this.tag == "Enemy" && this.stats.occupation == "Soldier")
        {
            if (path.Count != 0)
            {
                Collider[] hitColliders = Physics.OverlapSphere(path[path.Count - 1], 0.5f);
                int j = 0;
                while (j < hitColliders.Length)
                {
                    if (hitColliders[j].tag == "Hexagon")
                    {
                        targetHex = hitColliders[j].GetComponent<Hexagon>();
                        break;
                    }
                    j++;
                }

                /*for (int i = 0; i < gameManager.AIUnt.Count; i++)
                {
                    if (gameManager.AIUnt[i] != this && gameManager.AIUnt[i].targetHex != null && gameManager.AIUnt[i].targetHex == this.targetHex)
                    {
                        print(gameManager.AIUnt[i].targetHex);
                        print(this.targetHex);
                        foreach (Hexagon h in targetHex.neighbours)
                        {
                            if (h != null)
                            {
                                targetHex = h;
                                path[path.Count - 1] = h.transform.position;
                                break;
                            }
                        }
                        break;
                    }
                }

                if (targetHex.GetIsBuilded() == true && targetHex.GetCity().GetCitySide() == "Red")
                {
                    path.RemoveAt(path.Count - 1);
                }

                if (targetHex.GetCapacity() < this.allies.Length)
                {
                    foreach (Hexagon h in targetHex.neighbours)
                    {
                        if (h != null)
                        {
                            targetHex = h;
                            path[path.Count - 1] = h.transform.position;
                            break;
                        }
                    }
                }
            }
        }*/

        if (path.Count > 0)
        {
            visibleTarget = hex.GetVisible ();

            for (int u = 0; u<allies.Length && allies[u] != null; u += 1)
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
                    Builder builder = this.GetComponent<Builder> ();

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

            if (path[path.Count - 1] != hex.transform.position)
            {
                return 0;
            }
            else 
            {
                return 1;
            }
        }
        else
        {
            return -1;
        }
    }


    // Returns all units that have become allies with the current one (including itself).
    public UnitMovement[] GetAllies ()
    {
        return allies;
    }


    // After each turn, the movement limit of the character is reset.
    public void ResetMovement () 
    {
        moveLmt = (int) stats.speed;
    }


    // We set the movement limit to a determined value.
    public void SetMovement (int m)
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


    // Sets a new path for the unit.
    public void SetPath (List<Vector3> p) 
    {
        path = p;
    }
}