using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointObjectType
{
    ADD_POINT_1,
    ADD_POINT_2,
    ADD_POINT_3,
    SUB_POINT_1,
    SUB_POINT_2,
    SUB_POINT_3,
}

public class PointObject : MonoBehaviour, IShootableObject, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private PointObjectType _type;
    public PointObjectType type
    {
        get
        {
            return this._type;
        }
    }

    [SerializeField]
    private PointObjectConfig _config;
    public MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }

    [SerializeField]
    private float _timeBeforeDeactivateObject;

    // ========== Public methods ==========
    public void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartObject", "NOT_YET_IMPLEMENT");
    }

    public void StartObject(float percentIncrease)
    {
        ResetObject();
        float defaultMovementSpeed = gameObject.GetComponent<IObjectMovement>().movementSpeed;
        gameObject.GetComponent<IObjectMovement>().movementSpeed = defaultMovementSpeed * (1 + percentIncrease / 100);
        float defaultAccelerationRate = gameObject.GetComponent<IObjectMovement>().accelerationRate;
        gameObject.GetComponent<IObjectMovement>().accelerationRate = defaultAccelerationRate * (1 + percentIncrease / 100);
        gameObject.GetComponent<IObjectMovement>().Move(Vector3.down, Vector3.down);

        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectStartObject();
    }

    public void DestroyObjectByBullet()
    {
        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectDestroyObject();
        gameObject.GetComponent<Collider2D>().enabled = false;
        Invoke("DeactivateObject", _timeBeforeDeactivateObject);
    }

    public void DestroyObjectByEffectObject(EffectObjectType type)
    {
        switch (type)
        {
            case EffectObjectType.BOMB:
                OnDestroyedByBomb();
                break;
        }

    }

    public void ResetObject()
    {
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.GetComponent<IObjectMovement>().moveDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().accelerationDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().ResetSpeedToDefault();

        gameObject.GetComponent<IShootableObjectAnimation>().ResetEffectObject();
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void DeactivateObject()
    {
        gameObject.SetActive(false);
        ResetObject();
    }

    // ========== Private methods ==========
    private void OnDestroyedByBomb()
    {
        PointObjectConfig currentConfig = (PointObjectConfig)config;
        if (currentConfig.POINT > 0)
            Manager.instance.AddPoint(currentConfig.POINT);
        DeactivateObject();
    }
}
