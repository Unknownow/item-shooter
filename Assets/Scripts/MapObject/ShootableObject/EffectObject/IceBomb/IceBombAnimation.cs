using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBombAnimation : MonoBehaviour, IShootableObjectAnimation
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _angularSpeed;
    private float _currentAngularSpeed;
    private Animator _animator;

    private const string EXPLOSION_ANIMATION = "IceExplosion";
    private const string IDLE_ANIMATION = "Idle";

    // ========== MonoBehaviour methods ==========

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateRotation();
    }

    // ========== Public methods ==========
    public void DoEffectStartObject()
    {
        StartRotation();
        _animator.Play(IDLE_ANIMATION);
    }

    public void DoEffectDestroyObject()
    {
        StopRotation();

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(currentRotation);

        _animator.Play(EXPLOSION_ANIMATION);
        float time = _animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("OnExplosionDone", time);
    }

    public void ResetEffectObject()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        _animator.Play(IDLE_ANIMATION);
    }

    public void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
    {
        switch (objectType)
        {
            case EffectObjectType.BOMB:
                DoEffectDestroyObject();
                break;
            case EffectObjectType.ICE_BOMB:
                DoEffectDestroyObject();
                break;
            default:
                DoEffectDestroyObject();
                break;
        }
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

    private void OnExplosionDone()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
