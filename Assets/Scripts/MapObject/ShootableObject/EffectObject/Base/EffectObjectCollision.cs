using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectCollision : ShootableObjectCollision
{
    // ========== Fields and properties ==========
    [SerializeField]
    protected EffectConfig _config;
    // ========== MonoBehaviour methods ==========
    protected void Awake()
    {
        if (_config == null)
            _config = (EffectConfig)gameObject.GetComponent<EffectObject>().config;
    }
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
        Manager.instance.StartMultiplyPoint(_config.POINT_MULTIPLIER, _config.POINT_MULTIPLIER_DURATION);
        gameObject.GetComponent<EffectObject>().DestroyObjectByBullet();
    }
}