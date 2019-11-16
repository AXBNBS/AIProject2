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
        else if(movement.reachedTrg==true && this.tag!="Enemy")
        {
            bool enemyFound=false;
            UnitMovement[] aux = movement.currentHex.UnitsPlaced();
            int x = 0;
            for (int i = 0; i < aux.Length; i++)
            {
                if (aux[i].gameObject.tag == "Enemy")
                {
                    enemyFound = true;
                    x++;
                }
            }
            if (enemyFound == true)
            {
                Unit[] enemies = new Unit[x];
                int j = 0;
                for (int i = 0; i < aux.Length; i++)
                {
                    if (aux[i].gameObject.tag == "Enemy")
                    {
                        enemies[j] = aux[i].gameObject.GetComponent<Unit>();
                        j++;
                    }
                }
                Fight(enemies);
            }
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

    private void Fight(Unit[] enemies)
    {
        if (Random.Range(0, 10) > 0)
        {
            for(int i =0; i<enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
            }
        }
        else
        {
            for (int i = 0; i < movement.currentHex.UnitsPlaced().Length; i++)
            {
                Destroy(movement.currentHex.UnitsPlaced()[i].gameObject);
            }
            movement.FindPathTo(movement.previousHex);
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
