﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootableObjectAnimation
{
    void DoEffectStartObject();
    void DoEffectDestroyObject();
    void ResetEffectObject();
    void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject);
}
