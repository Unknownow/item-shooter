using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObjectConfig : ScriptableObject
{
    public float MOVEMENT_SPEED = 2;
    public float ACCELERATION_RATE = 0;
    public float GRAVITATIONAL_ACCELERATION_RATE = 9;
}
