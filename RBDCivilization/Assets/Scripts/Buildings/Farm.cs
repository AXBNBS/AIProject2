
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Farm : MonoBehaviour
{
    public bool active;


    // Variable initialization.
    private void Start ()
    {
        active = false;
    }


    /* Every active farm will produce stores after each turn.
    public void GenerateStores (string side, int stores) 
    {
        if (active == true) 
        {
            resourcesHld.changeStores (side, stores, true);
        }
    }*/
}