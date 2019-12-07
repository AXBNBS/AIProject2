﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeScript : MonoBehaviour
{
    public Grid grid;
    public EnemyFunctions enemyFunction;

    private int capitalLevel = 1;

    public ActionNode CheckWood;
    public ActionNode CheckMineral;
    public ActionNode CheckStores;
    public ActionNode CheckPopulation;
    public ActionNode LevelUp;
    public ActionNode ReclutUnits;
    public ActionNode CheckMoveUnits;
    public ActionNode MoveUnits;
    public ActionNode BuildSettlement;
    public ActionNode BuildFarm;
    public ActionNode BuildTunnel;
    public ActionNode Collect;
    public ActionNode CheckActiveFarms;
    public ActionNode Attack;
    public ActionNode CheckVictory;
    public ActionNode UnderAttack;
    public ActionNode MovementUnits;
    public ActionNode SaveUnits;
    public ActionNode AttackBuilding;
    public ActionNode CheckTotalUnits;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);


    // Start is called before the first frame update
    void Start()
    {
        CheckWood = new ActionNode(checkWood);
        CheckMineral = new ActionNode(checkMineral);
        CheckStores = new ActionNode(checkStores);
        CheckPopulation = new ActionNode(checkPopulation);
        LevelUp = new ActionNode(levelUp);
        ReclutUnits = new ActionNode(reclutUnits);
        CheckMoveUnits = new ActionNode(checkMoveUnits);
        MoveUnits = new ActionNode(moveUnits);
        BuildSettlement = new ActionNode(buildSettlement);
        BuildFarm = new ActionNode(buildFarm);
        BuildTunnel = new ActionNode(buildTunnel);
        Collect = new ActionNode(collect);
        CheckActiveFarms = new ActionNode(checkActiveFarms);
        Attack = new ActionNode(attack);
        UnderAttack = new ActionNode(underAttack);
        MovementUnits = new ActionNode(movementUnits);
        SaveUnits = new ActionNode(saveUnits);
        AttackBuilding = new ActionNode(attackBuilding);
        CheckTotalUnits = new ActionNode(checkTotalUnits);
    }

    public void Evaluate()
    {
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        Debug.Log("The IA is doing things");
        Hexagon hexCapital = grid.hexagons[14, 48];
        Hexagon hexPlayerCapital = grid.hexagons[14, 0];
        yield return null;

        if (underAttack() == NodeStates.SUCCESS)
        {
            Debug.Log("IA bajo ataque");
            if (checkTotalUnits() == NodeStates.SUCCESS)
            {
                Debug.Log("IA tiene ejercito");
                GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < units.Length; i++)
                {
                    if (units[i] != null && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                    {
                        Debug.Log("IA almacena unidades");
                        saveUnits(0, hexCapital);
                    }
                }
            }
            else
            {
                Debug.Log("IA no tiene ejército");
                if (checkPopulation() == NodeStates.SUCCESS && checkStores(5) == NodeStates.SUCCESS)
                {
                    Debug.Log("IA tiene poblacion libre y suficientes viveres");
                    reclutUnits(1, hexCapital);
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (units[i] != null && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                        {
                            Debug.Log("IA almacena unidades");
                            saveUnits(0, hexCapital);
                        }
                    }
                }
                else
                {
                    Debug.Log("IA no puede producir tropas");
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (units[i] != null && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                        {
                            Debug.Log("IA almacena unidades");
                            saveUnits(0, hexCapital);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("IA segura");
            if (checkTotalUnits() == NodeStates.SUCCESS)
            {
                Debug.Log("IA tiene ejercito");
                GameObject[] cities = GameObject.FindGameObjectsWithTag("BlueSettlement");

                if (cities.Length < 3)
                {
                    print("IA ha detectado pocos asentamientos del jugador.");
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int u = 0; u < units.Length; u += 1)
                    {
                        if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Soldier")
                        {
                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, hexPlayerCapital) == NodeStates.SUCCESS)
                            {
                                print("IA ataca capital del jugador.");
                                attack(0, null, null, units[u].GetComponent<Unit>());
                            }
                        }
                    }
                }
                else
                {
                    print("IA decide atacar asentamiento.");
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int u = 0; u < units.Length; u += 1)
                    {
                        if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Soldier")
                        {
                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, cities[0].GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                            {
                                print("IA ataca asentamiento del jugador.");
                                attack(0, null, null, units[u].GetComponent<Unit>());
                            }
                        }
                    }
                }
            }
            else
            {
                print("IA no tiene ejército.");
                if (checkPopulation() == NodeStates.SUCCESS)
                {
                    print("IA tiene población de sobra.");
                    if (checkStores(5) == NodeStates.SUCCESS)
                    {
                        print("IA tiene víveres.");
                        if (reclutUnits(1, hexCapital) == NodeStates.SUCCESS)
                        {
                            print("IA produce tropas.");
                        }
                    }
                    else
                    {
                        print("IA no tiene víveres.");
                        if (checkActiveFarms() == NodeStates.SUCCESS)
                        {
                            print("IA tiene granjas inactivas.");
                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                            bool participa = false;
                            for (int u = 0; u < units.Length; u += 1)
                            {
                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Farmer")
                                {
                                    foreach (Farm f in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().AIFrm)
                                    {
                                        if (!f.active)
                                        {
                                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, f.GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                            {
                                                print("IA activa granja.");
                                                saveUnits(0, f.GetComponent<City>().currentHex);
                                                participa = true;
                                            }
                                        }
                                    }
                                }
                            }

                            if (!participa)
                            {
                                print("IA recluta granjeros.");
                                reclutUnits(7, hexCapital);
                            }
                        }
                        else
                        {
                            Debug.Log("IA no tiene granjas inactivas");
                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                            bool prepare = false;
                            for (int u = 0; u < units.Length; u += 1)
                            {
                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Worker")
                                {
                                    prepare = true;
                                    Debug.Log("IA tiene constructor");
                                    if (checkWood(50) == NodeStates.SUCCESS && checkMineral(50) == NodeStates.SUCCESS)
                                    {
                                        Debug.Log("IA tiene recursos para construir");
                                        buildFarm(0, null, null, units[u].GetComponent<Unit>());
                                    }
                                    else
                                    {
                                        Debug.Log("IA no tiene recursos");
                                        bool haveCollectors = false;
                                        for (int i = 0; i < units.Length; i += 1)
                                        {
                                            if (units[i] != null && units[i].GetComponent<Unit>().settings.occupation == "Collector")
                                            {
                                                Debug.Log("IA tiene recolector");
                                                haveCollectors = true;
                                                if (checkWood(50) == NodeStates.FAILURE && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA ha llevado un recolector a un bosque");
                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                }
                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == -1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35].neighbours[0]) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA ha llevado a un constructor a la montaña");
                                                    if (checkWood(100) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA puede construir tunel");
                                                        buildTunnel(0, null, null, units[u].GetComponent<Unit>());
                                                    }
                                                    else if (movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA lleva recolector al bosque");
                                                        collect(0, null, null, units[u].GetComponent<Unit>());
                                                    }
                                                }
                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == 1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35]) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA ha llevado recolector a la montaña");
                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                }
                                            }
                                        }
                                        if (!haveCollectors)
                                        {
                                            Debug.Log("IA no tiene recolectores");
                                            if (checkStores(10) == NodeStates.SUCCESS)
                                            {
                                                Debug.Log("IA tiene viveres");
                                                reclutUnits(5, hexCapital);
                                                Debug.Log("IA recluta a un recolector");
                                            }
                                        }
                                    }
                                }
                            }

                            if (!prepare)
                            {
                                Debug.Log("IA no tiene constructores");
                                if (checkStores(15) == NodeStates.SUCCESS)
                                {
                                    Debug.Log("IA puede crear constructores");
                                    reclutUnits(6, hexCapital);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("IA no tiene hueco para poblacion");
                    if (checkWood(hexCapital.GetCity().GetNeededWood()) == NodeStates.SUCCESS && checkMineral(hexCapital.GetCity().GetNeededMinerals()) == NodeStates.SUCCESS)
                    {
                        Debug.Log("IA puede subir de nivel la capital");
                        if (levelUp(0, hexCapital) == NodeStates.SUCCESS)
                        {
                            Debug.Log("IA ha subido de nivel la capital");
                            if (checkStores(5) == NodeStates.SUCCESS)
                            {
                                print("IA tiene víveres.");
                                if (reclutUnits(1, hexCapital) == NodeStates.SUCCESS)
                                {
                                    print("IA produce tropas.");
                                }
                            }
                            else
                            {
                                print("IA no tiene víveres.");
                                if (checkActiveFarms() == NodeStates.SUCCESS)
                                {
                                    print("IA tiene granjas inactivas.");
                                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                    bool participa = false;
                                    for (int u = 0; u < units.Length; u += 1)
                                    {
                                        if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Farmer")
                                        {
                                            foreach (Farm f in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().AIFrm)
                                            {
                                                if (!f.active)
                                                {
                                                    if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, f.GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                                    {
                                                        print("IA activa granja.");
                                                        saveUnits(0, f.GetComponent<City>().currentHex);
                                                        participa = true;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (!participa)
                                    {
                                        print("IA recluta granjeros.");
                                        reclutUnits(7, hexCapital);
                                    }
                                }
                                else
                                {
                                    Debug.Log("IA no tiene granjas inactivas");
                                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                    bool prepare = false;
                                    for (int u = 0; u < units.Length; u += 1)
                                    {
                                        if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Worker")
                                        {
                                            prepare = true;
                                            Debug.Log("IA tiene constructor");
                                            if (checkWood(50) == NodeStates.SUCCESS && checkMineral(50) == NodeStates.SUCCESS)
                                            {
                                                Debug.Log("IA tiene recursos para construir");
                                                buildFarm(0, null, null, units[u].GetComponent<Unit>());
                                            }
                                            else
                                            {
                                                Debug.Log("IA no tiene recursos");
                                                bool haveCollectors = false;
                                                for (int i = 0; i < units.Length; i += 1)
                                                {
                                                    if (units[i] != null && units[i].GetComponent<Unit>().settings.occupation == "Collector")
                                                    {
                                                        Debug.Log("IA tiene recolector");
                                                        haveCollectors = true;
                                                        if (checkWood(50) == NodeStates.FAILURE && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                        {
                                                            Debug.Log("IA ha llevado un recolector a un bosque");
                                                            collect(0, null, null, units[i].GetComponent<Unit>());
                                                        }
                                                        else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == -1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35].neighbours[0]) == NodeStates.SUCCESS)
                                                        {
                                                            Debug.Log("IA ha llevado a un constructor a la montaña");
                                                            if (checkWood(100) == NodeStates.SUCCESS)
                                                            {
                                                                Debug.Log("IA puede construir tunel");
                                                                buildTunnel(0, null, null, units[u].GetComponent<Unit>());
                                                            }
                                                            else if (movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                            {
                                                                Debug.Log("IA lleva recolector al bosque");
                                                                collect(0, null, null, units[u].GetComponent<Unit>());
                                                            }
                                                        }
                                                        else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == 1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35]) == NodeStates.SUCCESS)
                                                        {
                                                            Debug.Log("IA ha llevado recolector a la montaña");
                                                            collect(0, null, null, units[i].GetComponent<Unit>());
                                                        }
                                                    }
                                                }
                                                if (!haveCollectors)
                                                {
                                                    Debug.Log("IA no tiene recolectores");
                                                    if (checkStores(10) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA tiene viveres");
                                                        reclutUnits(5, hexCapital);
                                                        Debug.Log("IA recluta a un recolector");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (!prepare)
                                    {
                                        Debug.Log("IA no tiene constructores");
                                        if (checkStores(15) == NodeStates.SUCCESS)
                                        {
                                            Debug.Log("IA puede crear constructores");
                                            reclutUnits(6, hexCapital);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("IA no puede subir el nivel de la capital");
                        bool settlementLevelUp = false;
                        GameObject[] citiesRed = GameObject.FindGameObjectsWithTag("RedSettlement");
                        for (int x = 0; x < citiesRed.Length; x++)
                        {
                            if (citiesRed[x].GetComponent<City>().GetLevel() < 3 && checkWood(citiesRed[x].GetComponent<City>().GetNeededWood()) == NodeStates.SUCCESS && checkWood(citiesRed[x].GetComponent<City>().GetNeededMinerals()) == NodeStates.SUCCESS)
                            {
                                Debug.Log("IA puede subir de nivel un asentamiento");
                                settlementLevelUp = true;
                                if (levelUp(0, citiesRed[x].GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                {
                                    Debug.Log("IA ha subido de nivel");
                                    if (checkStores(5) == NodeStates.SUCCESS)
                                    {
                                        print("IA tiene víveres.");
                                        if (reclutUnits(1, hexCapital) == NodeStates.SUCCESS)
                                        {
                                            print("IA produce tropas.");
                                        }
                                    }
                                    else
                                    {
                                        print("IA no tiene víveres.");
                                        if (checkActiveFarms() == NodeStates.SUCCESS)
                                        {
                                            print("IA tiene granjas inactivas.");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool participa = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Farmer")
                                                {
                                                    foreach (Farm f in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().AIFrm)
                                                    {
                                                        if (!f.active)
                                                        {
                                                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, f.GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                                            {
                                                                print("IA activa granja.");
                                                                saveUnits(0, f.GetComponent<City>().currentHex);
                                                                participa = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!participa)
                                            {
                                                print("IA recluta granjeros.");
                                                reclutUnits(7, hexCapital);
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("IA no tiene granjas inactivas");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool prepare = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Worker")
                                                {
                                                    prepare = true;
                                                    Debug.Log("IA tiene constructor");
                                                    if (checkWood(50) == NodeStates.SUCCESS && checkMineral(50) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA tiene recursos para construir");
                                                        buildFarm(0, null, null, units[u].GetComponent<Unit>());
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("IA no tiene recursos");
                                                        bool haveCollectors = false;
                                                        for (int i = 0; i < units.Length; i += 1)
                                                        {
                                                            if (units[i] != null && units[i].GetComponent<Unit>().settings.occupation == "Collector")
                                                            {
                                                                Debug.Log("IA tiene recolector");
                                                                haveCollectors = true;
                                                                if (checkWood(50) == NodeStates.FAILURE && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado un recolector a un bosque");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == -1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35].neighbours[0]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado a un constructor a la montaña");
                                                                    if (checkWood(100) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA puede construir tunel");
                                                                        buildTunnel(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                    else if (movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA lleva recolector al bosque");
                                                                        collect(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == 1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado recolector a la montaña");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                            }
                                                        }
                                                        if (!haveCollectors)
                                                        {
                                                            Debug.Log("IA no tiene recolectores");
                                                            if (checkStores(10) == NodeStates.SUCCESS)
                                                            {
                                                                Debug.Log("IA tiene viveres");
                                                                reclutUnits(5, hexCapital);
                                                                Debug.Log("IA recluta a un recolector");
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!prepare)
                                            {
                                                Debug.Log("IA no tiene constructores");
                                                if (checkStores(15) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA puede crear constructores");
                                                    reclutUnits(6, hexCapital);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (!settlementLevelUp)
                        {
                            Debug.Log("IA no puede subir de nivel un asentamiento");
                            if (checkWood(400) == NodeStates.SUCCESS && checkMineral(150) == NodeStates.SUCCESS)
                            {
                                Debug.Log("IA tiene materiales para construir asentamiento");
                                bool haveWorkers = false;
                                GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                for (int j = 0; j < units.Length; j++) 
                                {
                                    if (units[j].GetComponent<Unit>().settings.occupation == "Worker")
                                    {
                                        Debug.Log("IA tiene constructores");
                                        haveWorkers = true;
                                        buildSettlement(0, null, null, units[j].GetComponent<Unit>());
                                        Debug.Log("IA ha construido asentamiento");
                                        break;
                                    }
                                }

                                if (!haveWorkers)
                                {
                                    Debug.Log("IA no tiene constructores");
                                    if (checkStores(15) == NodeStates.SUCCESS)
                                    {
                                        Debug.Log("IA tiene suficentes viveres");
                                        reclutUnits(6, hexCapital);
                                        Debug.Log("IA ha reclutado un constructor");
                                    } else
                                    {
                                        print("IA no tiene víveres.");
                                        if (checkActiveFarms() == NodeStates.SUCCESS)
                                        {
                                            print("IA tiene granjas inactivas.");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool participa = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Farmer")
                                                {
                                                    foreach (Farm f in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().AIFrm)
                                                    {
                                                        if (!f.active)
                                                        {
                                                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, f.GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                                            {
                                                                print("IA activa granja.");
                                                                saveUnits(0, f.GetComponent<City>().currentHex);
                                                                participa = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!participa)
                                            {
                                                print("IA recluta granjeros.");
                                                reclutUnits(7, hexCapital);
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("IA no tiene granjas inactivas");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool prepare = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Worker")
                                                {
                                                    prepare = true;
                                                    Debug.Log("IA tiene constructor");
                                                    if (checkWood(50) == NodeStates.SUCCESS && checkMineral(50) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA tiene recursos para construir");
                                                        buildFarm(0, null, null, units[u].GetComponent<Unit>());
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("IA no tiene recursos");
                                                        bool haveCollectors = false;
                                                        for (int i = 0; i < units.Length; i += 1)
                                                        {
                                                            if (units[i] != null && units[i].GetComponent<Unit>().settings.occupation == "Collector")
                                                            {
                                                                Debug.Log("IA tiene recolector");
                                                                haveCollectors = true;
                                                                if (checkWood(50) == NodeStates.FAILURE && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado un recolector a un bosque");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == -1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35].neighbours[0]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado a un constructor a la montaña");
                                                                    if (checkWood(100) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA puede construir tunel");
                                                                        buildTunnel(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                    else if (movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA lleva recolector al bosque");
                                                                        collect(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == 1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado recolector a la montaña");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                            }
                                                        }
                                                        if (!haveCollectors)
                                                        {
                                                            Debug.Log("IA no tiene recolectores");
                                                            if (checkStores(10) == NodeStates.SUCCESS)
                                                            {
                                                                Debug.Log("IA tiene viveres");
                                                                reclutUnits(5, hexCapital);
                                                                Debug.Log("IA recluta a un recolector");
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!prepare)
                                            {
                                                Debug.Log("IA no tiene constructores");
                                                if (checkStores(15) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA puede crear constructores");
                                                    reclutUnits(6, hexCapital);
                                                }
                                            }
                                        }
                                    }
                                }
                            } else
                            {
                                Debug.Log("IA no tiene materiales para construir asentamiento");
                                GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                bool haveRecollectores = false;
                                for (int j = 0; j < units.Length; j++)
                                {
                                    if (units[j].GetComponent<Unit>().settings.occupation == "Collector")
                                    {
                                        Debug.Log("IA tiene recolector");
                                        haveRecollectores = true;
                                        if (movementUnits(0, units[j].GetComponent<Unit>().movement.currentHex(), grid.forestsArray[grid.forestsArray.Length/2 + 2]) == NodeStates.SUCCESS)
                                        {
                                            Debug.Log("IA ha llevado recolector a un bosque");
                                            collect(0, null, null, units[j].GetComponent<Unit>());
                                        }
                                    }
                                }

                                if (!haveRecollectores)
                                {
                                    Debug.Log("IA no mantiene recolectores");
                                    if (checkStores(10) == NodeStates.SUCCESS)
                                    {
                                        Debug.Log("IA puede obtener un recolector");
                                        reclutUnits(5, hexCapital);
                                        Debug.Log("IA recluta un recolector");
                                    } else
                                    {
                                        print("IA no tiene víveres.");
                                        if (checkActiveFarms() == NodeStates.SUCCESS)
                                        {
                                            print("IA tiene granjas inactivas.");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool participa = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Farmer")
                                                {
                                                    foreach (Farm f in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().AIFrm)
                                                    {
                                                        if (!f.active)
                                                        {
                                                            if (movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, f.GetComponent<City>().currentHex) == NodeStates.SUCCESS)
                                                            {
                                                                print("IA activa granja.");
                                                                saveUnits(0, f.GetComponent<City>().currentHex);
                                                                participa = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!participa)
                                            {
                                                print("IA recluta granjeros.");
                                                reclutUnits(7, hexCapital);
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("IA no tiene granjas inactivas");
                                            GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                                            bool prepare = false;
                                            for (int u = 0; u < units.Length; u += 1)
                                            {
                                                if (units[u] != null && units[u].GetComponent<Unit>().settings.occupation == "Worker")
                                                {
                                                    prepare = true;
                                                    Debug.Log("IA tiene constructor");
                                                    if (checkWood(50) == NodeStates.SUCCESS && checkMineral(50) == NodeStates.SUCCESS)
                                                    {
                                                        Debug.Log("IA tiene recursos para construir");
                                                        buildFarm(0, null, null, units[u].GetComponent<Unit>());
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("IA no tiene recursos");
                                                        bool haveCollectors = false;
                                                        for (int i = 0; i < units.Length; i += 1)
                                                        {
                                                            if (units[i] != null && units[i].GetComponent<Unit>().settings.occupation == "Collector")
                                                            {
                                                                Debug.Log("IA tiene recolector");
                                                                haveCollectors = true;
                                                                if (checkWood(50) == NodeStates.FAILURE && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado un recolector a un bosque");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == -1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[u].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35].neighbours[0]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado a un constructor a la montaña");
                                                                    if (checkWood(100) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA puede construir tunel");
                                                                        buildTunnel(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                    else if (movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.forestsArray[grid.forestsArray.Length / 2 + 2]) == NodeStates.SUCCESS)
                                                                    {
                                                                        Debug.Log("IA lleva recolector al bosque");
                                                                        collect(0, null, null, units[u].GetComponent<Unit>());
                                                                    }
                                                                }
                                                                else if (checkMineral(50) == NodeStates.FAILURE && grid.hexagons[21, 35].GetHexagonType() == 1 && grid.hexagons[21, 35].GetMountain() == true && movementUnits(0, units[i].GetComponent<Unit>().movement.currentHex, grid.hexagons[21, 35]) == NodeStates.SUCCESS)
                                                                {
                                                                    Debug.Log("IA ha llevado recolector a la montaña");
                                                                    collect(0, null, null, units[i].GetComponent<Unit>());
                                                                }
                                                            }
                                                        }
                                                        if (!haveCollectors)
                                                        {
                                                            Debug.Log("IA no tiene recolectores");
                                                            if (checkStores(10) == NodeStates.SUCCESS)
                                                            {
                                                                Debug.Log("IA tiene viveres");
                                                                reclutUnits(5, hexCapital);
                                                                Debug.Log("IA recluta a un recolector");
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!prepare)
                                            {
                                                Debug.Log("IA no tiene constructores");
                                                if (checkStores(15) == NodeStates.SUCCESS)
                                                {
                                                    Debug.Log("IA puede crear constructores");
                                                    reclutUnits(6, hexCapital);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private NodeStates checkWood(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkWood())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMineral(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkMineral())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkStores(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkStores())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkPopulation(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkPopulation())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates levelUp(int n, Hexagon hex, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.levelUp(hex) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates reclutUnits(int type, Hexagon hex, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.reclutUnits(type, hex) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMoveUnits(int type, Hexagon hex, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkMoveUnits(type, hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates moveUnits(int type, Hexagon hex, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.moveUnits(type, hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildSettlement(int n, Hexagon hex, Hexagon hex2, Unit unit)
    {
        if (enemyFunction.buildSettlement(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildFarm(int n, Hexagon hex, Hexagon hex2, Unit unit)
    {
        if (enemyFunction.buildFarm(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildTunnel(int n, Hexagon hex, Hexagon hex2, Unit unit)
    {
        if (enemyFunction.buildTunnel(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates collect(int n, Hexagon hex, Hexagon hex2, Unit unit)
    {
        if (enemyFunction.collect(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkActiveFarms(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkActiveFarms())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates attack(int n, Hexagon hex, Hexagon hex2, Unit unit)
    {
        if (enemyFunction.attack(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates underAttack(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.underAttack())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates movementUnits(int n, Hexagon startHex, Hexagon finalHex, Unit unit = null)
    {
        if (enemyFunction.movementUnits(startHex, finalHex) == 1)
            return NodeStates.SUCCESS;
        else if (enemyFunction.movementUnits(startHex, finalHex) == -1)
            return NodeStates.FAILURE;
        else
            return NodeStates.RUNNING;
    }

    private NodeStates saveUnits(int n, Hexagon hex, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.saveUnits(hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates attackBuilding(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.attackBuilding())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkTotalUnits(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null)
    {
        if (enemyFunction.checkTotalUnits())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }
}
