using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private int _blinkCountWhenGetHit;

    private Animator _animator;
    private SpriteRenderer _renderer;

    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Run";
    private const string ATTACK_ANIMATION = "Attack";

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        PlayIdleAnimation();
    }
    // ========== Public methods ==========
    public void PlayIdleAnimation()
    {
        _animator.Play(IDLE_ANIMATION);
    }

    public void PlayAttackAnimation()
    {
        _animator.Play(ATTACK_ANIMATION);
    }

    public void PlayRunLeftAnimation()
    {
        _animator.Play(RUN_ANIMATION);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, 180, currentRotation.z);
    }

    public void PlayRunRightAnimation()
    {
        _animator.Play(RUN_ANIMATION);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, 0, currentRotation.z);
    }

    public void PlayInvincibleAnimation(float duration)
    {
        float intervalBetweenBlink = duration / (2f * _blinkCountWhenGetHit);

        Sequence blinkSequence = DOTween.Sequence();
        blinkSequence.Append(_renderer.DOColor(new Color(1, 1, 1, 0), 0));
        blinkSequence.AppendInterval(intervalBetweenBlink);
        blinkSequence.Append(_renderer.DOColor(new Color(1, 1, 1, 1), 0));
        blinkSequence.AppendInterval(intervalBetweenBlink);
        blinkSequence.SetLoops(_blinkCountWhenGetHit);
        blinkSequence.Play();
    }
    // ========== Private methods ==========
}
