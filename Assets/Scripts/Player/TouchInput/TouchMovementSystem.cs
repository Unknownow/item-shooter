﻿using UnityEngine;

public class TouchMovementSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _playerYPositionRatioToScreen;
    
    private EventListener[] _eventListener = null;
    private Vector3 _lastTouchPosition = Vector3.zero;
    private Collider2D _playerCollider;

    // ========== MonoBehaviour Methods ==========
    void Start()
    {
        _lastTouchPosition = Vector3.zero;
        AddListeners();
        _playerCollider = gameObject.GetComponent<Collider2D>();
    }
    private void Update()
    {
        Vector3 position = transform.position;
        position.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * _playerYPositionRatioToScreen, 0)).y;
        transform.position = position;
    }

    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_MOVING_TOUCH, this, OnMovingTouch);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Private Methods ==========
    private bool CheckPositionInCameraBoundaries(Vector3 newPosition)
    {
        if (_playerCollider)
        {
            Bounds playerBound = _playerCollider.bounds;
            Vector3 newMin = newPosition - playerBound.extents;
            Vector3 newMax = newPosition + playerBound.extents;

            Camera mainCam = Camera.main;
            float cameraHeight = mainCam.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCam.aspect;
            Vector3 cameraPosition = mainCam.transform.position;

            if (
                newMin.x < cameraPosition.x - cameraWidth / 2 ||
                newMin.y < cameraPosition.y - cameraHeight / 2 ||
                newMax.x > cameraPosition.x + cameraWidth / 2 ||
                newMax.y > cameraPosition.y + cameraHeight / 2
            )
                return false;
            return true;
        }
        return false;
    }

    private void OnMovingTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchBegan(touch);
                break;
            case TouchPhase.Moved:
                OnTouchMoved(touch);
                break;
            case TouchPhase.Ended:
                OnTouchEnded(touch);
                break;
        }
    }

    private void OnTouchBegan(Touch touch)
    {
        Vector3 currentTouchPosition = touch.position;
        currentTouchPosition = Camera.main.ScreenToWorldPoint(currentTouchPosition);
        currentTouchPosition.z = 0;
        _lastTouchPosition = currentTouchPosition;
    }

    private void OnTouchMoved(Touch touch)
    {
        Vector3 delta = touch.deltaPosition;
        if (delta.sqrMagnitude > 2)
        {
            Vector3 currentTouchPosition = touch.position;
            currentTouchPosition = Camera.main.ScreenToWorldPoint(currentTouchPosition);
            currentTouchPosition.z = 0;

            Vector3 movingOffset = currentTouchPosition - _lastTouchPosition;
            Vector3 newPosition = transform.position + movingOffset;
            if (CheckPositionInCameraBoundaries(newPosition))
            {
                newPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * _playerYPositionRatioToScreen, 0)).y;
                transform.position = newPosition;
            }

            _lastTouchPosition = currentTouchPosition;
        }
    }

    private void OnTouchEnded(Touch touch)
    {
        _lastTouchPosition = Vector3.zero;
        return;
    }
}
