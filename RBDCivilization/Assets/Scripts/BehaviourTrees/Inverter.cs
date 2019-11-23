using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    //Nodo hijo a evaluar
    private Node m_node;

    public Node node
    {
        get
        {
            return m_node;
        }
    }

    //El constructor requiere el nodo hijo que esta "inverter decorator wraps"
    public Inverter (Node node)
    {
        m_node = node;
    }

    //Reporta un exito si el hijo falla y un fallo si el hijo tiene exito. Running reportará un running
    public override NodeStates Evaluate()
    {
        switch (m_node.Evaluate())
        {
            case NodeStates.FAILURE:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
        }
        m_nodeState = NodeStates.SUCCESS;
        return m_nodeState;
    }
}
