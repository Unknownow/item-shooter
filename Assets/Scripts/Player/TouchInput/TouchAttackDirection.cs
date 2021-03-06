﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAttackDirection : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private EventListener[] _eventListener = null;
    private Vector3 _touchBeganPosition = Vector3.zero;
    private float _disableTouchCountdown;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        _touchBeganPosition = Vector3.zero;
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Private Methods ==========

    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = CustomEventSystem.instance.AddListener(EventCode.ON_SHOOTING_TOUCH, this, OnShootingTouch);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
            CustomEventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnShootingTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchBegan(touch);
                break;
            case TouchPhase.Ended:
                OnTouchEnded(touch);
                break;
        }
    }

    private void OnTouchBegan(Touch touch)
    {
        Vector3 currentTouchPosition = touch.position;
        currentTouchPosition = CameraManager.mainCamera.ScreenToWorldPoint(currentTouchPosition);
        currentTouchPosition.z = 0;
        _touchBeganPosition = currentTouchPosition;
    }

    private void OnTouchEnded(Touch touch)
    {
        AttackToPosition(touch);
    }

    private void AttackToPosition(Touch touch)
    {
        Vector2 currentTouchPosition = touch.position;
        currentTouchPosition = CameraManager.mainCamera.ScreenToWorldPoint(currentTouchPosition);
        gameObject.GetComponent<IPlayerAttackSystem>().Attack(currentTouchPosition);
        return;
    }

    private void AttackWithDirection(Touch touch)
    {
        Vector3 currentTouchPosition = touch.position;
        currentTouchPosition = CameraManager.mainCamera.ScreenToWorldPoint(currentTouchPosition);
        currentTouchPosition.z = 0;

        Vector3 direction = currentTouchPosition - _touchBeganPosition;
        if (direction == Vector3.zero)
            return;
        direction = direction.normalized;

        gameObject.GetComponent<IPlayerAttackSystem>().Attack(direction);

        _touchBeganPosition = Vector3.zero;
        return;
    }
}
