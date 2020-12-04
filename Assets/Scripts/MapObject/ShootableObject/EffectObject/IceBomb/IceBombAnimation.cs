using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBombAnimation : ShootableObjectAnimation
{
    // ========== Fields and properties ==========

    private Animator _animator;
    private const string EXPLOSION_ANIMATION = "IceExplosion";
    private const string IDLE_ANIMATION = "Idle";

    // ========== MonoBehaviour methods ==========

    protected override void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // ========== Public methods ==========
    public override void DoEffectStartObject()
    {
        base.DoEffectStartObject();
        _animator.Play(IDLE_ANIMATION);
    }

    public override void DoEffectDestroyObject()
    {
        base.DoEffectDestroyObject();

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(currentRotation);

        _animator.Play(EXPLOSION_ANIMATION);
        float time = _animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("OnExplosionDone", time);
    }

    public override void ResetEffectObject()
    {
        _animator.Play(IDLE_ANIMATION);
        base.ResetEffectObject();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
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
    private void OnExplosionDone()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
