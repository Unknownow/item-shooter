using UnityEngine;

public class TouchMovementSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private float _playerYPositionRatioToScreen;
    private EventListener[] _eventListener = null;
    private Vector3 _lastTouchPosition = Vector3.zero;
    private Collider2D _playerCollider;
    private PlayerConfig _config;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        _config = gameObject.GetComponent<Player>().config;
        _playerYPositionRatioToScreen = _config.Y_POSITION_TO_SCREEN_RATIO;
        _lastTouchPosition = Vector3.zero;
        _playerCollider = gameObject.GetComponent<Collider2D>();
        AddListeners();
    }

    private void Update()
    {
        UpdatePlayerPosition();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Event listener Methods ==========
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
    // ========== Private Methods ==========
    private void UpdatePlayerPosition()
    {
        Vector3 position = transform.position;
        position.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * _playerYPositionRatioToScreen, 0)).y;
        transform.position = position;
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
            movingOffset.y = 0;
            Vector3 newPosition = transform.position + movingOffset;
            if (CheckPositionInCameraBoundaries(newPosition))
            {
                newPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * _playerYPositionRatioToScreen, 0)).y;
                transform.position = newPosition;
            }

            _lastTouchPosition = currentTouchPosition;

            if (movingOffset.x > 0)
                gameObject.GetComponent<PlayerAnimation>().PlayRunRightAnimation();
            else if (movingOffset.x < 0)
                gameObject.GetComponent<PlayerAnimation>().PlayRunLeftAnimation();
            else
                gameObject.GetComponent<PlayerAnimation>().PlayIdleAnimation();
        }
    }

    private void OnTouchEnded(Touch touch)
    {
        gameObject.GetComponent<PlayerAnimation>().PlayIdleAnimation();
        _lastTouchPosition = Vector3.zero;
        return;
    }

    private bool CheckPositionInCameraBoundaries(Vector3 newPosition)
    {
        if (_playerCollider)
        {
            Vector3 offset = newPosition - transform.position;
            Bounds playerBound = _playerCollider.bounds;
            Bounds newBound = new Bounds(playerBound.center + offset, playerBound.size);

            Vector3 newMin = newBound.min;
            Vector3 newMax = newBound.max;

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
}
