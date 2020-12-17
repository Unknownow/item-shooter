using System.Collections;
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
    [SerializeField]
    private int _totalPoint;
    public int totalPoint
    {
        get
        {
            return this._totalPoint;
        }
    }
    public int highScore
    {
        get { return PlayerPrefs.GetInt(PlayerPreferenceKey.HIGH_SCORE); }
        set { PlayerPrefs.SetInt(PlayerPreferenceKey.HIGH_SCORE, value); }
    }

    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _player;
    public GameObject player
    {
        get
        {
            if (_player == null)
                _player = Instantiate(_playerPrefab, new Vector3(0, -100, 0), Quaternion.identity);
            return _player;
        }
    }

    private float _pointMultiplier;
    private List<EventListener> _listeners;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        AddListeners();
        ResetPoint();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Public methods ==========
    public void ResetPlayer()
    {
        if (_player != null)
        {
            GameObject.Destroy(_player);
            _player = null;
        }
        _player = player;
    }

    public void DestroyPlayer()
    {
        if (_player != null)
        {
            GameObject.Destroy(_player);
            _player = null;
        }
    }

    public void AddPoint(int amount)
    {
        if (amount <= 0)
        {
            LogUtils.instance.Log(GetClassName(), "AddPoint", "AMOUNT = ", amount, "NOT_VALID");
            return;
        }
        _totalPoint += amount;
        CustomEventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
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
        CustomEventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
    }

    public void ResetPoint()
    {
        _totalPoint = 0;
        CustomEventSystem.instance.DispatchEvent(EventCode.ON_POINT_UPDATE, new object[] { totalPoint });
    }

    public void StartMultiplyPoint(float multiplier, float duration)
    {
        CancelInvoke("StopMultiplyPoint");
        _pointMultiplier = multiplier;
        Invoke("StopMultiplyPoint", duration);
    }

    public void StopMultiplyPoint()
    {
        _pointMultiplier = 1;
    }

    // ========== Private methods ==========
    // ========== Event listener methods ==========

    protected void AddListeners()
    {
        _listeners = new List<EventListener>();
        _listeners.Add(CustomEventSystem.instance.AddListener(EventCode.ON_PLAYER_DIED, this, OnPlayerDied));
        _listeners.Add(CustomEventSystem.instance.AddListener(EventCode.ON_MAIN_MENU, this, OnMainMenu, false));
    }

    protected void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
            CustomEventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    protected void OnPlayerDied(object[] eventParam)
    {
        if (highScore < totalPoint)
            highScore = totalPoint;
    }

    protected void OnMainMenu(object[] eventParam)
    {
        DestroyPlayer();
    }
}
