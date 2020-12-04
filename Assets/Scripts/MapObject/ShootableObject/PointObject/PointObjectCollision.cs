using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectCollision : ShootableObjectCollision
{
    // ========== Fields and properties ==========
    private PointObjectConfig _config;
    private PointObjectConfig config
    {
        get
        {
            if (_config == null)
                _config = (PointObjectConfig)gameObject.GetComponent<IMapObject>().config;
            return _config;
        }
    }

    // ========== MonoBehaviour methods ==========
    // ========== Public methods ==========
    public override void OnGetHitByBullet(GameObject bullet)
    {
        base.OnGetHitByBullet(bullet);
        if (config.POINT > 0)
            Manager.instance.AddPoint(config.POINT);
        else
            Manager.instance.SubPoint(Mathf.Abs(config.POINT));
        return;
    }

    // ========== Protected methods ==========
    protected override void OnHitPlayer(GameObject player)
    {
        player.GetComponent<Player>().OnGetHit(config.DAMAGE);
        gameObject.GetComponent<IShootableObject>().DestroyObjectByBullet();
        return;
    }
}
