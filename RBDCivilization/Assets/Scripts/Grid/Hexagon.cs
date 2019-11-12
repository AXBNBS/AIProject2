
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Hexagon : MonoBehaviour
{
    public Hexagon[] neighbours;
    public int presentUnt;

    private UnitMovement[] units;


    // .
    private void Awake ()
    {
        neighbours = new Hexagon[6];
        presentUnt = 0;
        units = new UnitMovement[5];
    }


    // Update is called once per frame.
    private void Update ()
    {
        
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
    public void AddUnit (UnitMovement unit) 
    {
        units[presentUnt] = unit;
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
}