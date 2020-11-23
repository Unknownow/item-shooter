﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBomb : EffectObject
{
    // ========== Fields and properties ==========
    [SerializeField]
    private IceBombConfig _config;
    public override MapObjectConfig config
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

    public float radius
    {
        get
        {
            return ((IceBombConfig)config).RADIUS;
        }
    }

    // ========== Public methods ==========
    public override void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartObject", "NOT_YET_IMPLEMENT");
    }

    public override void StartObject(float percentIncrease)
    {
        ResetObject();
        float defaultMovementSpeed = gameObject.GetComponent<IObjectMovement>().movementSpeed;
        gameObject.GetComponent<IObjectMovement>().movementSpeed = defaultMovementSpeed * (1 + percentIncrease / 100);
        float defaultAccelerationRate = gameObject.GetComponent<IObjectMovement>().accelerationRate;
        gameObject.GetComponent<IObjectMovement>().accelerationRate = defaultAccelerationRate * (1 + percentIncrease / 100);
        gameObject.GetComponent<IObjectMovement>().Move(Vector3.down, Vector3.down);

        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectStartObject();
    }

    public override void DestroyObjectByBullet()
    {
        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectDestroyObject();
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.GetComponent<Collider2D>().enabled = false;
        Invoke("OnDetonate", ((IceBombConfig)config).DELAY_BEFORE_AFFECT);
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

    public override void OnAffectedByEffectObject(EffectObjectType type, GameObject sourceObject)
    {
        switch (type)
        {
            case EffectObjectType.BOMB:
                DestroyObjectByBullet();
                break;
            default:
                DestroyObjectByBullet();
                break;
        }
    }

    // ========== Private methods ==========
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ((IceBombConfig)_config).RADIUS);
    }

    private void OnDetonate()
    {
        Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(transform.position, ((IceBombConfig)config).RADIUS, _affectedLayer);
        foreach (Collider2D collider in affectedColliders)
        {
            SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
            if (renderer == null)
                continue;
            if (!ObjectUtils.CheckIfSpriteInScreen(renderer))
                continue;

            IShootableObject affectedObject = collider.GetComponent<IShootableObject>();
            affectedObject.OnAffectedByEffectObject(type, gameObject);
        }
    }
}