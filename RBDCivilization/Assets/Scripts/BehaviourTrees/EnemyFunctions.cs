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

    // Start is called before the first frame update
    void Start()
    {
        resourcesHolder = GameObject.FindObjectOfType<ResourcesHolder>();
        capital = GameObject.FindGameObjectWithTag("RedCapital").GetComponent<City>();
    }

    public bool checkWood(int n = 0)
    {
        int enoughWood = 0;
        if (capital.GetLevel() == 1)
        {
            enoughWood = 400;
        } else if (capital.GetLevel() == 2)
        {
            enoughWood = 600;
        }
        else if(capital.GetLevel() == 3)
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

    public bool checkMineral(int n = 0)
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

    public bool checkStores(int n = 0)
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

    public int levelUp (Hexagon hex)
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

    public int reclutUnits (int type, Hexagon hex)
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
            if (type == 1)
            {
                GameObject train = Instantiate(orcPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", orcSettings.stores, false);
                return 1; //Va bien
            } else if (type == 2)
            {
                GameObject train = Instantiate(rollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", rollSettings.stores, false);
                return 1; //Va bien
            } else if (type == 3)
            {
                GameObject train = Instantiate(goblinPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", goblinSettings.stores, false);
                return 1; //Va bien
            } else if (type == 4)
            {
                GameObject train = Instantiate(trollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", trollSettings.stores, false);
                return 1; //Va bien
            } else if (type == 5)
            {
                GameObject train = Instantiate(cuctanyaPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", cuctanyaSettings.stores, false);
                return 1; //Va bien
            } else if (type == 6)
            {
                GameObject train = Instantiate(puppetPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", puppetSettings.stores, false);
                return 1; //Va bien
            } else if (type == 7)
            {
                GameObject train = Instantiate(witcherPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                resourcesHolder.changeCurrentPopulation("Red", 1, true);
                resourcesHolder.changeStores("Red", witcherSettings.stores, false);
                return 1; //Va bien
            } else
            {
                return 0; //Por si acaso ponemos un valor raro
            }
        } else
        {
            return 0; //No tiene espacio para moverse
        }
    }

    public bool checkMoveUnits(int type, Hexagon hex)
    {
        City c = hex.GetCity();
        if (c.GetHumans() != 0 && type == 1)
        {
            return true;
        } else if (c.GetElves() != 0 && type == 2)
        {
            return true;
        } else if (c.GetDwarfs() != 0 && type == 3)
        {
            return true;
        } else if (c.GetCats() != 0 && type == 4)
        {
            return true;
        } else if (c.GetTwiis() != 0 && type == 5)
        {
            return true;
        } else if (c.GetTurroncitos() != 0 && type == 6)
        {
            return true;
        } else if (c.GetCraftsmen() != 0 && type == 7)
        {
            return true;
        } else
        {
            return false; //No se tiene esa unidad en la ciudad
        }
    }

    public bool moveUnits (int type, Hexagon hex)
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
            if (type == 1)
            {
                hex.GetCity().AddUnits(orcSettings.race, -1, -orcSettings.defense);
                GameObject train = Instantiate(orcPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 2)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(rollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 3)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(goblinPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 4)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(trollPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 5)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(cuctanyaPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 6)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(puppetPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else if (type == 7)
            {
                hex.GetCity().AddUnits(rollSettings.race, -1, -rollSettings.defense);
                GameObject train = Instantiate(witcherPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                return true; //Va bien
            }
            else
            {
                return false; //Por si acaso ponemos un valor raro
            }
        } else
        {
            return false; //Si no hay hueco para generar
        }
    }
}
