using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootableObject
{
    void StartObject(float percentIncrease);
    void StartObject();
    void DestroyObject();
    void ResetObject();
}
