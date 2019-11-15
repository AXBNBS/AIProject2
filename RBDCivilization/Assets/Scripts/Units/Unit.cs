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

    void Start()
    {
        attack = settings.attack;
        speed = settings.speed;
        defense = settings.defense;
        race = settings.race;
        occupation = settings.occupation;
        stores = settings.stores;
        movement = this.GetComponent<UnitMovement>();
    }

    void Update()
    {
        if (movement.reachedTrg == true && movement.currentHex.GetIsBuilded())
        {
            Assault();
        }
        //Para el combate habra que poder distinguir dentro de un hexagono entre enemigos y aliados
        else if(movement.reachedTrg==true && movement.currentHex.UnitsPlaced().Length > 2)
        {
            Fight();
        }
    }

    private void Assault()
    {
        //Habra que mirar de obtener todas las unidades de un mismo bando que esten en un hexagono para crear bien las probabilidades
        if (Random.Range(0, 10) > 9)
        {
            Destroy(movement.currentHex.GetCity().gameObject);
            movement.currentHex.SetIsBuilded(false);
        }
        else
        {
            movement.FindPathTo(movement.previousHex);
        }
    }

    private void Fight()
    {
        if (Random.Range(0, 10) > 9)
        {
            int enemyUnits = movement.currentHex.UnitsPlaced().Length;
            for (int i = 0; i < movement.currentHex.UnitsPlaced().Length; i++)
            {
                Destroy(movement.currentHex.UnitsPlaced()[i].gameObject);
            }

            movement.currentHex.presentUnt -= enemyUnits;
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
