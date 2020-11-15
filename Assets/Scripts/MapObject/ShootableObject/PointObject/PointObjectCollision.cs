using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectCollision : ShootableObjectCollision
{
    // ========== Fields and properties ==========
    private PointObject _basePointObject;

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
    private void Awake()
    {
        _basePointObject = gameObject.GetComponent<PointObject>();
    }

    // ========== Public methods ==========
    public override void OnGetHitByBullet(GameObject bullet)
    {
        if (config.POINT > 0)
            Manager.instance.AddPoint(config.POINT);
        else
            Manager.instance.SubPoint(Mathf.Abs(config.POINT));
        gameObject.SetActive(false);
        return;
    }

    // // ========== Protected methods ==========
    protected override void OnHitPlayer(GameObject player)
    {
        player.GetComponent<Player>().OnGetHit(config.DAMAGE);
        return;
    }
}
