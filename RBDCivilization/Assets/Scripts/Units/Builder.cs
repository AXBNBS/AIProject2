
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
    private GameManager gameManager;

    // Variable initialization.
    private void Start ()
    {
        working = false;
        remainingTrn = 0;
        construction = null;
        unitMvm = this.GetComponent<UnitMovement> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    // We update the info about the hexagon this unit is in constantly.
    private void Update ()
    {
        hex = unitMvm.currentHex;
    }


    // The unit starts working of the construction of its assingned building.
    public void BeginConstruction (GameObject building, int mineral, int wood) 
    {
        /*b.remainingTrn -= 1;
        if (b.remainingTrn <= 0)
        {
            b.working = false;

            if (doneJob.Contains(b.hex) == false)
            {
                if (b.hex.environment != null)
                {
                    Destroy(b.hex.environment);
                }
                b.EndConstruction("red");
                doneJob.Add(b.hex);
            }
        }*/
        working = true;
        constructionDat = building.GetComponent<City> ();
        int allies = unitMvm.GetAllies().Length;
        construction = building;
        spentMin = mineral;
        spentWod = wood;
        if (this.tag == "Ally")
        {
            remainingTrn = 1;
        }
        else 
        {
            if (hex.environment != null)
            {
                Destroy (hex.environment);
            }
            EndConstruction ("red");
            gameManager.AIBld.Remove(this);
            gameManager.AIUnt.Remove(this.GetComponent<UnitMovement>());
            this.GetComponent<UnitMovement>().currentHex.EmptyHexagon();
            Destroy(this.gameObject);
        }
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
                    Destroy(hex.neighbours[n].environment);
                    hex.neighbours[n].environment = Instantiate (construction, hex.neighbours[n].transform.position, Quaternion.identity);

                    hex.neighbours[n].SetHexagonType (1);

                    break;
                }
            }
        }
        else 
        {
            if (this.tag == "Ally") 
            {
                hex.environment = Instantiate (construction, hex.transform.position, Quaternion.identity);
            }
            else 
            {
                hex.environment = Instantiate (construction, hex.transform.position, Quaternion.Euler (0, 180, 0));
            }

            hex.SetCity (hex.environment.GetComponent<City>());
            hex.GetCity().SetCitySide (side);
            hex.GetCity().SetSettings();
            if (construction.tag.Contains ("Settlement") == true)
            {
                resourcesHld.changeTotalPopulation (side, 3, true);
                hex.GetCity().SetCityType("Settlement");
                hex.GetCity().currentHex = hex;

                if (this.tag == "Enemy") 
                {
                    hex.environment.tag = "RedSettlement";
                    hex.GetCity().SetCitySide("Red");
                }
            }
            else 
            {
                hex.GetCity().SetCityType("Farm");
                hex.GetCity().currentHex = hex;
                if (this.tag == "Enemy")
                { 
                    GameManager.instance.AIFrm.Add (hex.environment.GetComponent<Farm> ());

                    construction.tag = "RedFarm";
                    hex.GetCity().SetCitySide("Red");
                }
                else
                {
                    GameManager.instance.playerFrm.Add (hex.environment.GetComponent<Farm> ());
                }
            }
            hex.SetHexagonType(1);

            Hexagon auxHex = hex;

            bool keepSearch = true;

            foreach (Hexagon h in hex.neighbours)
            {
                if (h != null && h.presentUnt == 0)
                {
                    unitMvm.FindPathTo (h);
                    keepSearch = false;
                    break;
                }
            }

            if (keepSearch)
            {
                foreach (Hexagon h in hex.neighbours)
                {
                    if (h != null)
                    {
                        foreach (Hexagon h1 in h.neighbours)
                        {
                            if (h1 != null && h1.presentUnt == 0 && h1.GetIsBuilded()==false)
                            {
                                unitMvm.SetMovement(int.MaxValue);
                                unitMvm.FindPathTo(h1);
                                unitMvm.ResetMovement();
                                print(h1);
                                keepSearch = false;
                                break;
                            }
                        }
                        if (!keepSearch)
                            break;
                    }
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