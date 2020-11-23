using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootableObject
{
    void StartObject(float percentIncrease);
    void StartObject();
    void DestroyObjectByBullet();
    void ResetObject();
    void DeactivateObject();
    void OnAffectedByEffectObject(EffectObjectType type, GameObject sourceObject);
}
