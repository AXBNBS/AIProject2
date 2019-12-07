using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    //Metodo para la accion
    public delegate NodeStates ActionNodeDelegate(int n = 0, Hexagon hex = null, Hexagon hex2 = null, Unit unit = null);

    //El delegador que es llamado para evaluar este nodo
    private ActionNodeDelegate m_action;

    //Ya que este nodo no contiene una lógica por si mismo, la logica debe ser introducida
    //en la forma de un delegador. Como el signature de estados, la accion necesita devolver
    //un NodeStates enum
    public ActionNode (ActionNodeDelegate action)
    {
        m_action = action;
    }

    //Evalua el nodo usando el pasado en el delegador y reporta el estado resultanto como sea apropiado
    public override NodeStates Evaluate()
    {
        switch (m_action())
        {
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.FAILURE:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
            default:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
        }
    }
}
