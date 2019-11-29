using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int level;
    private int neededMinerals;
    private int neededWood;
    private float defense;
    private string cityType;
    private string side;

    public int humans;
    private int cats;
    private int elves;
    private int dwarfs;
    private int twiis;
    public int craftsmen;
    public int turroncitos;

    private int orcs;
    private int panecillos;
    private int goblins;
    private int trolls;
    private int cuctañas;
    private int puppets;
    private int witchers;

    public CitySettings settings;
    public GameObject nextLevel;
    public GameObject previousLevel;

    private int capacity = 35;

    // Start is called before the first frame update
    void Awake()
    {
        defense = settings.defense;
        level = settings.level;
        neededMinerals = settings.neededMinerals;
        neededWood = settings.neededWood;
        cityType = settings.cityType;

        humans = 0;
        cats = 0;
        elves = 0;
        dwarfs = 0;
        twiis = 0;
        craftsmen = 0;
        turroncitos = 0;
        orcs = 0;
        panecillos = 0;
        goblins = 0;
        trolls = 0;
        cuctañas = 0;
        puppets = 0;
        witchers = 0;
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

    public float GetDefense()
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

    public int GetCapacity()
    {
        return capacity - humans - cats - elves - dwarfs - twiis - craftsmen - turroncitos - orcs - panecillos - goblins - trolls - cuctañas - puppets - witchers;
    }

    public void AddUnits(string race, int amount, float defenseIncrement)
    {
        switch (race)
        {
            case "Human":
                humans += amount;
                defense += defenseIncrement;
                break;
            case "Cat":
                cats += amount;
                defense += defenseIncrement;
                break;
            case "Elf":
                elves += amount;
                defense += defenseIncrement;
                break;
            case "Dwarf":
                dwarfs += amount;
                defense += defenseIncrement;
                break;
            case "Twii":
                twiis += amount;
                defense += defenseIncrement;
                break;
            case "Craftsman":
                craftsmen += amount;
                defense += defenseIncrement;
                break;
            case "Turroncito":
                turroncitos += amount;
                defense += defenseIncrement;
                break;
            case "Orc":
                orcs += amount;
                defense += defenseIncrement;
                break;
            case "Panecillo":
                panecillos += amount;
                defense += defenseIncrement;
                break;
            case "Goblin":
                goblins += amount;
                defense += defenseIncrement;
                break;
            case "Troll":
                trolls += amount;
                defense += defenseIncrement;
                break;
            case "Cuctaña":
                cuctañas += amount;
                defense += defenseIncrement;
                break;
            case "Puppet":
                puppets += amount;
                defense += defenseIncrement;
                break;
            case "Witcher":
                witchers += amount;
                defense += defenseIncrement;
                break;
            default:
                break;
        }
    }

    public int GetHumans()
    {
        return humans;
    }

    public int GetCats()
    {
        return cats;
    }

    public int GetElves()
    {
        return elves;
    }

    public int GetDwarfs()
    {
        return dwarfs;
    }

    public int GetTwiis()
    {
        return twiis;
    }

    public int GetCraftsmen()
    {
        return craftsmen;
    }

    public int GetTurroncitos()
    {
        return turroncitos;
    }

    public int GetOrcs()
    {
        return orcs;
    }

    public int GetPanecillos()
    {
        return panecillos;
    }

    public int GetGoblins()
    {
        return goblins;
    }

    public int GetTrolls()
    {
        return trolls;
    }

    public int GetCuctañas()
    {
        return cuctañas;
    }

    public int GetPuppets()
    {
        return puppets;
    }

    public int GetWitchers()
    {
        return witchers;
    }

    public void SetSettings()
    {
        defense = settings.defense;
        level = settings.level;
        neededMinerals = settings.neededMinerals;
        neededWood = settings.neededWood;
    }
}
