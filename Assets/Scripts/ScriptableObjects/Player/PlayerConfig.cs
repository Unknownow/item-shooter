using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/Player/PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    public int MAX_HEALTH_POINT = 4;
    public float INTERVAL_BETWEEN_GET_HIT = 2;
    public float Y_POSITION_TO_SCREEN_RATIO = 0.1f;
    public float Y_POSITION_INPUT_TO_SCREEN_RATIO = 0.5f;
}
