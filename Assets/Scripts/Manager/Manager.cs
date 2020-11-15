﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Singleton instance ==========
    private static Manager _instance;
    public static Manager instance
    {
        get
        {
            return _instance;
        }
    }
    private Manager()
    {
        if (_instance == null)
            _instance = this;
    }

    // ========== Fields and properties ==========
    private int _totalPoint;
    public int totalPoint
    {
        get
        {
            return this._totalPoint;
        }
    }

    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _player;
    public GameObject player
    {
        get
        {
            if (_player == null)
                _player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            return _player;
        }
    }

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _player = player;
        ResetPoint();
    }

    // ========== Public methods ==========
    public void AddPoint(int amount)
    {
        if (amount <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "AddPoint", "AMOUNT = ", amount, "NOT_VALID");
            return;
        }
        _totalPoint += amount;
        EventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
    }

    public void SubPoint(int amount)
    {
        if (amount <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "SubPoint", "AMOUNT = ", amount, "NOT_VALID");
            return;
        }
        _totalPoint -= amount;
        if (_totalPoint < 0)
            _totalPoint = 0;
        EventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
    }

    public void ResetPoint()
    {
        _totalPoint = 0;
        EventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
    }

    // ========== Private methods ==========
}
