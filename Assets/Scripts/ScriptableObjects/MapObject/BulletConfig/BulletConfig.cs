using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "ScriptableObjects/BulletConfig/BulletConfig", order = 0)]
public class BulletConfig : MapObjectConfig
{
    [Tooltip("Amount of bullets shoot per second")]
    public float FIRE_RATE = 2;
}