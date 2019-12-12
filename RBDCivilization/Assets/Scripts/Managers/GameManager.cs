
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
    public HashSet<Hexagon> avoidedHex;
    public BehaviourTreeScript behaviourTree;
    public LayerMask terrainMsk;

    [SerializeField] private int storesPerFrm;
    private ResourcesHolder resourcesHld;
    private CameraController cameraCtr;

    private BuildingMenu buildingMenu;
    public GameObject endTurnButton;
    public GameObject actionsMenuUI;
    public GameObject unitMenuUI;
    public GameObject cityMenuUI;
    public GameObject confirmationMenuUI;
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
        storesPerFrm = 5;//ESTO ES DE EJEMPLO

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

    private void FixedUpdate ()
    {
        if (Input.GetKeyDown (KeyCode.Z) == true)
        {
            for (int i = 0; i < 49; i++)
            {
                for (int j = 0; j < 29; j++)
                {
                    grid.hexagons[j, i].SetVisible (true);
                }
            }
            grid.visibleAll = true;
        }
    }

    // Every enemy unit gets its movement limit reset, buildings are finished (if enough turns have passed), resources are collected (if enough turns have passed) and farms produce new stores if active.
    public void EndPlayerTurn () 
    {
        cameraCtr.SetNullSelectedUnit ();

        List<Hexagon> doneJob = new List<Hexagon> ();
        bool oneActiveFrm = false;

        avoidedHex = new HashSet<Hexagon> ();

        foreach (UnitMovement u in playerUnt) 
        {
            avoidedHex.Add (u.currentHex);
            print("avoiding " + u.currentHex.name);
            foreach (Hexagon n in u.currentHex.neighbours)
            {
                if (n != null)
                {
                    avoidedHex.Add (n);
                    print("avoiding " + n.name);
                }
            }
        }
        foreach (UnitMovement u in AIUnt) 
        {
            u.ResetMovement ();

            u.targetHex = null;
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
            }
            else
            {
                f.active = false;
            }

            if (f.active == true) 
            {
                oneActiveFrm = true;

                resourcesHld.changeStores ("red", storesPerFrm, true);
            }
        }
        if (oneActiveFrm == false) 
        {
            resourcesHld.changeStores ("red", 3, true);
        }

        endTurnButton.SetActive (false);
        actionsMenuUI.SetActive(false);
        unitMenuUI.SetActive(false);
        cityMenuUI.SetActive(false);
        confirmationMenuUI.SetActive(false);
        behaviourTree.Evaluate ();
    }


    // Every player unit gets its movement limit reset, buildings are finished (if enough turns have passed), resources are collected (if enough turns have passed) and farms produce new stores if active.
    public void StartPlayerTurn () 
    {
        endTurnButton.SetActive(true);

        List<Hexagon> doneJob = new List<Hexagon> ();
        bool oneActiveFrm = false;

        foreach (UnitMovement u in playerUnt)
        {
            u.ResetMovement ();

            u.targetHex = null;
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
                oneActiveFrm = true;

                resourcesHld.changeStores ("blue", storesPerFrm, true);
            }
        }
        if (oneActiveFrm == false) 
        {
            resourcesHld.changeStores ("blue", 3, true);
        }
        if (buildingMenu.remainingTurnsToUpgrade == 0 && buildingMenu.upgrading==true)
        {
            buildingMenu.Upgrade ();
        }
        else if (buildingMenu.remainingTurnsToUpgrade > 0)
        {
            buildingMenu.remainingTurnsToUpgrade--;
        }
    }
}