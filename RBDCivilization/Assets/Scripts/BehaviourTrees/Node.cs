using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node : MonoBehaviour
{
    //Devuelve el estado del nodo
    public delegate NodeStates NodeReturn();

    //El estado actual del nodo
    protected NodeStates m_nodeState;

    public NodeStates nodeState
    {
        get
        {
            return m_nodeState;
        }
    }

    //Constructor
    public Node() { }

    //Para implementar classes usa este método para evaluar el set deseado de condiciones
    public abstract NodeStates Evaluate();
}
