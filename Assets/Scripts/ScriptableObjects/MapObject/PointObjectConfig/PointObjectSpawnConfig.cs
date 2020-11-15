using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointObjectSpawnConfig", menuName = "ScriptableObjects/PointObjectConfig/PointObjectSpawnConfig", order = 1)]
public class PointObjectSpawnConfig : ScriptableObject
{
    public int[] SPAWN_RATE_ARRAY;

    public float DEFAULT_TIME_BETWEEN_SPAWN;
    public float MIN_TIME_BETWEEN_SPAWN;

    public int DEFAULT_POINT_NEEDED_TO_NEXT_LEVEL;
    public float PERCENTAGE_INCREASE_EACH_LEVEL;
    public int MAX_LEVEL;
}
