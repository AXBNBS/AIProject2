
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<UnitMovement> playerUnt, AIUnt;
    public List<Farm> playerFrm, AIFrm;
    public List<Builder> playerBld, AIBld;
    public List<Collector> playerCll, AICll;
    public List<Hexagon> restoringHexagons;
    public BehaviourTreeScript behaviourTree;

    [SerializeField] private int storesPerFrm;
    private ResourcesHolder resourcesHld;
    private CameraController cameraCtr;

    private BuildingMenu buildingMenu;
    public GameObject endTurnButton;
    public Grid grid;

    // We get every present unit, farm, builder and collector (independently of their faction) at the start of the game, and add them to their corresponding lists.
    private void Start ()
    {
        buildingMenu = GameObject.FindObjectOfType<BuildingMenu>();
        UnitMovement[] units = GameObject.FindObjectsOfType<UnitMovement> ();
        Farm[] farms = GameObject.FindObjectsOfType<Farm> ();
        Builder[] builders = GameObject.FindObjectsOfType<Builder> ();
        Collector[] collectors = GameObject.FindObjectsOfType<Collector> ();

        restoringHexagons = new List<Hexagon>();
        storesPerFrm = 30;//ESTO ES DE EJEMPLO

        instance = this;
        playerUnt = new List<UnitMovement> ();
        AIUnt = new List<UnitMovement> ();
        playerFrm = new List<Farm> ();
        AIFrm = new List<Farm> ();
        playerBld = new List<Builder> ();
        AIBld = new List<Builder> ();
        playerCll = new List<Collector> ();
        AICll = new List<Collector> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
        cameraCtr = GameObject.FindObjectOfType<CameraController> ();

        foreach (UnitMovement u in units) 
        {
            //print(u.name);
            if (u.tag == "Enemy")
            {
                AIUnt.Add (u);
            }
            else 
            {
                playerUnt.Add (u);
            }
        }
        foreach (Farm f in farms)
        {
            if (f.tag == "RedFarm")
            {
                AIFrm.Add (f);
            }
            else 
            {
                playerFrm.Add (f);
            }
        }
        foreach (Builder b in builders) 
        {
            //print(b.name);
            if (b.tag == "Enemy")
            {
                AIBld.Add (b);
            }
            else
            {
                playerBld.Add (b);
            }
        }
        foreach (Collector c in collectors) 
        {
            if (c.tag == "Enemy")
            {
                AICll.Add (c);
            }
            else 
            {
                playerCll.Add (c);
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < 49; i++)
            {
                for (int j = 0; j < 29; j++)
                {
                    grid.hexagons[j, i].SetVisible(true);
                }
            }
        }
    }

    // Every enemy unit gets its movement limit reset, buildings are finished (if enough turns have passed), resources are collected (if enough turns have passed) and farms produce new stores if active.
    public void EndPlayerTurn () 
    {
        cameraCtr.SetNullSelectedUnit ();

        List<Hexagon> doneJob = new List<Hexagon> ();

        foreach (UnitMovement u in AIUnt) 
        {
            u.ResetMovement ();
        }

        /*foreach (Builder b in AIBld) 
        {
            if (b.working == true)
            {
                b.remainingTrn -= 1;
                if (b.remainingTrn <= 0)
                {
                    b.working = false;

                    if (doneJob.Contains (b.hex) == false) 
                    {
                        if (b.hex.environment != null)
                        {
                            Destroy (b.hex.environment);
                        }
                        b.EndConstruction ("red");
                        doneJob.Add (b.hex);
                    }
                }
            }
        }

        foreach (Collector c in AICll) 
        {
            if (c.working == true) 
            {
                c.remainingTrn -= 1;
                if (c.remainingTrn <= 0) 
                {
                    c.working = false;

                    if (doneJob.Contains (c.hex) == false) 
                    {
                        c.CollectResources ("red");
                        doneJob.Add (c.hex);
                    }
                }
            }
        }*/
        foreach (Farm f in AIFrm) 
        {
            if (f.GetComponent<City>().GetWitchers() > 0)
            {
                f.active = true;
                print("AAAAAA");
            }
            else
            {
                f.active = false;
            }

            if (f.active == true) 
            {
                resourcesHld.changeStores ("red", storesPerFrm, true);
            }
        }

        endTurnButton.SetActive(false);
        behaviourTree.Evaluate();
    }


    // Every player unit gets its movement limit reset, buildings are finished (if enough turns have passed), resources are collected (if enough turns have passed) and farms produce new stores if active.
    public void StartPlayerTurn () 
    {
        endTurnButton.SetActive(true);
        List<Hexagon> doneJob = new List<Hexagon> ();

        foreach (UnitMovement u in playerUnt)
        {
            u.ResetMovement ();
        }

        foreach(Hexagon h in restoringHexagons)
        {
            h.SetRemainingTurnsToCollect(-1);
        }

        for (int i = restoringHexagons.Count - 1; i >= 0; i--)
        {
            if (restoringHexagons[i].GetRemainingTurnsToCollect() == 0)
            {
                restoringHexagons.RemoveAt(i);
            }
        }

        foreach (Builder b in playerBld)
        {
            if (b.working == true)
            {
                b.remainingTrn -= 1;
                if (b.remainingTrn == 0)
                {
                    b.working = false;

                    if (doneJob.Contains (b.hex) == false)
                    {
                        if (b.hex.environment != null)
                        {
                            Destroy (b.hex.environment);
                        }
                        b.EndConstruction ("blue");
                        doneJob.Add (b.hex);
                    }
                }
            }
        }

        foreach (Collector c in playerCll)
        {
            if (c.working == true)
            {
                c.remainingTrn -= 1;
                if (c.remainingTrn == 0)
                {
                    c.working = false;

                    if (doneJob.Contains (c.hex) == false)
                    {
                        c.CollectResources ("blue");
                        doneJob.Add (c.hex);
                    }
                }
            }
        }
        foreach (Farm f in playerFrm)
        {
            if (f.GetComponent<City>().GetTurroncitos() > 0)
            {
                f.active = true;
            }
            else
            {
                f.active = false;
            }
            if (f.active == true)
            {
                resourcesHld.changeStores ("blue", storesPerFrm, true);
            }
        }

        if (buildingMenu.remainingTurnsToUpgrade == 0 && buildingMenu.upgrading==true)
        {
            buildingMenu.Upgrade();
        }
        else if(buildingMenu.remainingTurnsToUpgrade>0)
        {
            buildingMenu.remainingTurnsToUpgrade--;
        }
    }
}