using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float attack;
    private float speed;
    private float defense;
    private string race;
    private string occupation;
    private int stores;

    public UnitSettings settings;
    public GameManager gameManager;

    private UnitMovement movement;
    private GameObject GameManager;

    public float totalPower;

    void Start()
    {
        attack = settings.attack;
        speed = settings.speed;
        defense = settings.defense;
        race = settings.race;
        occupation = settings.occupation;
        stores = settings.stores;
        movement = this.GetComponent<UnitMovement>();
        totalPower = 0;

        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        if (movement.reachedTrg == true && movement.currentHex.GetIsBuilded())
        {
            Assault();
        }
        if (occupation == "Worker")
        {

        }
    }

    private void Assault()
    {
        //Habra que mirar de obtener todas las unidades de un mismo bando que esten en un hexagono para crear bien las probabilidades

        if (Random.Range(0, 10) > 0)
        {
            if (movement.currentHex.GetCity().GetLevel() == 1)
            {
                Destroy(movement.currentHex.GetCity().gameObject);
                movement.currentHex.SetIsBuilded(false);
            }
            else
            {
                GameObject previousLevel = movement.currentHex.GetCity().previousLevel;

                Object.Destroy(movement.currentHex.GetCity().gameObject);

                GameObject build = Instantiate(previousLevel, new Vector3(movement.currentHex.CentroHexagono.position.x, movement.currentHex.CentroHexagono.position.y, movement.currentHex.CentroHexagono.position.z), Quaternion.identity);

                movement.currentHex.SetCity(build.GetComponent<City>());
                movement.FindPathTo(movement.previousHex);
            }

            if (movement.currentHex.GetCity().GetCityType() == "Capital")
            {
                GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 5, false);
            } else
            {
                GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 3, false);
            }
        }
        else
        {
            movement.FindPathTo(movement.previousHex);
        }
    }

    private void Fight(Hexagon hex)
    {
        if (hex.UnitsPlaced() == null)
        {
            return;
        }
        float alliesPower = 0;
        float enemiesPower = 0;
        bool addCurrentHexagon = true;
        for(int i =0; i < hex.neighbours.Length; i++)
        {
            if (hex.neighbours[i] != null)
            {
                UnitMovement[] units = hex.neighbours[i].UnitsPlaced();
                if (units!= null && units.Length != 0)
                {
                    for(int j=0; j < units.Length; j++)
                    {
                        if (units[j].tag == "Enemy")
                        {
                            enemiesPower += units[j].stats.attack;
                        }
                        else if(units[j].tag=="Ally")
                        {
                            alliesPower += units[j].stats.attack;
                        }
                    }
                }
            }
            if (hex.neighbours[i] == movement.currentHex)
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
        UnitMovement[] allies = movement.GetAllies();
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
        if (alliesPower > enemiesPower)
        {
            for (int i = 0; i < hex.presentUnt; i++)
            {
                if(localUnits[i].tag=="Enemy")
                  Destroy(localUnits[i].gameObject);
            }
            hex.presentUnt = 0;
            movement.FindPathTo(hex);
        }
        else
        {
            movement.currentHex.presentUnt = 0;
            movement.currentHex.units = new UnitMovement[5];
            for (int i = 0; i < allies.Length; i++)
            {
                if(allies[i].tag=="Ally")
                  Destroy(allies[i].gameObject);
            }
        }
    }

    public float GetAttack()
    {
        return attack;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetDefense()
    {
        return defense;
    }

    public string GetRace()
    {
        return race;
    }

    public string GetOccupation()
    {
        return occupation;
    }

    public int GetStores()
    {
        return stores;
    }
}
