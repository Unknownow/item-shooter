using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private PlayerConfig _config;
    public PlayerConfig config { get { return _config; } }
    private int _maxHealthPoint;
    private int _currentHealthPoint;
    public int currentHealthPoint
    {
        get
        {
            return _currentHealthPoint;
        }
    }

    private bool _isHitable;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _maxHealthPoint = _config.MAX_HEALTH_POINT;
        _currentHealthPoint = _maxHealthPoint;
        _isHitable = true;
    }
    // ========== Public methods ==========
    public void OnGetHit(int damageAmount)
    {
        if (!_isHitable)
            return;

        if (damageAmount <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "OnGetHit", "DAMAGE_AMOUNT =", damageAmount, "<=0 NOT_VALID");
            return;
        }
        int newHealthPoint = _currentHealthPoint - damageAmount;
        _currentHealthPoint = (newHealthPoint < 0) ? 0 : newHealthPoint;
        EventSystem.instance.DispatchEvent(EventCode.ON_PLAYER_HEALTH_UPDATE, new object[] { _currentHealthPoint, _maxHealthPoint });
        if (_currentHealthPoint <= 0)
            EventSystem.instance.DispatchEvent(EventCode.ON_PLAYER_DIED);
        else
        {
            _isHitable = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<PlayerAnimation>().PlayInvincibleAnimation(_config.INTERVAL_BETWEEN_GET_HIT);
            Invoke("OnHitable", _config.INTERVAL_BETWEEN_GET_HIT);
        }
    }

    public void OnHeal(int healAmount)
    {
        if (healAmount <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "OnHeal", "HEAL_AMOUNT =", healAmount, "<=0 NOT_VALID");
            return;
        }
        if (_currentHealthPoint <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "OnHeal", "PLAYER_ALREADY_DEAD");
            return;
        }
        int newHealthPoint = _currentHealthPoint + healAmount;
        _currentHealthPoint = (newHealthPoint > _maxHealthPoint) ? _maxHealthPoint : newHealthPoint;
        EventSystem.instance.DispatchEvent(EventCode.ON_PLAYER_HEALTH_UPDATE, new object[] { _currentHealthPoint, _maxHealthPoint });
    }

    // ========== Private methods ==========
    private void OnHitable()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        _isHitable = true;
    }
}
