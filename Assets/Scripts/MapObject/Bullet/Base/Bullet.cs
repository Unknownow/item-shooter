using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    BULLET_NORMAL,
}

public class Bullet : MonoBehaviour, IMapObject
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    protected BulletType _type;
    public BulletType type
    {
        get
        {
            return this._type;
        }
    }

    [SerializeField]
    private MapObjectConfig _config;
    public MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }
    [SerializeField]
    private float _timeBeforeDeactivateObject;
    private Transform _bulletSprite;

    // ========== MonoBehaviour methods ==========
    protected void Awake()
    {
        _bulletSprite = transform.GetChild(0);
    }

    // ========== Public methods ==========
    public void ResetObject()
    {
        gameObject.GetComponent<IObjectMovement>().gravityEnabled = false;
        gameObject.GetComponent<IObjectMovement>().ResetSpeedToDefault();
        _bulletSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void DeactivateObject()
    {
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.SetActive(false);
        ResetObject();
    }

    public void OnHitShootableObject(GameObject target)
    {
        _bulletSprite.GetComponent<SpriteRenderer>().color = new Color32(90, 90, 90, 90);
        gameObject.GetComponent<IObjectMovement>().gravityEnabled = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Invoke("DeactivateObject", _timeBeforeDeactivateObject);
    }
}
