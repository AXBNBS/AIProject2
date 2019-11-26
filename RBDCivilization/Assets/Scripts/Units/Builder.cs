
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Builder : MonoBehaviour
{
    public bool working;
    public int remainingTrn;
    public GameObject construction;
    public Hexagon hex;

    //[SerializeField] private GameObject settlement, farm, tunnel;
    private UnitMovement unitMvm;
    private City constructionDat;
    private ResourcesHolder resourcesHld;
    private int spentMin, spentWod;


    // Variable initialization.
    private void Start ()
    {
        working = false;
        remainingTrn = 0;
        construction = null;
        unitMvm = this.GetComponent<UnitMovement> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
    }


    // We update the info about the hexagon this unit is in constantly.
    private void Update ()
    {
        hex = unitMvm.currentHex;
    }


    // The unit starts working of the construction of its assingned building.
    public void BeginConstruction (GameObject building, int mineral, int wood) 
    {
        working = true;
        constructionDat = building.GetComponent<City> ();
        remainingTrn = constructionDat.settings.turns;
        construction = building;
        spentMin = mineral;
        spentWod = wood;
    }


    // The unit finishes its construction, changing the hexagon's corresponding parameters to make the new cell act as expected, instantiating the proper game objects in those cells and also moving the builder away from the cell (if a settlement or 
    //farm has been built), changing the total population (if a settlement has been built) or adding a farm to the game manager's list of farms (if a farm has been built).
    public void EndConstruction (string side) 
    {
        if (construction.tag == "Tunnel")
        {
            for (int n = 0; n < hex.neighbours.Length; n += 1) 
            {
                if (hex.neighbours[n] != null && hex.neighbours[n].GetMountain () == true && hex.neighbours[n].GetHexagonType () == -1)
                {
                    hex.neighbours[n].environment = Instantiate (construction, hex.neighbours[n].transform.position, Quaternion.identity);

                    hex.neighbours[n].SetHexagonType (1);

                    break;
                }
            }
        }
        else 
        {
            hex.environment = Instantiate (construction, hex.transform.position, Quaternion.identity);

            hex.SetCity (constructionDat);
            hex.GetCity().SetCitySide (side);
            if (construction.tag.Contains ("Settlement") == true)
            {
                resourcesHld.changeTotalPopulation (side, 3, true);

                if (this.tag == "Enemy") 
                {
                    construction.tag = "RedSettlement";
                }
            }
            else 
            {
                if (this.tag == "Enemy")
                { 
                    GameManager.instance.AIFrm.Add (construction.GetComponent<Farm> ());

                    construction.tag = "RedFarm";
                }
                else
                {
                    GameManager.instance.playerFrm.Add (construction.GetComponent<Farm> ());
                }
            }

            Hexagon auxHex = hex;

            foreach (Hexagon h in hex.neighbours)
            {
                if (h != null && h.presentUnt == 0)
                {
                    unitMvm.FindPathTo (h);

                    break;
                }
            }

            auxHex.SetIsBuilded (true);
        }
    }


    // The builder stops working to move somewhere else, the resources that it had previously spent on the building will be recovered.
    public void StopBuilding () 
    {
        working = false;

        if (this.tag == "Enemy")
        {
            resourcesHld.changeWood ("red", spentWod, true);
            resourcesHld.changeMineral ("red", spentMin, true);
        }
        else 
        {
            resourcesHld.changeWood ("blue", spentWod, true);
            resourcesHld.changeMineral ("blue", spentMin, true);
        }
    }
}