using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    BULLET_NORMAL,
}

public class Bullet : MonoBehaviour, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    protected BulletType _type;
    public BulletType type
    {
        get
        {
            return this._type;
        }
    }

    [SerializeField]
    private MapObjectConfig _config;
    public MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }
}
