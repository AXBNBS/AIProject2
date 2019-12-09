using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFunctions : MonoBehaviour
{
    public GameObject orcPrefab;
    public UnitSettings orcSettings;
    public GameObject rollPrefab;
    public UnitSettings rollSettings;
    public GameObject goblinPrefab;
    public UnitSettings goblinSettings;
    public GameObject trollPrefab;
    public UnitSettings trollSettings;
    public GameObject cuctanyaPrefab;
    public UnitSettings cuctanyaSettings;
    public GameObject puppetPrefab;
    public UnitSettings puppetSettings;
    public GameObject witcherPrefab;
    public UnitSettings witcherSettings;

    private ResourcesHolder resourcesHolder;
    private City capital;

    public GameObject settlement, farm, tunnel;
    [SerializeField] private int settlementMin, settlementWod, farmMin, farmWod, tunnelMin, tunnelWod;

    public Grid grid;

    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        resourcesHolder = GameObject.FindObjectOfType<ResourcesHolder>();
        capital = GameObject.FindGameObjectWithTag("RedCapital").GetComponent<City>();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public bool checkWood(int n)
    {
        int enoughWood = 0;
        if (capital.GetLevel() == 1)
        {
            enoughWood = 400;
        } else if (capital.GetLevel() == 2)
        {
            enoughWood = 600;
        }
        else if (capital.GetLevel() == 3)
        {
            enoughWood = 800;
        } else if (capital.GetLevel() == 4)
        {
            enoughWood = 1000;
        }
        else if (capital.GetLevel() == 5)
        {
            enoughWood = 1200;
        }

        if (n != 0)
            enoughWood = n;

        if (enoughWood < resourcesHolder.GetRedWood())
            return true;
        else
            return false;
    }

    public bool checkMineral(int n)
    {
        int enoughMineral = 0;
        if (capital.GetLevel() == 1)
        {
            enoughMineral = 150;
        }
        else if (capital.GetLevel() == 2)
        {
            enoughMineral = 250;
        }
        else if (capital.GetLevel() == 3)
        {
            enoughMineral = 350;
        }
        else if (capital.GetLevel() == 4)
        {
            enoughMineral = 475;
        }
        else if (capital.GetLevel() == 5)
        {
            enoughMineral = 600;
        }

        if (n != 0)
            enoughMineral = n;

        if (enoughMineral < resourcesHolder.GetRedMineral())
            return true;
        else
            return false;
    }

    public bool checkStores(int n)
    {
        int enoughStores = 0;
        if (capital.GetLevel() == 1)
        {
            enoughStores = 25;
        }
        else if (capital.GetLevel() == 2)
        {
            enoughStores = 50;
        }
        else if (capital.GetLevel() == 3)
        {
            enoughStores = 75;
        }
        else if (capital.GetLevel() == 4)
        {
            enoughStores = 100;
        }
        else if (capital.GetLevel() == 5)
        {
            enoughStores = 125;
        }

        if (n != 0)
            enoughStores = n;

        if (enoughStores < resourcesHolder.GetRedStores())
            return true;
        else
            return false;
    }

    public bool checkPopulation()
    {
        int difference = resourcesHolder.GetRedTotalPopulation() - resourcesHolder.GetRedCurrentPopulation();

        if (difference < 1)
        {
            return false;
        } else
        {
            return true;
        }
    }

    public int levelUp(Hexagon hex)
    {
        if ((hex.GetCity().GetCityType() == "Settlement" && hex.GetCity().GetLevel() < 3) || (hex.GetCity().GetLevel() < 5 && hex.GetCity().GetCityType() == "Capital"))
        {
            resourcesHolder.changeWood("Red", hex.GetCity().GetNeededWood(), false);
            resourcesHolder.changeMineral("Red", hex.GetCity().GetNeededMinerals(), false);
            if (hex.GetCity().GetCityType() == "Sawmill" || hex.GetCity().GetCityType() == "Farm" || hex.GetCity().GetCityType() == "Mina" || hex.GetCity().GetCityType() == "Settlement")
            {
                resourcesHolder.changeTotalPopulation("Red", 3, true);
            }
            else
            {
                resourcesHolder.changeTotalPopulation("Red", 5, true);
            }

            int orcs = hex.GetCity().GetOrcs();
            int roll = hex.GetCity().GetPanecillos();
            int goblins = hex.GetCity().GetGoblins();
            int trolls = hex.GetCity().GetTrolls();
            int cuctanyas = hex.GetCity().GetCuctanyas();
            int puppets = hex.GetCity().GetPuppets();
            int witchers = hex.GetCity().GetWitchers();

            string type = hex.GetCity().GetCityType();

            GameObject nextLevel = hex.GetCity().nextLevel;

            Destroy(hex.environment);

            hex.environment = Instantiate(nextLevel, new Vector3(hex.CentroHexagono.position.x, hex.CentroHexagono.position.y, hex.CentroHexagono.position.z), Quaternion.identity);

            hex.SetCity(hex.environment.GetComponent<City>());
            hex.GetCity().SetCityType(type);
            hex.GetCity().SetCitySide("Red");

            hex.GetCity().AddUnits("Human", orcs, orcs);
            hex.GetCity().AddUnits("Cat", roll, roll * 1.5f);
            hex.GetCity().AddUnits("Elf", goblins, goblins);
            hex.GetCity().AddUnits("Dwarf", trolls, trolls);
            hex.GetCity().AddUnits("Twii", cuctanyas, cuctanyas * 2);
            hex.GetCity().AddUnits("Craftsman", puppets, puppets * 0.5f);
            hex.GetCity().AddUnits("Turroncito", witchers, witchers);

            if (hex.GetCity().GetCityType() == "Capital")
                capital = hex.GetCity();

            return 1; //Se puede aumentar de nivel
        } else
        {
            return 0; //Ya tiene el nivel maximo
        }
    }

    public int reclutUnits(int type, Hexagon hex)
    {
        Hexagon generate = null;
        for (int i = 0; i < hex.neighbours.Length; i++)
        {
            if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
            {
                generate = hex.neighbours[i];
                break;
            }
        }

        if (generate != null)
        {
            GameObject train;
            if (type == 1)
            {
                train = Instantiate(orcPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", orcSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            } else if (type == 2)
            {
                train = Instantiate(rollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", rollSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            } else if (type == 3)
            {
                train = Instantiate(goblinPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", goblinSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            } else if (type == 4)
            {
                train = Instantiate(trollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", trollSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            } else if (type == 5)
            {
                train = Instantiate(cuctanyaPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", cuctanyaSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
                gameManager.GetComponent<GameManager>().AICll.Add(train.GetComponent<Collector>());
            } else if (type == 6)
            {
                train = Instantiate(puppetPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", puppetSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
                gameManager.GetComponent<GameManager>().AIBld.Add(train.GetComponent<Builder>());
            } else
            {
                train = Instantiate(witcherPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", witcherSettings.stores, false);
                train.GetComponent<UnitMovement>().target = generate.transform.position;         
            }
            gameManager.GetComponent<GameManager>().AIUnt.Add(train.GetComponent<UnitMovement>());
            return 1; //Va bien
        } else
        {
            return 0; //No tiene espacio para moverse
        }
    }

    public bool checkMoveUnits(int type, Hexagon hex)
    {
        City c = hex.GetCity();
        if (c.GetOrcs() != 0 && type == 1)
        {
            return true;
        } else if (c.GetPanecillos() != 0 && type == 2)
        {
            return true;
        } else if (c.GetGoblins() != 0 && type == 3)
        {
            return true;
        } else if (c.GetTrolls() != 0 && type == 4)
        {
            return true;
        } else if (c.GetCuctanyas() != 0 && type == 5)
        {
            return true;
        } else if (c.GetPuppets() != 0 && type == 6)
        {
            return true;
        } else if (c.GetWitchers() != 0 && type == 7)
        {
            return true;
        } else
        {
            return false; //No se tiene esa unidad en la ciudad
        }
    }

    public bool moveUnits(int type, Hexagon hex)
    {
        Hexagon generate = null;
        for (int i = 0; i < hex.neighbours.Length; i++)
        {
            if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
            {
                generate = hex.neighbours[i];
                break;
            }
        }

        if (generate != null)
        {
            GameObject train;
            if (type == 1)
            {
                hex.GetCity().AddUnits(orcSettings.race, -1, -orcSettings.defense);
                train = Instantiate(orcPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            }
            else if (type == 2)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                train = Instantiate(rollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            }
            else if (type == 3)
            {
                hex.GetCity().AddUnits(goblinSettings.race, -1, -goblinSettings.defense);
                train = Instantiate(goblinPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            }
            else if (type == 4)
            {
                hex.GetCity().AddUnits(trollSettings.race, -1, -trollSettings.defense);
                train = Instantiate(trollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            }
            else if (type == 5)
            {
                hex.GetCity().AddUnits(cuctanyaSettings.race, -1, -cuctanyaSettings.defense);
                train = Instantiate(cuctanyaPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
                gameManager.GetComponent<GameManager>().AICll.Add(train.GetComponent<Collector>());
            }
            else if (type == 6)
            {
                hex.GetCity().AddUnits(puppetSettings.race, -1, -puppetSettings.defense);
                train = Instantiate(puppetPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
                gameManager.GetComponent<GameManager>().AIBld.Add(train.GetComponent<Builder>());
            }
            else
            {
                hex.GetCity().AddUnits(witcherSettings.race, -1, -witcherSettings.defense);
                train = Instantiate(witcherPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                train.GetComponent<UnitMovement>().target = generate.transform.position;
            }
            gameManager.GetComponent<GameManager>().AIUnt.Add(train.GetComponent<UnitMovement>());
            return true;//Va bien
        } else
        {
            return false; //Si no hay hueco para generar
        }
    }

    public bool buildSettlement(Unit unit)
    {
        if (unit.GetOccupation() == "Worker" && unit.movement.currentHex.GetIsBuilded() == false)
        {
            foreach (Hexagon h in unit.movement.currentHex.neighbours)
            {
                if (h != null && h.GetIsBuilded())
                {
                    return false; //No se puede construir (depende como busque la IA donde moverse esto puede no ser necesario)
                }
            }
            if (resourcesHolder.GetRedWood() >= settlementWod && resourcesHolder.GetRedMineral() >= settlementMin)
            {
                resourcesHolder.changeWood("Red", settlementWod, false);
                resourcesHolder.changeMineral("Red", settlementMin, false);
                unit.GetComponent<Builder>().BeginConstruction(settlement, settlementMin, settlementWod);
                return true; //Ha empezado a construir
            }
        }
        return false;//La unidad no era un constructor
    }

    public bool buildFarm(Unit unit)
    {
        if (unit.GetOccupation() == "Worker" && unit.movement.currentHex.GetIsBuilded() == false)
        {
            foreach (Hexagon h in unit.movement.currentHex.neighbours)
            {
                if (h != null && h.GetIsBuilded())
                {
                    return false; //No se puede construir (depende como busque la IA donde moverse esto puede no ser necesario)
                }
            }
            if (resourcesHolder.GetRedWood() >= farmWod && resourcesHolder.GetRedMineral() >= farmMin)
            {
                resourcesHolder.changeWood("Red", farmWod, false);
                resourcesHolder.changeMineral("Red", farmMin, false);
                unit.GetComponent<Builder>().BeginConstruction(farm, farmMin, farmWod);
                return true; //Ha empezado a construir
            }
        }
        return false;//La unidad no era un constructor
    }

    public bool buildTunnel(Unit unit)
    {
        if (unit.GetOccupation() == "Worker")
        {
            for (int i = 0; i < unit.movement.currentHex.neighbours.Length; i++)
            {
                if (unit.movement.currentHex.neighbours[i] != null && unit.movement.currentHex.neighbours[i].GetMountain() == true && unit.movement.currentHex.neighbours[i].GetHexagonType() == -1)
                {
                    if (resourcesHolder.GetRedWood() >= tunnelWod && resourcesHolder.GetRedMineral() >= tunnelMin)
                    {
                        resourcesHolder.changeWood("Red", tunnelWod, false);
                        resourcesHolder.changeMineral("Red", tunnelMin, false);
                        unit.GetComponent<Builder>().BeginConstruction(tunnel, tunnelMin, tunnelWod);
                        return true;//Empieza a crear el tunel
                    }
                }
            }
        }
        return false;//No se podia construir
    }

    public bool collect(Unit unit)
    {
        if (unit.GetOccupation() == "Collector" && unit.movement.currentHex.GetRemainingTurnsToCollect() <= 0)
        {
            unit.GetComponent<Collector>().BeginCollect();
            return true;//Puede recolectar
        }
        return false;//No puede recolectar
    }

    public bool checkActiveFarms()
    {
        foreach (Farm f in gameManager.GetComponent<GameManager>().AIFrm)
        {
            if (!f.active)
                return true;
        }
        return false;
    }

    public bool attack(Unit unit)
    {
        int range = Mathf.RoundToInt(unit.GetSpeed());
        Hexagon originHex = unit.movement.currentHex;
        bool canWin = false;
        Hexagon destinyHex = null;
        Queue<Hexagon> q = new Queue<Hexagon>();
        q.Enqueue(originHex);

        while (q.Count > 0)
        {
            Hexagon element = q.Dequeue();
            if (element != null && element.presentUnt != 0 && element.UnitsPlaced()[0].tag == "Ally")
            {
                canWin = checkVictory(element, unit);
                destinyHex = element;
                break;
            }

            if (element.neighbours.Length > 0)
            {
                foreach (Hexagon h in element.neighbours)
                {
                    if (!q.Contains(h))
                        q.Enqueue(h);
                }
            }
        }

        if (canWin)
        {
            //movemos las unidades al hexagono objetivo y destruimos las unidades azules 
            unit.movement.targetHex = destinyHex;
            foreach (Hexagon h in unit.movement.currentHex.neighbours)
            {
                if (h != null && h == destinyHex)
                {
                    unit.SendMessage("Fight", unit.movement.targetHex);
                    unit.movement.FindPathTo(destinyHex);
                    return true;
                }
            }
            unit.movement.FindPathTo(destinyHex);
            return true;
        }
        return false;
    }

    private bool checkVictory(Hexagon hex, Unit unit)
    {
        if (hex.UnitsPlaced() == null)
        {
            return false;
        }
        float alliesPower = 0;
        float enemiesPower = 0;
        bool addCurrentHexagon = true;
        for (int i = 0; i < hex.neighbours.Length; i++)
        {
            if (hex.neighbours[i] != null)
            {
                UnitMovement[] units = hex.neighbours[i].UnitsPlaced();
                if (units != null && units.Length != 0)
                {
                    for (int j = 0; j < units.Length; j++)
                    {
                        if (units[j].tag == "Enemy")
                        {
                            enemiesPower += units[j].stats.attack;
                        }
                        else if (units[j].tag == "Ally")
                        {
                            alliesPower += units[j].stats.attack;
                        }
                    }
                }
            }
            if (hex.neighbours[i] == unit.movement.currentHex)
            {
                addCurrentHexagon = false;
            }
        }
        UnitMovement[] localUnits = hex.UnitsPlaced();
        if (localUnits != null && localUnits.Length != 0)
        {
            for (int j = 0; j < localUnits.Length; j++)
            {
                if (localUnits[j].tag == "Enemy")
                {
                    enemiesPower += localUnits[j].stats.attack;
                }
                else if (localUnits[j].tag == "Ally")
                {
                    alliesPower += localUnits[j].stats.attack;
                }
            }
        }
        UnitMovement[] allies = unit.movement.GetAllies();
        if (addCurrentHexagon)
        {
            for (int j = 0; j < allies.Length; j++)
            {
                if (allies[j].tag == "Enemy")
                {
                    enemiesPower += allies[j].stats.attack;
                }
                else if (allies[j].tag == "Ally")
                {
                    alliesPower += allies[j].stats.attack;
                }
            }
        }
        if (Random.Range(0, 10) >= 5 + alliesPower - enemiesPower)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool underAttack() //Cuando este el número de mapa correcto se pondra el hexagono correcto de entonces
    {
        int total = 0;
        for (int i = 25; i < 49; i++)
        {
            for (int j = 0; j < 29; j++)
            {
                if (grid.hexagons[j, i].units[0] != null && grid.hexagons[j, i].presentUnt != 0 && grid.hexagons[j, i].units[0].tag == "Ally" && grid.hexagons[j, i].units[0].stats.occupation == "Soldier")
                {
                    for (int x = 0; x < grid.hexagons[j, i].presentUnt; x++)
                    {
                        total++;
                    }
                }
            }
        }

        if (total > 5)
        {
            return true; //Esta bajo ataque
        } else
        {
            return false; //No esta bajo ataque
        }
    }

    // Returns -1 the unit hasn't moved, 0 if it has moved but still hasn't reached its target or +1 if it's arrived to its target.
    public int movementUnits(Hexagon startHex, Hexagon finalHex)
    {

        //StartCoroutine("waitToReturn");
        UnitMovement[] units = startHex.UnitsPlaced();
        if (units.Length > 0 && units[0] != null)
        {
            int result = units[0].FindPathTo(finalHex);
            return result;
        }
        else
        {
            return -1;
        }
    }

    // Returs true if units can be saved at the city, otherwise returns false.
    public bool saveUnits (Hexagon hex)
    {
        UnitMovement[] units = hex.UnitsPlaced();
        if (hex.GetCity().GetCapacity () >= units.Length)
        {
            hex.GetCity().AddUnits (units[0].stats.race, units.Length, units[0].stats.defense * units.Length);
            units[0].currentHex.EmptyHexagon ();
            for (int u = 0; u < units.Length; u += 1)
            {
                if (units[u].stats.occupation == "Worker")
                {
                    gameManager.GetComponent<GameManager>().AIBld.Remove(units[u].GetComponent<Builder>());
                }
                else if(units[u].stats.occupation == "Collector")
                {
                    gameManager.GetComponent<GameManager>().AICll.Remove(units[u].GetComponent<Collector>());
                }
                gameManager.GetComponent<GameManager>().AIUnt.Remove(units[u]);
                Destroy (units[u].gameObject);
            }

            return true;
        }
        else 
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            GameObject train;

            for (int i = 0; i < units.Length; i++)
            {
                if (units[i].stats.race == "Orc")
                {
                    train = Instantiate(orcPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Roll")
                {
                    train = Instantiate(rollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Goblin")
                {
                    train = Instantiate(goblinPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Troll")
                {
                    train = Instantiate(trollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Cuctanya")
                {
                    train = Instantiate(cuctanyaPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Puppet")
                {
                    train = Instantiate(puppetPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
                else if (units[i].stats.race == "Witcher")
                {
                    train = Instantiate(witcherPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                    if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(train.GetComponent<Builder>());
                    }
                    else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(train.GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(train.GetComponent<UnitMovement>());
                }
            }

            units[0].currentHex.EmptyHexagon();
            for (int u = 0; u < units.Length; u += 1)
            {
                if (units[u].stats.occupation == "Worker")
                {
                    gameManager.GetComponent<GameManager>().AIBld.Remove(units[u].GetComponent<Builder>());
                }
                else if (units[u].stats.occupation == "Collector")
                {
                    gameManager.GetComponent<GameManager>().AICll.Remove(units[u].GetComponent<Collector>());
                }
                gameManager.GetComponent<GameManager>().AIUnt.Remove(units[u]);
                Destroy(units[u].gameObject);
            }

            return false;
        }
    }

    public bool attackBuilding()
    {
        return true;
    }

    public bool checkTotalUnits()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
        if (units.Length > 4)
            return true;
        else
            return false;
    }
}
