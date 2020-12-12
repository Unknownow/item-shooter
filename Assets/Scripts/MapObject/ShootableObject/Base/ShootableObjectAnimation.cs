using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObjectAnimation : MonoBehaviour, IShootableObjectAnimation
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    protected Color _slowColor;
    protected Color _defaultColor;
    protected float[] _colorChangeSpeed;
    [SerializeField]
    protected float _angularSpeed;
    protected float _currentAngularSpeed;
    protected float _angularSpeedMultiplier;
    protected float _slowDuration;

    // ========== MonoBehaviour methods ==========
    protected virtual void Awake()
    {
        _colorChangeSpeed = new float[4];
        _defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected virtual void Update()
    {
        UpdateRotation();
        UpdateSlowDuration();
    }

    // ========== Public methods ==========
    public virtual void DoEffectStartObject()
    {
        StartRotation();
    }

    public virtual void DoEffectDestroyObject()
    {
        StopRotation();
    }

    public virtual void ResetEffectObject()
    {
        ResetSlowEffect();
    }

    public virtual void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
    {
        switch (objectType)
        {
            case EffectObjectType.BOMB:
                DoEffectDestroyObject();
                break;
            case EffectObjectType.ICE_BOMB:
                DoEffectSlowedByIceBomb(sourceObject);
                break;
            default:
                DoEffectDestroyObject();
                break;
        }
    }

    public void SlowDownAngularSpeed(float percentage = 0, float duration = -100)
    {
        _angularSpeedMultiplier = 1 - percentage / 100f;
        _slowDuration = duration;
    }

    public void SpeedUpAngularSpeed(float percentage = 0, float duration = -100)
    {
        _angularSpeedMultiplier = 1 + percentage / 100f;
        _slowDuration = duration;
    }

    // ========== Private methods ==========
    protected void StartRotation()
    {
        _currentAngularSpeed = Random.Range(0.8f, 1.2f) * _angularSpeed;
        _currentAngularSpeed = (Random.Range(-1, 1) < 0) ? -_currentAngularSpeed : _currentAngularSpeed;
    }

    protected void StopRotation()
    {
        _currentAngularSpeed = 0;
    }

    protected void UpdateRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + Time.deltaTime * _angularSpeedMultiplier * _currentAngularSpeed);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    protected void DoEffectSlowedByIceBomb(GameObject sourceObject)
    {
        float slowPercentage = sourceObject.GetComponent<IceBomb>().slowPercentage;
        float slowDuration = sourceObject.GetComponent<IceBomb>().slowDuration;
        SlowDownAngularSpeed(slowPercentage, slowDuration);
        gameObject.GetComponent<SpriteRenderer>().color = _slowColor;

        _colorChangeSpeed[0] = (_defaultColor.r - _slowColor.r) / slowDuration;
        _colorChangeSpeed[1] = (_defaultColor.g - _slowColor.g) / slowDuration;
        _colorChangeSpeed[2] = (_defaultColor.b - _slowColor.b) / slowDuration;
        _colorChangeSpeed[3] = (_defaultColor.a - _slowColor.a) / slowDuration;
    }

    protected void UpdateSlowDuration()
    {
        if (_slowDuration <= 0)
        {
            if (_slowDuration != -100f)
                ResetSlowEffect();
            _slowDuration = -1;
            return;
        }
        _slowDuration -= Time.deltaTime;

        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        currentColor.r += _colorChangeSpeed[0] * Time.deltaTime;
        currentColor.g += _colorChangeSpeed[1] * Time.deltaTime;
        currentColor.b += _colorChangeSpeed[2] * Time.deltaTime;
        currentColor.a += _colorChangeSpeed[3] * Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    protected void ResetSlowEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        SpeedUpAngularSpeed();
    }
}
