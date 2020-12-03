using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceBombConfig", menuName = "ScriptableObjects/ShootableObject/IceBombConfig", order = 1)]
public class IceBombConfig : EffectConfig
{
    public float RADIUS = 4;
    public float SLOW_PERCENTAGE = 50;
    public float SLOW_DURATION = 2;
    public float RADIUS_SPEED = 4;
}
