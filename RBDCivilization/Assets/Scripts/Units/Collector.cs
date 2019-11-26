
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Collector : MonoBehaviour
{
    public bool working;
    public int remainingTrn;
    public Hexagon hex;

    private UnitMovement unitMvm;
    private ResourcesHolder resourcesHld;


    // Variable initialization.
    private void Start ()
    {
        working = false;
        remainingTrn = 0;
        unitMvm = this.GetComponent<UnitMovement> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
    }


    // We update the info about the hexagon this unit is in constantly.
    private void Update ()
    {
        hex = unitMvm.currentHex;
    }


    // When the recollection has finished, the corresponding resources will be added to the team the collector belongs to.
    public void CollectResources (string side) 
    {
        if (hex.GetMountain () == true)
        {
            resourcesHld.changeMineral (side, Random.Range (100, 201), true);
        }
        else 
        {
            resourcesHld.changeWood (side, Random.Range (100, 201), true);
        }
    }
}