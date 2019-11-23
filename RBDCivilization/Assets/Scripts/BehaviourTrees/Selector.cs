using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    //Los hijos de los nodos para este selector
    protected List<Node> m_nodes = new List<Node>();

    //El constructor requiere de una lista de nodos hijos para ser pasados
    public Selector (List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //Si cualquiera de los hijos reporta un exito, el selector inmediatamente reportará una recompensa. Si todos fallan, reportará un fallo
    public override NodeStates Evaluate()
    {
        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        return m_nodeState;
    }
}
