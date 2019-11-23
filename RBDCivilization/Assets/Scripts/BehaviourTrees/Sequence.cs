using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    // Hijos que no pertenecen a esta secuencia
    private List<Node> m_nodes = new List<Node>();

    //Debe proporcionar un set inicial de nodos hijos al trabajo
    public Sequence(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //Si un hijo reporta un fallo, el nodo entero falla. Cuando todos los nodos devuelven un acierto, el nodo entero reporta un acierto
    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }
        }
        m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return m_nodeState;
    }
}
