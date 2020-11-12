using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    NORMAL_BULLET,
}

public class Bullet : MonoBehaviour
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
}
