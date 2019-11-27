﻿
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
    private GameManager gameManager;

    // Variable initialization.
    private void Start ()
    {
        working = false;
        remainingTrn = 0;
        unitMvm = this.GetComponent<UnitMovement> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    // We update the info about the hexagon this unit is in constantly.
    private void Update ()
    {
        hex = unitMvm.currentHex;
    }

    public void BeginCollect()
    {
        working = true;
        remainingTrn = 2;
    }

    // When the recollection has finished, the corresponding resources will be added to the team the collector belongs to.
    public void CollectResources (string side) 
    {
        if (hex.GetMountain () == true)
        {
            resourcesHld.changeMineral (side, Random.Range (100, 201), true);
        }
        else if(hex.GetHexagonType()==2)
        {
            resourcesHld.changeWood (side, Random.Range (100, 201), true);
        }
        else
        {
            resourcesHld.changeStores (side, Random.Range(10, 31), true);
        }
        this.unitMvm.currentHex.SetRemainingTurnsToCollect(3);
        gameManager.restoringHexagons.Add(this.unitMvm.currentHex);
    }
}