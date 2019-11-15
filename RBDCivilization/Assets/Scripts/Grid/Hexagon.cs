
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Hexagon : MonoBehaviour
{
    public Hexagon[] neighbours;
    public int presentUnt;

    private UnitMovement[] units;
    private SphereCollider sphereCol;

    public Material MaterialVisible;
    public Material MaterialNoVisible;

    private int hexagonType; //-1 rio o montaña sin perforar, 1 pradera o montaña perforada, 2 bosque
    private bool isBuilded = false; //Si hay construccion en el hexagono
    private bool visible = false; //Si esta visible o no
    private City building = null; //Tipo de edificio pudiendo ser "capital", "farm", "sawmill" o "mina"
    
    public Transform CentroHexagono; //Para cuando generes edificios


    // .
    private void Awake ()
    {
        neighbours = new Hexagon[6];
        presentUnt = 0;
        units = new UnitMovement[5];
        sphereCol = this.gameObject.GetComponent<SphereCollider> ();
    }


    //
    private void OnTriggerExit (Collider other)
    {
        if (other.tag == "Unit") 
        {
            units = new UnitMovement[5];
            presentUnt = 0;
        }
    }


    //
    private void OnDrawGizmosSelected () 
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere (this.transform.position, sphereCol.radius * this.transform.localScale.x);
    }


    //
    public void AddUnit (UnitMovement unit, int position) 
    {
        units[position] = unit;
        presentUnt += 1;
    }


    //
    public int GetCapacity () 
    {
        return (units.Length - presentUnt);
    }


    //
    public UnitMovement[] UnitsPlaced () 
    {
        if (presentUnt == 0)
        {
            return null;
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


    //
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


    public int GetHexagonType()
    {
        return hexagonType;
    }


    public void SetHexagonType(int n)
    {
        hexagonType = n;
    }


    public bool GetIsBuilded()
    {
        return isBuilded;
    }


    public void SetIsBuilded(bool n)
    {
        isBuilded = n;
    }


    public bool GetVisible()
    {
        return visible;
    }


    public void SetVisible(bool n)
    {
        visible = n;
        var renderer = GetComponent<Renderer>();
        if (n == true)
        {
            renderer.material = MaterialVisible;
        } else
        {
            renderer.material = MaterialNoVisible;
        }
    }


    public City GetCity()
    {
        return building;
    }


    public void SetCity(City n)
    {
        building = n;
    }


    //
    public bool TargetInHexagon (Vector3 target) 
    {
        return (Vector3.Distance (this.transform.position, target) <= sphereCol.radius * this.transform.localScale.x);
    }
}