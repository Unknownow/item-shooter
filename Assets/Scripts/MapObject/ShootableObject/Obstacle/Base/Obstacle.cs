using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    POINT_1,
    POINT_5,
    POINT_10,
    PENALTY_1,
    PENALTY_5,
    PENALTY_10,
}

public class Obstacle : MonoBehaviour, IShootableObject
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

    public void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartMoving", "NOT_YET_IMPLEMENT");
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
