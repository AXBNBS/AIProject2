using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeScript : MonoBehaviour
{
    public Grid grid;
    public EnemyFunctions enemyFunction;

    private int capitalLevel = 1;
    private int orcs = 0;
    private int rolls = 0;
    private int goblins = 0;
    private int trolls = 0;
    private int cuctanyas = 0;
    private int puppets = 0;
    private int witchers = 0;
    
    public ActionNode CheckWood;
    public ActionNode CheckMineral;
    public ActionNode CheckStores;
    public ActionNode CheckPopulation;
    public ActionNode LevelUp;
    public ActionNode ReclutUnits;
    public ActionNode CheckMoveUnits;
    public ActionNode MoveUnits;
    public ActionNode BuildSettlement;
    public ActionNode BuildFarm;
    public ActionNode BuildTunnel;
    public ActionNode Collect;
    public ActionNode CheckActiveFarms;
    public ActionNode Attack;
    public ActionNode CheckVictory;
    public ActionNode UnderAttack;
    public ActionNode MovementUnits;
    public ActionNode SaveUnits;
    public ActionNode AttackBuilding;
    public ActionNode CheckTotalUnits;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);

    // Start is called before the first frame update
    void Start()
    {
        CheckWood = new ActionNode(checkWood);
        CheckMineral = new ActionNode(checkMineral);
        CheckStores = new ActionNode(checkStores);
        CheckPopulation = new ActionNode(checkPopulation);
        LevelUp = new ActionNode(levelUp);
        ReclutUnits = new ActionNode(reclutUnits);
        CheckMoveUnits = new ActionNode(checkMoveUnits);
        MoveUnits = new ActionNode(moveUnits);
        BuildSettlement = new ActionNode(buildSettlement);
        BuildFarm = new ActionNode(buildFarm);
        BuildTunnel = new ActionNode(buildTunnel);
        Collect = new ActionNode(collect);
        CheckActiveFarms = new ActionNode(checkActiveFarms);
        Attack = new ActionNode(attack);
        UnderAttack = new ActionNode(underAttack);
        MovementUnits = new ActionNode(movementUnits);
        SaveUnits = new ActionNode(saveUnits);
        AttackBuilding = new ActionNode(attackBuilding);
        CheckTotalUnits = new ActionNode(checkTotalUnits);
    }

    public void Evaluate()
    {
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        Debug.Log("The IA is doing things");
        Hexagon hexCapital = grid.hexagons[14, 48];
        yield return null;

        if (underAttack() == NodeStates.SUCCESS)
        {
            Debug.Log("IA bajo ataque");
            if (checkTotalUnits() == NodeStates.SUCCESS)
            {
                Debug.Log("IA tiene ejercito");
                GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < units.Length; i++)
                {
                    if (units[i] != null && movementUnits(units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                    {
                        Debug.Log("IA almacena unidades");
                        saveUnits(hexCapital);                        
                    }
                }
            } else
            {
                Debug.Log("IA no tiene ejército");
                if (checkPopulation() == NodeStates.SUCCESS && checkStores(5) == NodeStates.SUCCESS)
                {
                    Debug.Log("IA tiene poblacion libre y suficientes viveres");
                    reclutUnits(1, hexCapital);
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (units[i] != null && movementUnits(units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                        {
                            Debug.Log("IA almacena unidades");
                            saveUnits(hexCapital);
                        }
                    }
                } else
                {
                    ("IA no puede producir tropas");                    
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (units[i] != null && movementUnits(units[i].GetComponent<Unit>().movement.currentHex, hexCapital) == NodeStates.SUCCESS)
                        {
                            Debug.Log("IA almacena unidades");
                            saveUnits(hexCapital);
                        }
                    }
                }
            }
        } else
        {
            Debug.Log("IA segura");
            if (checkTotalUnits() == NodeStates.SUCCESS)
            {
                Debug.Log("IA tiene ejercito");
            }
        }
    }

    private NodeStates checkWood(int n = 0)
    {
        if (enemyFunction.checkWood())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMineral(int n = 0)
    {
        if (enemyFunction.checkMineral())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkStores(int n = 0)
    {
        if (enemyFunction.checkStores())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkPopulation()
    {
        if (enemyFunction.checkPopulation())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates levelUp(Hexagon hex)
    {
        if (enemyFunction.levelUp(hex) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates reclutUnits(int type, Hexagon hex)
    {
        if (enemyFunction.reclutUnits(type, hex) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMoveUnits(int type, Hexagon hex)
    {
        if (enemyFunction.checkMoveUnits(type, hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates moveUnits(int type, Hexagon hex)
    {
        if (enemyFunction.moveUnits(type, hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildSettlement(Unit unit)
    {
        if (enemyFunction.buildSettlement(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildFarm(Unit unit)
    {
        if (enemyFunction.buildFarm(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildTunnel(Unit unit)
    {
        if (enemyFunction.buildTunnel(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates collect(Unit unit)
    {
        if (enemyFunction.Collect(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkActiveFarms()
    {
        if (enemyFunction.checkActiveFarms())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates attack(Unit unit)
    {
        if (enemyFunction.attack(unit))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates underAttack()
    {
        if (enemyFunction.underAttack())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates movementUnits(Hexagon startHex, Hexagon finalHex)
    {
        if (enemyFunction.movementUnits(startHex, finalHex) == 1)
            return NodeStates.SUCCESS;
        else if (enemyFunction.movementUnits(startHex, finalHex) == -1)
            return NodeStates.FAILURE;
        else
            return NodeStates.RUNNING;
    }

    private NodeStates saveUnits(Hexagon hex)
    {
        if (enemyFunction.saveUnits(hex))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates attackBuilding()
    {
        if (enemyFunction.attackBuilding())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkTotalUnits()
    {
        if (enemyFunction.checkTotalUnits())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }
}
