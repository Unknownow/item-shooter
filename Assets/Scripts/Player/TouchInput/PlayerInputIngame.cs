﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputIngame : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private float _playerTransformDelayCountdown;
    private int _currentMovingFingerId = -1;
    private int _currentShootingFingerId = -1;

    // ========== MonoBehaviour Methods ==========
    void Awake()
    {
        _currentMovingFingerId = -1;
    }

    void Update()
    {
        if (!GameFlowManager.instance.isPlaying)
            return;
        GetPlayerInput();
        GetPlayerInputWithMouse();
    }

    // ========== Private Methods ==========
    /// <summary>
    /// Get player input and dispatch it.
    /// </summary>
    private void GetPlayerInput()
    {
        if (Input.touchCount > 0)
        {
            if (_currentMovingFingerId == -1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.position.y < Screen.height / 5 && _currentMovingFingerId == -1)
                        _currentMovingFingerId = touch.fingerId;
                    else if (touch.position.y >= Screen.height / 5 && _currentShootingFingerId == -1)
                        _currentShootingFingerId = touch.fingerId;
                }
            }

            if (_currentMovingFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = GetTouchByFingerID(_currentMovingFingerId);
                    OnMovingTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }

            if (_currentShootingFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = GetTouchByFingerID(_currentShootingFingerId);
                    OnShootingTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }
        }
    }

    private void GetPlayerInputWithMouse()
    {
        if (InputWrapper.Input.touchCount > 0)
        {
            if (_currentMovingFingerId == -1)
            {
                for (int i = 0; i < InputWrapper.Input.touchCount; i++)
                {
                    Touch touch = InputWrapper.Input.GetTouch(i);
                    if (touch.position.y < Screen.height / 5 && _currentMovingFingerId == -1)
                        _currentMovingFingerId = touch.fingerId;
                    else if (touch.position.y >= Screen.height / 5 && _currentShootingFingerId == -1)
                        _currentShootingFingerId = touch.fingerId;
                }
            }

            if (_currentMovingFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = GetTouchByFingerIDWithMouse(_currentMovingFingerId);
                    OnMovingTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }

            if (_currentShootingFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = GetTouchByFingerIDWithMouse(_currentShootingFingerId);
                    OnShootingTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }
        }
    }

    private void OnMovingTouch(Touch touch)
    {
        EventSystem.instance.DispatchEvent(EventCode.ON_MOVING_TOUCH, new object[] { touch });
        if (touch.phase == TouchPhase.Ended)
            _currentMovingFingerId = -1;
    }

    private void OnShootingTouch(Touch touch)
    {
        EventSystem.instance.DispatchEvent(EventCode.ON_SHOOTING_TOUCH, new object[] { touch });
        if (touch.phase == TouchPhase.Ended)
            _currentShootingFingerId = -1;
    }

    private Touch GetTouchByFingerID(int fingerId)
    {
        if (Input.touchCount <= 0)
            throw new UnityException("NO TOUCHES EXIST");

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch currentTouch = Input.GetTouch(i);
            if (currentTouch.fingerId == fingerId)
                return currentTouch;
        }

        throw new UnityException("TOUCH WITH FINGER_ID" + fingerId.ToString() + "DOES NOT EXIST");
    }

    private Touch GetTouchByFingerIDWithMouse(int fingerId)
    {
        if (InputWrapper.Input.touchCount <= 0)
            throw new UnityException("NO TOUCHES EXIST");

        for (int i = 0; i < InputWrapper.Input.touchCount; i++)
        {
            Touch currentTouch = InputWrapper.Input.GetTouch(i);
            if (currentTouch.fingerId == fingerId)
                return currentTouch;
        }

        throw new UnityException("TOUCH WITH FINGER_ID" + fingerId.ToString() + "DOES NOT EXIST");
    }
}