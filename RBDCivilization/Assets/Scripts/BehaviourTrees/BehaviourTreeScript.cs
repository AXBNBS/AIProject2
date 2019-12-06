using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeScript : MonoBehaviour
{
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
    }

    public void Evaluate()
    {
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        Debug.Log("The IA is doing things");
        yield return null;
    }

    private NodeStates checkWood()
    {
        if (enemyFunction.checkWood())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMineral()
    {
        if (enemyFunction.checkMineral())
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkStores()
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

    private NodeStates levelUp()
    {
        if (enemyFunction.levelUp(null) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates reclutUnits()
    {
        if (enemyFunction.reclutUnits(0,null) == 1)
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates checkMoveUnits()
    {
        if (enemyFunction.checkMoveUnits(0,null))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates moveUnits()
    {
        if (enemyFunction.moveUnits(0,null))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildSettlement()
    {
        if (enemyFunction.buildSettlement(null))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildFarm()
    {
        if (enemyFunction.buildFarm(null))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates buildTunnel()
    {
        if (enemyFunction.buildTunnel(null))
            return NodeStates.SUCCESS;
        else
            return NodeStates.FAILURE;
    }

    private NodeStates collect()
    {
        if (enemyFunction.Collect(null))
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

    private NodeStates attack()
    {
        if (enemyFunction.attack(null))
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
}
