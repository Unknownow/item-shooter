using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EffectObject
{
    // ========== Fields and properties ==========
    [SerializeField]
    protected BombConfig _config;
    public new MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }

    [SerializeField]
    private LayerMask _affectedLayer;

    [SerializeField]
    private float _timeBeforeDeactivateObject;
    // ========== Public methods ==========
    public override void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartObject", "NOT_YET_IMPLEMENT");
    }

    public override void StartObject(float percentIncrease)
    {
        ResetObject();
        LogUtils.instance.Log(GetClassName(), "StartObject(float percentIncrease)", "NOT_YET_IMPLEMENT");
    }

    public override void DestroyObjectByBullet()
    {
        LogUtils.instance.Log(GetClassName(), "DestroyObjectByBullet", "NOT_YET_IMPLEMENT");

        Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(transform.position, ((BombConfig)config).RADIUS, _affectedLayer);
        foreach (Collider2D collider in affectedColliders)
        {
            SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
            if (renderer == null)
                continue;
            if (!ObjectUtils.instance.CheckIfSpriteInScreen(renderer))
                continue;

            PointObject pointObject = collider.GetComponent<PointObject>();
            pointObject.DestroyObjectByEffectObject(type);
        }

        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectDestroyObject();
        gameObject.GetComponent<Collider2D>().enabled = false;

        Invoke("DeactivateObject", _timeBeforeDeactivateObject);
    }

    public override void ResetObject()
    {
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.GetComponent<IObjectMovement>().moveDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().accelerationDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().ResetSpeedToDefault();

        gameObject.GetComponent<IShootableObjectAnimation>().ResetEffectObject();
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public override void DeactivateObject()
    {
        gameObject.SetActive(false);
        ResetObject();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ((BombConfig)_config).RADIUS);
    }
}
