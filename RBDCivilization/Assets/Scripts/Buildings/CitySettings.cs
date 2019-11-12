using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="City Setting")]
public class CitySettings : ScriptableObject
{
    public int level;
    public int neededMinerals;
    public int neededWood;
    public int defense;
}
