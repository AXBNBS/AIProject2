using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int level;
    private int neededMinerals;
    private int neededWood;
    private int defense;
    private string cityType;
    private string side;

    public CitySettings settings;
    public GameObject nextLevel;
    public GameObject previousLevel;

    // Start is called before the first frame update
    void Start()
    {
        defense = settings.defense;
        level = settings.level;
        neededMinerals = settings.neededMinerals;
        neededWood = settings.neededWood;
        cityType = settings.cityType;
    }
    
    public void SetCityType(string n)
    {
        cityType = n;
    }

    public string GetCityType()
    {
        return cityType;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetNeededMinerals()
    {
        return neededMinerals;
    }
    
    public int GetNeededWood()
    {
        return neededWood;
    }

    public void SetCitySide(string s)
    {
        side = s;
    }

    public string GetCitySide()
    {
        return side;
    }
}
