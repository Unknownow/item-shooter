using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private TextMeshProUGUI _textPoint;
    private EventListener[] _eventListener;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _textPoint = gameObject.GetComponent<TextMeshProUGUI>();
        SetCurrentPoint(0);
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Public methods ==========
    // ========== Private methods ==========
    private void SetCurrentPoint(int point)
    {
        _textPoint.SetText(point.ToString());
    }
    // ========== Event listener methods ==========
    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_POINT_UPDATE, this, OnPointUpdate);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnPointUpdate(object[] eventParam)
    {
        int currentPoint = (int)eventParam[0];
        SetCurrentPoint(currentPoint);
    }
}
