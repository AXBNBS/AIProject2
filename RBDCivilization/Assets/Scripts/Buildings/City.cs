using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int level;
    private int neededMinerals;
    private int neededWood;
    private int defense;

    public CitySettings settings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        defense = settings.defense;
        level = settings.level;
        neededMinerals = settings.neededMinerals;
        neededWood = settings.neededWood;
    }
}
