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
    private int _maxHealthPoint;
    public int _currentHealthPoint;
    public int currentHealthPoint
    {
        get
        {
            return _currentHealthPoint;
        }
    }

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _maxHealthPoint = _config.MAX_HEALTH_POINT;
        _currentHealthPoint = _maxHealthPoint;
    }

    // ========== Public methods ==========
    public void OnGetHit(int damageAmount)
    {
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
}
