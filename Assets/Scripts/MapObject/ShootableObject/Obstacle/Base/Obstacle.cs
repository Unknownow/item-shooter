using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    ADD_POINT_1,
    // ADD_POINT_2,
    SUB_POINT_1,
    // SUB_POINT_2,
    // POWER_UP_1,
    // POWER_UP_2,
}

public class Obstacle : MonoBehaviour, IShootableObject, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private ObstacleType _type;
    public ObstacleType type
    {
        get
        {
            return this._type;
        }
    }

    [SerializeField]
    private ObstacleConfig _config;
    public MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }

    private void Awake()
    {
    }

    public void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartMoving", "NOT_YET_IMPLEMENT");
        gameObject.GetComponent<IObjectMovement>().movementSpeed = Random.Range(2, 4);
        gameObject.GetComponent<IObjectMovement>().accelerationRate = Random.Range(10, 12);
        gameObject.GetComponent<IObjectMovement>().Move(Vector3.down, Vector3.down);
    }

    public void DestroyObject()
    {
        LogUtils.instance.Log(GetClassName(), "DestroyObject", "NOT_YET_IMPLEMENT");
    }

    public void ResetObject()
    {
        LogUtils.instance.Log(GetClassName(), "ResetObject", "NOT_YET_IMPLEMENT");
    }
}
