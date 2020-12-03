using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombConfig", menuName = "ScriptableObjects/ShootableObject/BombConfig", order = 0)]
public class BombConfig : EffectConfig
{
    public float RADIUS = 4;
    public float DELAY_BEFORE_AFFECT = 0.2f;
}

