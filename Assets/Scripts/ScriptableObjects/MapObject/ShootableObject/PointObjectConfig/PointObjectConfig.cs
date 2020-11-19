using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointObjectConfig", menuName = "ScriptableObjects/ShootableObject/PointObjectConfig", order = 0)]
public class PointObjectConfig : MapObjectConfig
{
    public int POINT = 1;
    public int DAMAGE = 1;
}
