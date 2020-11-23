using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceBombConfig", menuName = "ScriptableObjects/ShootableObject/IceBombConfig", order = 1)]
public class IceBombConfig : MapObjectConfig
{
    public float RADIUS = 4;
    public float DELAY_BEFORE_AFFECT = 0.2f;
    public float SLOW_PERCENTAGE = 50;
}
