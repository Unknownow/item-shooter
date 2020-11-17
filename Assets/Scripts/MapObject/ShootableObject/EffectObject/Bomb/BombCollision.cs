using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollision : ShootableObjectCollision
{
    // ========== Fields and properties ==========
    // ========== MonoBehaviour methods ==========
    // ========== Public methods ==========
    public override void OnGetHitByBullet(GameObject bullet)
    {
        base.OnGetHitByBullet(bullet);
        LogUtils.instance.Log(GetClassName(), "OnGetHitByBullet", "NOT_YET_IMPLEMENT");
    }

    // ========== Protected methods ==========
    protected override void OnHitPlayer(GameObject player)
    {
        LogUtils.instance.Log(GetClassName(), "OnHitPlayer", "NOT_YET_IMPLEMENT");
    }
}
