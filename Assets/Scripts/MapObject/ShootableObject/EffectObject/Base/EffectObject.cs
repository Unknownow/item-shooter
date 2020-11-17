using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectObjectType
{
    BOMB,
    FLAME,
    FREEZE
}

public abstract class EffectObject : MonoBehaviour, IShootableObject, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    public MapObjectConfig config { get; }

    [SerializeField]
    protected EffectObjectType _type;
    public EffectObjectType type
    {
        get
        {
            return _type;
        }
    }

    // ========== Public methods ==========
    public abstract void StartObject();

    public abstract void StartObject(float percentIncrease);

    public abstract void DestroyObjectByBullet();

    public abstract void ResetObject();

    public abstract void DeactivateObject();
}
