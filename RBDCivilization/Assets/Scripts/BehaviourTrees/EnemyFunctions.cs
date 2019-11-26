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
}
