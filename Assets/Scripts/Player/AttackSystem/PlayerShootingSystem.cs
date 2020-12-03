using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingSystem : MonoBehaviour, IPlayerAttackSystem
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private BulletType _currentBulletType;
    public BulletType currentBulletType
    {
        get
        {
            return _currentBulletType;
        }
        set
        {
            _currentBulletType = value;
            GameObject currentBullet = BulletPool.instance.GetBullet(_currentBulletType);
            _currentBulletConfig = (BulletConfig)currentBullet.GetComponent<Bullet>().config;
            currentBullet.SetActive(false);
        }
    }
    private BulletConfig _currentBulletConfig;

    private float bulletShootInterval
    {
        get
        {
            if (_currentBulletConfig == null)
                return 0;
            return 1 / (_currentBulletConfig.FIRE_RATE * _bulletShootIntervalMultiplier);
        }
    }

    private float _bulletShootIntervalMultiplier;
    private float _bulletShootIntervalMultiplierDuration;

    private Transform _gun;
    private bool _isShootable;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        // Get gun in child
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(TagList.PLAYER_GUN))
            {
                _gun = transform.GetChild(i);
                break;
            }
        }

        currentBulletType = BulletType.BULLET_NORMAL;
        _bulletShootIntervalMultiplier = 1;
        _bulletShootIntervalMultiplierDuration = 0;
        _isShootable = true;
    }

    // ========== Public methods ==========
    public void Attack()
    {
        LogUtils.instance.Log(GetClassName(), "Attack()", "NOT YET OVERRIDE");
    }

    public void Attack(Vector3 direction)
    {
        if (!_isShootable)
            return;

        LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", direction.ToString());
        float directionAngle = Vector3.Angle(Vector3.up, direction);
        if (directionAngle > 90)
        {
            LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", "DIRECTION_ANGLE = ", directionAngle.ToString(), "NOT_VALID");
            if (Vector3.Angle(Vector3.right, direction) < 90)
                direction = Vector3.right;
            else
                direction = -Vector3.right;
        }

        GameObject bullet = BulletPool.instance.GetBullet(_currentBulletType);
        bullet.GetComponent<IObjectMovement>().ResetSpeedToDefault();
        bullet.GetComponent<IObjectMovement>().Move(direction);
        if (_bulletShootIntervalMultiplier > 1)
            bullet.GetComponent<IObjectMovement>().SpeedUp(100 * (_bulletShootIntervalMultiplier - 1), _bulletShootIntervalMultiplierDuration);
        else if (_bulletShootIntervalMultiplier > 0 && _bulletShootIntervalMultiplier < 1)
            bullet.GetComponent<IObjectMovement>().SlowDown(100 * (1 - _bulletShootIntervalMultiplier), _bulletShootIntervalMultiplierDuration);
        _gun.GetComponent<PlayerGunController>().ChangeWeaponDirection(direction);
        bullet.transform.position = _gun.position;

        _isShootable = false;
        Invoke("OnShootable", bulletShootInterval);

        gameObject.GetComponent<PlayerAnimation>().PlayAttackAnimation();
    }

    public void Attack(Vector2 position)
    {
        if (!_isShootable)
            return;

        LogUtils.instance.Log(GetClassName(), "Attack(Vector2 position)", position.ToString());
        Vector3 direction = (Vector3)position - _gun.position;

        float directionAngle = Vector3.Angle(Vector3.up, direction);
        if (directionAngle > 90)
        {
            LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", "DIRECTION_ANGLE = ", directionAngle.ToString(), "NOT_VALID");
            if (Vector3.Angle(Vector3.right, direction) < 90)
                direction = Vector3.right;
            else
                direction = -Vector3.right;
        }

        GameObject bullet = BulletPool.instance.GetBullet(_currentBulletType);
        bullet.GetComponent<IObjectMovement>().ResetSpeedToDefault();
        bullet.GetComponent<IObjectMovement>().Move(direction);
        if (_bulletShootIntervalMultiplier > 1)
            bullet.GetComponent<IObjectMovement>().SpeedUp(100 * (_bulletShootIntervalMultiplier - 1), _bulletShootIntervalMultiplierDuration);
        else if (_bulletShootIntervalMultiplier > 0 && _bulletShootIntervalMultiplier < 1)
            bullet.GetComponent<IObjectMovement>().SlowDown(100 * (1 - _bulletShootIntervalMultiplier), _bulletShootIntervalMultiplierDuration);
        _gun.GetComponent<PlayerGunController>().ChangeWeaponDirection(direction);
        bullet.transform.position = _gun.position;

        _isShootable = false;
        Invoke("OnShootable", bulletShootInterval);

        gameObject.GetComponent<PlayerAnimation>().PlayAttackAnimation();
    }

    public void Attack(GameObject target)
    {
        LogUtils.instance.Log(GetClassName(), "Attack(GameObject target)", "NOT YET OVERRIDE");
    }

    public void OnShootable()
    {
        _isShootable = true;
    }

    public void ChangeBulletShootIntervalMultiplier(float multiplier, float duration)
    {
        CancelInvoke("ResetBulletShootIntervalMultiplier");
        _bulletShootIntervalMultiplier = multiplier;
        _bulletShootIntervalMultiplierDuration = duration;
        gameObject.GetComponent<Animator>().speed = multiplier;
        Invoke("ResetBulletShootIntervalMultiplier", duration);
    }

    public void ResetBulletShootIntervalMultiplier()
    {
        gameObject.GetComponent<Animator>().speed = 1;
        _bulletShootIntervalMultiplier = 1;
    }
}
