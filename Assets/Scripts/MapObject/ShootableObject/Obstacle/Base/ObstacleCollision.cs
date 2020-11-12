using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : ShootableObjectCollision
{
    // ========== Public methods ==========
    public override void OnGetHitByBullet(GameObject bullet)
    {
        LogUtils.instance.Log(GetClassName(), "OnGetHitByBullet", "NOT YET IMPLEMENT!");
        return;
    }

    // // ========== Protected methods ==========
    protected override void OnHitPlayer(GameObject player)
    {
        LogUtils.instance.Log(GetClassName(), "OnGetHitByBullet", "NOT YET IMPLEMENT!");
        return;
    }
}
