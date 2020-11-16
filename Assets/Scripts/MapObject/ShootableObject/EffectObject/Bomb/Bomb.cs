using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IShootableObject, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private PointObjectConfig _config;
    public MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }

    // ========== Public methods ==========
    public void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartObject", "NOT_YET_IMPLEMENT");
    }

    public void StartObject(float percentIncrease)
    {
        ResetObject();
        LogUtils.instance.Log(GetClassName(), "StartObject(float percentIncrease)", "NOT_YET_IMPLEMENT");
    }

    public void DestroyObjectByBullet()
    {
        LogUtils.instance.Log(GetClassName(), "DestroyObjectByBullet", "NOT_YET_IMPLEMENT");
    }

    public void ResetObject()
    {
        LogUtils.instance.Log(GetClassName(), "ResetObject", "NOT_YET_IMPLEMENT");
    }

    public void DeactivateObject()
    {
        LogUtils.instance.Log(GetClassName(), "DeactivateObject", "NOT_YET_IMPLEMENT");
    }
}
