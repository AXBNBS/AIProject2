
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class Hexagon : MonoBehaviour
{
    public Hexagon[] neighbours;
    public int presentUnt;
    public GameObject environment;
    public UnitMovement[] units;

    private SphereCollider sphereCol;

    public Material MaterialVisible;
    public Material MaterialNoVisible;
    public int hexagonType; // -1 rio o montaña sin perforar/+1 pradera o montaña perforada/+2 bosque/0 lago

    private bool isBuilded = false; //Si hay construccion en el hexagono
    public bool visible = false; //Si esta visible o no
    private City building = null; //Tipo de edificio pudiendo ser "capital", "farm", "sawmill" o "mina"
    
    public Transform CentroHexagono; //Para cuando generes edificios

    private bool mountain;
    private int remainingTurnsToCollect;


    // Variable initialization.
    private void Awake ()
    {
        neighbours = new Hexagon[6];
        presentUnt = 0;
        units = new UnitMovement[5];
        sphereCol = this.gameObject.GetComponent<SphereCollider> ();
        mountain = false;
        remainingTurnsToCollect = 0;

        InvokeRepeating ("CheckVisibility", 0, 0.5f);
    }


    public void CheckVisibility ()
    {
        if (this.environment != null)
        {
            if (this.visible == false)
            {
                this.environment.transform.GetChild(0).gameObject.SetActive (false);
            }
            else
            {
                this.environment.transform.GetChild(0).gameObject.SetActive (true);
            }
        }

        /*if (this.visible == false && presentUnt != 0)
        {
            for (int i = 0; i < presentUnt; i++)
            {
                units[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else if (presentUnt != 0)
        {
            for (int i = 0; i < presentUnt; i++)
            {
                units[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }*/

        if (Grid.instance.visibleAll == true) 
        {
            CancelInvoke ("CheckVisibility");
        }
    }


    // If a unit leaves the hexagon and was a part of the units list, we know every unit is going to leave, so there are now 0 present units and the array is emptied.
    private void OnTriggerExit (Collider other)
    {
        if (other.tag == "Ally" || other.tag == "Enemy") 
        {
            if (units.Contains (other.GetComponent<UnitMovement> ()) == true) 
            {
                EmptyHexagon ();
            }
        }
    }


    /*
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere (this.transform.position, sphereCol.radius);
    }*/


    // To add a new unit to the hexagon's array of units. 
    public void AddUnit (UnitMovement unit, int position) 
    {
        units[position] = unit;
        presentUnt += 1;
    }


    // To get the number of units this hexagon can contain.
    public int GetCapacity () 
    {
        return (units.Length - presentUnt);
    }


    // We return an array containing every unit that's currently in the hexagon.
    public UnitMovement[] UnitsPlaced () 
    {
        if (presentUnt == 0)
        {
            return new UnitMovement[0];
        }
        else 
        {
            UnitMovement[] result = new UnitMovement[presentUnt];

            for (int r = 0; r < result.Length; r += 1)
            {
                result[r] = units[r];
            }

            return result;
        }
    }


    // To get the shortest path from the current hexagon to another one that serves as destination.
    public List<Vector3> GetPath (Hexagon hex)
    {
        /*IDictionary<Hexagon, int> distances = new Dictionary<Hexagon, int> ();
        IDictionary<Hexagon, Hexagon> parents = new Dictionary<Hexagon, Hexagon> ();
        List<Hexagon> unexplored = new List<Hexagon> ();

        foreach (Hexagon h in Grid.instance.hexagons)
        {
            if (h.hexagonType >= 0)
            {
                distances.Add (new KeyValuePair<Hexagon, int> (h, int.MaxValue));
                parents.Add (new KeyValuePair<Hexagon, Hexagon> (h, null));
                unexplored.Add (h);
            }
        }

        distances[this] = 0;

        while (unexplored.Count > 0)
        {
            Hexagon current = distances.Where(x => unexplored.Contains (x.Key)).OrderBy(x => x.Value).First().Key;

            if (current == hex) 
            {
                List<Vector3> path = new List<Vector3> ();
                Hexagon check = hex;

                while (check != this) 
                {
                    path.Add (check.transform.position);

                    check = parents[check];
                }

                path.Reverse ();
                UnitsPlaced()[0].SetPath (path);

                return distances[hex];
            }

            unexplored.Remove (current);

            foreach (Hexagon n in current.neighbours) 
            {
                if (n != null && n.hexagonType >= 0) 
                {
                    int distance;

                    if (n.hexagonType != 0)
                    {
                        distance = distances[current] + n.hexagonType;
                    }
                    else 
                    {
                        distance = distances[current] - reach;
                    }
                    if (distance < distances[n]) 
                    {
                        distances[n] = distance;
                        parents[n] = current;
                    }
                }
            }
        }

        return 0;
    }*/

        /*float checkedDst;

        int bestChoice = 0, cost = 0;
        float bestDst = Vector3.Distance(this.transform.position, hex.transform.position);
        List<Vector3> result = new List<Vector3>();
        Hexagon currentHex = this;

        while (bestDst != 0)
        {
            for (int i = 0; i < currentHex.neighbours.Length; i += 1)
            {
                if (currentHex.neighbours[i] != null)
                {
                    checkedDst = Vector3.Distance(currentHex.neighbours[i].transform.position, hex.transform.position);
                    if (checkedDst < bestDst)
                    {
                        bestChoice = i;
                        bestDst = checkedDst;

                        if (bestDst == 0)
                        {
                            break;
                        }
                    }
                }
            }

            result.Add(currentHex.neighbours[bestChoice].transform.position);

            cost += currentHex.neighbours[bestChoice].GetHexagonType();
            if (currentHex.neighbours[bestChoice].GetHexagonType() == 0 && currentHex.UnitsPlaced()[0].GetMovementLimit() > cost)
            {
                cost = 0;
            }
            currentHex = currentHex.neighbours[bestChoice];
        }

        UnitsPlaced()[0].SetPath(result);

        return cost;*/

        /*float checkedDst;
        bool roundStr;

        int bestChoice = 0;
        float bestDst = float.MaxValue;
        List<Vector3> result = new List<Vector3> ();
        Hexagon currentHex = this;

        while (bestDst != 0)
        {
            roundStr = true;
            for (int i = 0; i < currentHex.neighbours.Length; i += 1)
            {
                if (currentHex.neighbours[i] != null && currentHex.neighbours[i].GetHexagonType () >= 0 && result.Contains (currentHex.neighbours[i].transform.position) == false)
                {
                    if (roundStr == true)
                    {
                        bestChoice = i;
                        bestDst = Vector3.Distance (currentHex.neighbours[i].transform.position, hex.transform.position);
                        roundStr = false;
                    }
                    else 
                    {
                        checkedDst = Vector3.Distance (currentHex.neighbours[i].transform.position, hex.transform.position);
                        if (checkedDst < bestDst) 
                        {
                            bestChoice = i;
                            bestDst = checkedDst;
                        }
                    }

                    if (bestDst == 0) 
                    {
                        break;
                    }
                }
            }

            result.Add (currentHex.neighbours[bestChoice].transform.position);
            print ("Chosen: " + currentHex.neighbours[bestChoice].name);

            currentHex = currentHex.neighbours[bestChoice];
        }

        return result;*/
        Hexagon current;

        Queue<Hexagon> queue = new Queue<Hexagon> ();
        HashSet<Hexagon> explored = new HashSet<Hexagon> ();
        List<Vector3> result = new List<Vector3> ();
        IDictionary<Hexagon, Hexagon> parents = new Dictionary<Hexagon, Hexagon> ();

        queue.Enqueue (this);

        while (queue.Count != 0) 
        {
            current = queue.Dequeue ();

            if (current == hex) 
            {
                break;
            }

            foreach (Hexagon n in current.neighbours) 
            {
                if (n != null && n.hexagonType >= 0 && explored.Contains (n) == false) 
                {
                    explored.Add (n);
                    parents.Add (n, current);
                    queue.Enqueue (n);
                }
            }
        }

        for (Hexagon h = hex; h != this; h = parents[h]) 
        {
            result.Add (h.transform.position);
        }
        result.Reverse ();

        return result;
    }


    // Traces a path for the enemy unit that will make sure it avoids the player's units, unless the final destination of the unit is a cell that already has player units in it.
    public List<Vector3> GetTacticalPath (Hexagon hex) 
    {
        Hexagon current;

        Queue<Hexagon> queue = new Queue<Hexagon> ();
        HashSet<Hexagon> explored = new HashSet<Hexagon> ();
        List<Hexagon> avoid = new List<Hexagon> ();
        List<Vector3> result = new List<Vector3> ();
        IDictionary<Hexagon, Hexagon> parents = new Dictionary<Hexagon, Hexagon> ();

        foreach (Hexagon h in GameManager.instance.avoidedHex) 
        {
            avoid.Add (h);
        }
        avoid.Remove (hex);
        foreach (Hexagon n in hex.neighbours) 
        {
            avoid.Remove (n);
        }
        queue.Enqueue (this);

        while (queue.Count != 0)
        {
            current = queue.Dequeue ();

            if (current == hex)
            {
                break;
            }

            foreach (Hexagon n in current.neighbours)
            {
                if (n != null && n.hexagonType >= 0 && avoid.Contains (n) == false && explored.Contains (n) == false)
                {
                    explored.Add (n);
                    parents.Add (n, current);
                    queue.Enqueue (n);
                }
            }
        }

        if (parents.ContainsKey (hex) == true)
        {
            for (Hexagon h = hex; h != this; h = parents[h])
            {
                result.Add (h.transform.position);
            }
            result.Reverse ();

            print ("AI used tactical pathfinding to reach hexagon " + hex.name);
            return result;
        }
        else 
        {
            print ("AI used regular pathfinding to reach hexagon " + hex.name);
            return GetPath (hex);
        }
    }


    public int GetHexagonType ()
    {
        return hexagonType;
    }


    public void SetHexagonType (int n)
    {
        hexagonType = n;
    }


    public bool GetIsBuilded ()
    {
        return isBuilded;
    }


    public void SetIsBuilded (bool n)
    {
        isBuilded = n;
    }


    public bool GetVisible ()
    {
        return visible;
    }


    public void SetVisible (bool n)
    {
        visible = n;

        var renderer = GetComponent<Renderer> ();

        if (n == true)
        {
            if(this.hexagonType==0 || this.hexagonType == -2)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            renderer.material = MaterialVisible;
        } 
        else
        {
            renderer.material = MaterialNoVisible;
        }
    }


    public City GetCity ()
    {
        return building;
    }


    public void SetCity (City n)
    {
        building = n;
    }


    // We check that the specified point is part of the hexagon's area.
    public bool TargetInHexagon (Vector3 target) 
    {
        return (Vector3.Distance (this.transform.position, target) <= sphereCol.radius * this.transform.localScale.x);
    }


    public UnitMovement[] GetUnits ()
    {
        return units;
    }


    public bool GetMountain ()
    {
        return mountain;
    }


    public void SetMountain (bool n)
    {
        mountain = n;

        SetHexagonType (-1);
    }


    public void EmptyHexagon () 
    {
        presentUnt = 0;
        units = new UnitMovement[5];
    }


    public int GetRemainingTurnsToCollect ()
    {
        return remainingTurnsToCollect;
    }


    public void SetRemainingTurnsToCollect (int turns)
    {
        remainingTurnsToCollect += turns;
    }

    public void SetMaterialVisible(Material m)
    {
        MaterialVisible = m;
    }

    public Material GetMaterialVisible()
    {
        return MaterialVisible;
    }


    // We'll call this function if an hexagon is unavailable for whatheaver reason, so it returns the closest available one to it.
    public Hexagon FindClosestAvailableHex () 
    {
        Hexagon chosen;

        Hexagon available = null;
        Queue<Hexagon> check = new Queue<Hexagon> ();

        check.Enqueue (this);

        while (available == null && check.Count > 0) 
        {
            chosen = check.Dequeue ();

            foreach (Hexagon n in chosen.neighbours) 
            {
                if (n.hexagonType >= 0 && n.presentUnt == 0 && (n.environment == null || n.environment.tag == "Tunnel" || n.environment.tag == "Forest"))
                {
                    available = n;

                    break;
                }
                else
                {
                    if (check.Contains (n) == false)
                    {
                        check.Enqueue (n);
                    }
                }
            }
        }

        return available;
    }
}