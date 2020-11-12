using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    INFINITE,
    HAS_END_POINT
}

public class StraightObjectMovement : MonoBehaviour, IObjectMovement
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private bool _rotateWithVelocity;
    [SerializeField]
    [Tooltip("Default direction for this object. Used to rotate with velocity")]
    private Vector3 _defaultDirection = Vector3.up;

    [SerializeField]
    [Tooltip("Movement config. Leave it empty to get default config of object")]
    private MapObjectConfig _movementConfig;
    public MapObjectConfig movementConfig
    {
        get
        {
            if (_movementConfig == null)
                _movementConfig = gameObject.GetComponent<IMapObject>().config;
            return _movementConfig;
        }
    }

    private float _movementSpeed;
    public float movementSpeed
    {
        get
        {
            return this._movementSpeed;
        }
        set
        {
            this._movementSpeed = value;
            _velocity = velocity.normalized * value;
        }
    }
    public float currentMovementSpeed
    {
        get
        {
            return velocity.sqrMagnitude;
        }
    }
    private float _accelerationRate;
    public float accelerationRate
    {
        get
        {
            return this._accelerationRate;
        }
        set
        {
            this._accelerationRate = value;
            _acceleration = acceleration.normalized * value;
        }
    }
    public Vector3 moveDirection
    {
        get
        {
            return velocity.normalized;
        }
        set
        {
            _velocity = value.normalized * movementSpeed;
        }
    }
    public Vector3 accelerationDirection
    {
        get
        {
            return acceleration.normalized;
        }
        set
        {
            _acceleration = value.normalized * accelerationRate;
        }
    }
    private Vector3 _velocity = Vector3.zero;
    public Vector3 velocity
    {
        get
        {
            return this._velocity;
        }
    }
    private Vector3 _acceleration;
    public Vector3 acceleration
    {
        get
        {
            return this._acceleration;
        }
    }

    private Vector3 _endPosition;
    private MoveType _moveType;


    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        ResetSpeedToDefault();
        _endPosition = transform.position;
    }

    private void Update()
    {
        UpdateBulletPosition();
        RotateWithVelocity();
    }

    // ========== Public Methods ==========
    public void ResetSpeedToDefault()
    {
        if (movementConfig != null)
        {
            movementSpeed = movementConfig.MOVEMENT_SPEED;
            accelerationRate = movementConfig.ACCELERATION_RATE;
        }
        else
        {
            movementSpeed = 0;
            accelerationRate = 0;
        }
    }

    public void MoveBy(Vector3 distance)
    {
        _endPosition = transform.position + distance;
        _moveType = MoveType.HAS_END_POINT;

        Vector3 direction = _endPosition - transform.position;
        direction = direction.normalized;
        moveDirection = direction;
    }

    public void MoveTo(Vector3 endPosition)
    {
        _endPosition = endPosition;
        _moveType = MoveType.HAS_END_POINT;

        Vector3 direction = _endPosition - transform.position;
        direction = direction.normalized;
        moveDirection = direction;
    }

    public void Move()
    {
        LogUtils.instance.Log(GetClassName(), "Move()", "NOT OVERRIDED!");
    }

    public void Move(Vector3 moveDirection)
    {
        _endPosition = Vector3.zero;
        _moveType = MoveType.INFINITE;

        this.moveDirection = moveDirection.normalized;
    }

    public void Move(Vector3 moveDirection, Vector3 accelerationDirection)
    {
        _endPosition = Vector3.zero;
        _moveType = MoveType.INFINITE;

        this.moveDirection = moveDirection.normalized;
        this.accelerationDirection = accelerationDirection.normalized;
    }

    public void StopMoving()
    {
        _endPosition = transform.position;
        _moveType = MoveType.HAS_END_POINT;
    }

    // ========== Private Methods ==========
    private void UpdateBulletPosition()
    {
        if (_moveType == MoveType.HAS_END_POINT)
        {
            if (_endPosition == transform.position)
                return;

            if (Vector3.Distance(_endPosition, transform.position) < movementSpeed * 3 * Time.deltaTime)
            {
                transform.position = _endPosition;
                return;
            }
        }

        transform.position += velocity * Time.deltaTime;
        _velocity += acceleration * Time.deltaTime;
    }

    private void RotateWithVelocity()
    {
        if (!_rotateWithVelocity)
            return;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, velocity.normalized);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _defaultDirection.normalized);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
