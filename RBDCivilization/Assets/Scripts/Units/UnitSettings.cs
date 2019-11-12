using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Unit Setting")]
public class UnitSettings : ScriptableObject
{
    public float attack;
    public float speed;
    public float defense;
    public string race;
    public string occupation;
    public int stores;
}
