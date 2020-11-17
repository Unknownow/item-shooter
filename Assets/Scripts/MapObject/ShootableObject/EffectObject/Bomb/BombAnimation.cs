using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour, IShootableObjectAnimation
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _angularSpeed;
    private float _currentAngularSpeed;

    // ========== MonoBehaviour methods ==========

    private void Awake()
    {
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

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(currentRotation);

        gameObject.GetComponent<Animator>().SetTrigger("Explode");
    }

    public void ResetEffectObject()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Reset");
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
