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

    private UnitMovement movement;

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
    }

    void Update()
    {
        if (movement.reachedTrg == true && movement.currentHex.GetIsBuilded())
        {
            Assault();
        }
        else if(movement.reachedTrg==false )
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
        }
        else
        {
            movement.FindPathTo(movement.previousHex);
        }
    }

    private void Fight(Hexagon hex)
    {
        float alliesPower = 0;
        float enemiesPower = 0;
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

        print(alliesPower + "   "+ enemiesPower);
        if (alliesPower > enemiesPower)
        {
            for(int i =0; i<hex.presentUnt; i++)
            {
                Destroy(hex.UnitsPlaced()[i].gameObject);
            }
            hex.presentUnt = 0;
            this.movement.FindPathTo(hex);
        }
        else
        {
            for (int i = 0; i < movement.currentHex.presentUnt; i++)
            {
                Hexagon aux=movement.currentHex;
                Destroy(movement.currentHex.UnitsPlaced()[i].gameObject);
                aux.presentUnt = 0;
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
