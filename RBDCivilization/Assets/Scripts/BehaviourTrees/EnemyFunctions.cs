using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFunctions : MonoBehaviour
{
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

            int humans = hex.GetCity().GetHumans();
            int cats = hex.GetCity().GetCats();
            int elves = hex.GetCity().GetElves();
            int dwarfs = hex.GetCity().GetDwarfs();
            int twiis = hex.GetCity().GetTwiis();
            int craftsmen = hex.GetCity().GetCraftsmen();
            int turroncitos = hex.GetCity().GetTurroncitos();

            string type = hex.GetCity().GetCityType();

            GameObject nextLevel = hex.GetCity().nextLevel;

            Destroy(hex.environment);

            hex.environment = Instantiate(nextLevel, new Vector3(hex.CentroHexagono.position.x, hex.CentroHexagono.position.y, hex.CentroHexagono.position.z), Quaternion.identity);

            hex.SetCity(hex.environment.GetComponent<City>());
            hex.GetCity().SetCityType(type);
            hex.GetCity().SetCitySide("Red");

            hex.GetCity().AddUnits("Human", humans, humans);
            hex.GetCity().AddUnits("Cat", cats, cats * 1.5f);
            hex.GetCity().AddUnits("Elf", elves, elves);
            hex.GetCity().AddUnits("Dwarf", dwarfs, dwarfs);
            hex.GetCity().AddUnits("Twii", twiis, twiis * 2);
            hex.GetCity().AddUnits("Craftsman", craftsmen, craftsmen * 0.5f);
            hex.GetCity().AddUnits("Turroncito", turroncitos, turroncitos);

            if (hex.GetCity().GetCityType() == "Capital")
                capital = hex.GetCity();

            return 1; //Se puede aumentar de nivel
        } else
        {
            return 0; //Ya tiene el nivel maximo
        }
    }
}
