
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<UnitMovement> playerUnt, AIUnt;
    public List<Farm> playerFrm, AIFrm;
    //public List<Collector> collectors;
    public List<Builder> playerBld, AIBld;
    /*public List<Farm> farms;

    private ResourcesHolder resourceMng;*/


    // Start is called before the first frame update.
    private void Start ()
    {
        instance = this;

        UnitMovement[] units = GameObject.FindObjectsOfType<UnitMovement> ();
        Builder[] builders = GameObject.FindObjectsOfType<Builder> ();

        playerUnt = new List<UnitMovement> ();
        AIUnt = new List<UnitMovement> ();
        playerBld = new List<Builder> ();
        AIBld = new List<Builder> ();

        foreach (UnitMovement u in units) 
        {
            //print(u.name);
            if (u.tag == "Ally")
            {
                playerUnt.Add (u);
            }
            else 
            {
                AIUnt.Add (u);
            }
        }
        foreach (Builder b in builders) 
        {
            //print(b.name);
            if (b.tag == "Ally")
            {
                playerBld.Add (b);
            }
            else
            {
                AIBld.Add (b);
            }
        }
        /*collectors = new List<Collector> ();
        builders = new List<Builder> ();
        farms = new List<Farm> ();
        resourceMng = this.GetComponent<ResourcesHolder> ();*/
    }


    // The AI does its actions and, after that, resources are given to each faction if the collectors have completed their tasks, buildings are built or upgraded if builders are done, and every active farm awards 10 stores to its corresponding 
    //faction.
    public void EndTurn () 
    {
        foreach (UnitMovement u in playerUnt) 
        {
            u.ResetMovement ();
        }
        foreach (Builder b in playerBld) 
        {
            if (b.working == true) 
            {
                b.remainingTrn -= 1;

                if (b.remainingTrn == 0) 
                {
                    b.working = false;

                    if (b.hex.environment != null) 
                    {
                        Destroy (b.hex.environment);
                    }
                    b.EndConstruction ("blue");
                }
            }
        }
        /*foreach (Builder b in builders)
        {
            if (b.building == true)
            {
                b.remainingTrn -= 1;

                if (b.remainingTrn == 0)
                {
                    b.building = false;

                    if (doneJob.Contains(b.hex) == false)
                    {
                        if (b.hex.environment != null)
                        {
                            Destroy(b.hex.environment);
                        }

                        b.hex.environment = Instantiate(b.construction, new Vector3(b.hex.CentroHexagono.position.x, b.hex.CentroHexagono.position.y, b.hex.CentroHexagono.position.z));

                        if (b.construction.tag.Contains("Farm") == true)
                        {
                            farms.Add(b.construction.GetComponent<Farm>());
                        }
                        doneJob.Add(b.hex);
                    }
                }
            }
        }*/

        // (AI does stuff)

        /*while (enemyFinished == false) { }

        List<Hexagon> doneJob = new List<Hexagon> ();

        foreach (Collector c in collectors) 
        {
            if (c.collecting == true) 
            {
                c.remainingTrn -= 1;

                if (c.remainingTrn == 0) 
                {
                    c.collecting = false;

                    if (doneJob.Contains (c.hex) == false) 
                    {
                        if (c.hex.GetHexagonType () == 2)
                        {
                            if (c.tag == "Enemy")
                            {
                                resourceMng.changeWood ("red", Random.Range (100, 201), true);
                            }
                            else
                            {
                                resourceMng.changeWood ("blue", Random.Range (100, 201), true);
                            }
                            doneJob.Add (c.hex);
                        }
                        else
                        {
                            if (c.tag == "Enemy")
                            {
                                resourceMng.changeMineral ("red", Random.Range (100, 201), true);
                            }
                            else
                            {
                                resourceMng.changeMineral ("blue", Random.Range (100, 201), true);
                            }
                            doneJob.Add (c.hex);
                        }
                    }
                }
            }
        }

        foreach (Builder b in builders)
        {
            if (b.building == true)
            {
                b.remainingTrn -= 1;

                if (b.remainingTrn == 0)
                {
                    b.building = false;

                    if (doneJob.Contains (b.hex) == false)
                    {
                        if (b.hex.environment != null) 
                        {
                            Destroy (b.hex.environment);
                        }

                        b.hex.environment = Instantiate (b.construction, new Vector3 (b.hex.CentroHexagono.position.x, b.hex.CentroHexagono.position.y, b.hex.CentroHexagono.position.z));

                        if (b.construction.tag.Contains ("Farm") == true)
                        {
                            farms.Add (b.construction.GetComponent<Farm> ());
                        }
                        doneJob.Add (b.hex);
                    }
                }
            }
        }

        foreach (Farm f in farms)
        {
            if (f.active == true)
            {
                if (f.tag == "EnemyFarm")
                {
                    resourceMng.changeStores ("red", 10, true); 
                }
                else 
                {
                    resourceMng.changeStores ("blue", 10, true);
                }
            }
        }*/
    }
}