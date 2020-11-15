using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectAnimation : MonoBehaviour, IShootableObjectAnimation
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _angularSpeed;
    private float _currentAngularSpeed;
    private SplitInHalfAnimation _splitInHalfAnimation;

    // ========== MonoBehaviour methods ==========

    private void Awake()
    {
        _splitInHalfAnimation = gameObject.GetComponent<SplitInHalfAnimation>();
    }

    private void Update()
    {
        UpdateRotation();
    }

    // ========== Public methods ==========
    public void DoEffectStartObject()
    {
        StartRotation();
    }

    public void DoEffectDestroyObject()
    {
        StopRotation();
        _splitInHalfAnimation.DoEffectSplitInHalf();
    }

    public void ResetEffectObject()
    {
        _splitInHalfAnimation.ResetEffect();
    }

    // ========== Private methods ==========
    private void StartRotation()
    {
        _currentAngularSpeed = Random.Range(0.8f, 1.2f) * _angularSpeed;
        _currentAngularSpeed = (Random.Range(-1, 1) < 0) ? -_currentAngularSpeed : _currentAngularSpeed;
    }

    private void StopRotation()
    {
        _currentAngularSpeed = 0;
    }

    private void UpdateRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + Time.deltaTime * _currentAngularSpeed);
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
