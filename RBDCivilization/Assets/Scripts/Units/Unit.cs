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
    private GameObject gameManager;

    public UnitMovement movement;

    public float totalPower;
    private FinishScript finishScript;

    void Awake()
    {
        attack = settings.attack;
        speed = settings.speed;
        defense = settings.defense;
        race = settings.race;
        occupation = settings.occupation;
        stores = settings.stores;
        //Debug.Log("Stores:" + stores);
        movement = this.GetComponent<UnitMovement>();
        totalPower = 0;

        gameManager = GameObject.FindGameObjectWithTag("GameController");
        finishScript = GameObject.FindGameObjectWithTag("Interface").GetComponentInChildren<FinishScript>();
    }

    void Update()
    {
        if (movement.reachedTrg == true && movement.currentHex.GetIsBuilded() && (((movement.currentHex.GetCity().GetCitySide()=="Blue" || movement.currentHex.GetCity().GetCitySide() == "blue") && this.tag=="Enemy") || ((movement.currentHex.GetCity().GetCitySide() == "Red" || movement.currentHex.GetCity().GetCitySide() == "red") && this.tag == "Ally")))
        {
            Assault();
        }
        else if(movement.reachedTrg == true && movement.currentHex.GetIsBuilded() && (((movement.currentHex.GetCity().GetCitySide() == "Blue" || movement.currentHex.GetCity().GetCitySide() == "blue") && this.tag == "Ally") || ((movement.currentHex.GetCity().GetCitySide() == "Red" || movement.currentHex.GetCity().GetCitySide() == "red") && this.tag == "Enemy")))
        {
            Stack();
        }
    }

    private void Stack()
    {
        if (movement.currentHex.GetCity().GetCapacity() < movement.GetAllies().Length)
        {
            movement.GetAllies()[0].FindPathTo(movement.previousHex);
            return;
        }
        float defense = 0;
        foreach(UnitMovement a in movement.GetAllies())
        {
            if (a != null)
                defense += a.stats.defense;
        }
        UnitMovement[] allies = movement.GetAllies();
        movement.currentHex.presentUnt = 0;
        movement.currentHex.units = new UnitMovement[5];
        if (movement.GetAllies().Length > 0)
            movement.currentHex.GetCity().AddUnits(movement.GetAllies()[0].stats.race, movement.GetAllies().Length, defense);
        for (int i = 0; i < allies.Length; i++)
        {
            if (allies[i] != null)
            {
                gameManager.GetComponent<GameManager>().playerUnt.Remove(allies[i].GetComponent<UnitMovement>());
                Destroy(allies[i].gameObject);
            }
        }
    }

    private void Assault()
    {
        //Habra que mirar de obtener todas las unidades de un mismo bando que esten en un hexagono para crear bien las probabilidades
        bool finish = false;

        float alliesPower = 0;
        float enemiesPower = 0;
        for (int i = 0; i < movement.currentHex.neighbours.Length; i++)
        {
            if (movement.currentHex.neighbours[i] != null)
            {
                UnitMovement[] units = movement.currentHex.neighbours[i].UnitsPlaced();
                if (units != null && units.Length != 0)
                {
                    for (int j = 0; j < units.Length; j++)
                    {
                        if (units[j].tag == "Enemy")
                        {
                            enemiesPower += units[j].stats.attack;
                        }
                        else if (units[j].tag == "Ally")
                        {
                            alliesPower += units[j].stats.attack;
                        }
                    }
                }
            }
        }
        UnitMovement[] localUnits = movement.currentHex.UnitsPlaced();
        if (localUnits != null && localUnits.Length != 0)
        {
            for (int j = 0; j < localUnits.Length; j++)
            {
                if (movement.currentHex.GetCity().GetCitySide()=="Red")
                {
                    enemiesPower += movement.currentHex.GetCity().GetDefense();
                }
                else if (movement.currentHex.GetCity().GetCitySide() == "Blue")
                {
                    alliesPower += movement.currentHex.GetCity().GetDefense();
                }
                else if (localUnits[j].tag == "Ally")
                {
                    alliesPower += localUnits[j].stats.attack;
                }
                else if (localUnits[j].tag == "Enemy")
                {
                    enemiesPower += localUnits[j].stats.attack;
                }
            }
        }
        UnitMovement[] allies = movement.GetAllies();

        if (this.tag=="Ally"? Random.Range(0.0f, 9.0f) >= 5.0f + enemiesPower-alliesPower : Random.Range(0.0f, 9.0f) >= 5.0f - enemiesPower + alliesPower)
        {
            if (movement.currentHex.GetCity().GetLevel() == 1)
            {
                if (movement.currentHex.GetCity().GetCityType() == "Farm")
                {
                    if (movement.currentHex.GetCity().tag == "RedFarm")
                    {
                        gameManager.GetComponent<GameManager>().AIFrm.Remove(movement.currentHex.GetCity().GetComponent<Farm>());
                    }
                    else
                    {
                        gameManager.GetComponent<GameManager>().playerFrm.Remove(movement.currentHex.GetCity().GetComponent<Farm>());
                    }
                }
                if (movement.currentHex.GetCity().GetCityType() == "Capital")
                    finish = true;
                Destroy(movement.currentHex.GetCity().gameObject);
                movement.currentHex.SetIsBuilded(false);                
            }
            else
            {
                GameObject previousLevel = movement.currentHex.GetCity().previousLevel;

                Object.Destroy(movement.currentHex.GetCity().gameObject);

                movement.currentHex.environment = Instantiate(previousLevel, new Vector3(movement.currentHex.CentroHexagono.position.x, movement.currentHex.CentroHexagono.position.y, movement.currentHex.CentroHexagono.position.z), Quaternion.identity);

                movement.currentHex.SetCity(movement.currentHex.environment.GetComponent<City>());
                movement.FindPathTo(movement.previousHex);
            }

            if (movement.currentHex.GetCity().gameObject.tag == "BlueCapital")
            {
                if (movement.currentHex.GetCity().GetCityType() == "Capital")
                {
                    gameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 5, false);
                    if (finish)
                        finishScript.finishMatch("Blue");
                }
                else
                {
                    gameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 3, false);
                }
            }
            else
            {
                if (movement.currentHex.GetCity().GetCityType() == "Capital")
                {
                    gameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Red", 5, false);
                    if (finish)
                    {
                        finishScript.finishMatch("Red");
                        
                    }
                }
                else
                {
                    gameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Red", 3, false);
                }
            }
        }
        else
        {
            movement.currentHex.presentUnt = 0;
            movement.currentHex.units = new UnitMovement[5];
            for (int i = 0; i < allies.Length; i++)
            {
                if (allies[i].tag == "Enemy")
                {
                    if (allies[i].stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().AIBld.Remove(allies[i].GetComponent<Builder>());
                    }
                    else if (allies[i].stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().AICll.Remove(allies[i].GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().AIUnt.Remove(allies[i].GetComponent<UnitMovement>());
                }

                else if (allies[i].tag == "Ally")
                {
                    if (allies[i].stats.occupation == "Worker")
                    {
                        gameManager.GetComponent<GameManager>().playerBld.Remove(allies[i].GetComponent<Builder>());
                    }
                    else if (allies[i].stats.occupation == "Collector")
                    {
                        gameManager.GetComponent<GameManager>().playerCll.Remove(allies[i].GetComponent<Collector>());
                    }
                    gameManager.GetComponent<GameManager>().playerUnt.Remove(allies[i].GetComponent<UnitMovement>());
                }

                Destroy(allies[i].gameObject);
            }
            
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
        for (int i = 0; i < hex.neighbours.Length; i++)
        {
            if (hex.neighbours[i] != null)
            {
                UnitMovement[] units = hex.neighbours[i].UnitsPlaced();
                if (units != null && units.Length != 0)
                {
                    for (int j = 0; j < units.Length; j++)
                    {
                        if (units[j].tag == "Enemy")
                        {
                            enemiesPower += units[j].stats.attack;
                        }
                        else if (units[j].tag == "Ally")
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
        if (this.tag == "Ally")
        {
            if (Random.Range(0, 10)>=5-alliesPower+enemiesPower)
            {
                for (int i = 0; i < hex.presentUnt; i++)
                {
                    if (localUnits[i].tag == "Enemy")
                    {
                        if (localUnits[i].stats.occupation == "Worker")
                        {
                            gameManager.GetComponent<GameManager>().AIBld.Remove(localUnits[i].GetComponent<Builder>());
                        }
                        else if (localUnits[i].stats.occupation == "Collector")
                        {
                            gameManager.GetComponent<GameManager>().AICll.Remove(localUnits[i].GetComponent<Collector>());
                        }
                        gameManager.GetComponent<GameManager>().AIUnt.Remove(localUnits[i].GetComponent<UnitMovement>());
                        Destroy(localUnits[i].gameObject);
                    }
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
                    if (allies[i].tag == "Ally")
                    {
                        if (allies[i].stats.occupation == "Worker")
                        {
                            gameManager.GetComponent<GameManager>().playerBld.Remove(allies[i].GetComponent<Builder>());
                        }
                        else if (allies[i].stats.occupation == "Collector")
                        {
                            gameManager.GetComponent<GameManager>().playerCll.Remove(allies[i].GetComponent<Collector>());
                        }
                        gameManager.GetComponent<GameManager>().playerUnt.Remove(allies[i].GetComponent<UnitMovement>());
                        Destroy(allies[i].gameObject);
                    }
                }
            }
        }
        else if (this.tag == "Enemy")
        {
            if (Random.Range(0, 10) >= 5 + alliesPower - enemiesPower)
            {
                for (int i = 0; i < hex.presentUnt; i++)
                {
                    if (localUnits[i].tag == "Ally")
                    {
                        if (localUnits[i].stats.occupation == "Worker")
                        {
                            gameManager.GetComponent<GameManager>().playerBld.Remove(localUnits[i].GetComponent<Builder>());
                        }
                        else if (localUnits[i].stats.occupation == "Collector")
                        {
                            gameManager.GetComponent<GameManager>().playerCll.Remove(localUnits[i].GetComponent<Collector>());
                        }
                        gameManager.GetComponent<GameManager>().playerUnt.Remove(localUnits[i].GetComponent<UnitMovement>());
                        Destroy(localUnits[i].gameObject);
                    }
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
                    if (allies[i].tag == "Enemy")
                    {
                        if (allies[i].stats.occupation == "Worker")
                        {
                            gameManager.GetComponent<GameManager>().AIBld.Remove(allies[i].GetComponent<Builder>());
                        }
                        else if (allies[i].stats.occupation == "Collector")
                        {
                            gameManager.GetComponent<GameManager>().AICll.Remove(allies[i].GetComponent<Collector>());
                        }
                        gameManager.GetComponent<GameManager>().AIUnt.Remove(allies[i].GetComponent<UnitMovement>());
                        Destroy(allies[i].gameObject);
                    }
                }
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
        //Debug.Log(stores);
        return stores;
    }
}
