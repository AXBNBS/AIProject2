
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

        InvokeRepeating("CheckVisibility", 0, 0.5f);
    }

    public void CheckVisibility()
    {
        if (this.environment != null)
        {
            if (this.visible == false)
            {
                this.environment.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                this.environment.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (this.visible==false && presentUnt != 0)
        {
            for(int i=0; i < presentUnt; i++)
            {
                units[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
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
        float checkedDst;

        int bestChoice = 0;
        float bestDst = Vector3.Distance (this.transform.position, hex.transform.position);
        List<Vector3> result = new List<Vector3> ();
        Hexagon currentHex = this;

        while (bestDst != 0) 
        {
            for (int i = 0; i < currentHex.neighbours.Length; i += 1)
            {
                if (currentHex.neighbours[i] != null)
                {
                    checkedDst = Vector3.Distance (currentHex.neighbours[i].transform.position, hex.transform.position);
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

            result.Add (currentHex.neighbours[bestChoice].transform.position);

            currentHex = currentHex.neighbours[bestChoice];
        }

        return result;
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
}