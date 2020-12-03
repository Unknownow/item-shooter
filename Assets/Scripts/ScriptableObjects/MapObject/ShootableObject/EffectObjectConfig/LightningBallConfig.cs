using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningBallConfig", menuName = "ScriptableObjects/ShootableObject/LightningBallConfig", order = 2)]
public class LightningBallConfig : EffectConfig
{
    public int TARGET_COUNT = 4;
    public float RADIUS = 3;
    public float DELAY_BETWEEN_CHAIN = 0.05f;
}
