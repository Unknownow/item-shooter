﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GUIPauseGame : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    private List<Color> _childColors;
    private List<Vector3> _childScale;
    // ========== MonoBehaviour methods ==========
    protected void Awake()
    {
        _childColors = new List<Color>();
        _childScale = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Image>() != null)
                _childColors.Add(child.GetComponent<Image>().color);
            _childScale.Add(child.localScale);
        }
    }
    // ========== Public methods ==========
    public void OnPauseClick()
    {
        GameFlowManager.instance.OnPause();
        DoEffectIn();
    }

    public void OnResumeClick()
    {
        float duration = 0.3f;
        DoEffectOut(duration);
        Sequence onResumeSequence = DOTween.Sequence();
        onResumeSequence.AppendInterval(duration + 0.1f);
        onResumeSequence.AppendCallback(() =>
        {
            GameFlowManager.instance.OnResume();
        });
        onResumeSequence.Play();
    }

    public void OnRestartClick()
    {
        float duration = 0.3f;
        DoEffectOut(duration);
        Sequence onRestartSequence = DOTween.Sequence();
        onRestartSequence.AppendInterval(duration + 0.1f);
        onRestartSequence.AppendCallback(() =>
        {
            GameFlowManager.instance.OnStartGame();
        });
        onRestartSequence.Play();
    }

    // ========== Protected methods ==========
    protected void PrepareDoEffectIn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                Color currentColor = childImage.color;
                currentColor.a = 0;
                childImage.color = currentColor;
            }

            child.localScale = Vector3.zero;
        }
    }

    protected void DoEffectIn(float duration = 0.3f)
    {
        PrepareDoEffectIn();
        float delayTime = 0;
        float deltaDelayTime = 0.05f;
        for (int i = 0; i < transform.childCount; i++)
        {
            delayTime += deltaDelayTime * i;
            Transform child = transform.GetChild(i);

            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                Sequence fadeInSequence = DOTween.Sequence();
                fadeInSequence.AppendInterval(delayTime);
                fadeInSequence.Append(child.GetComponent<Image>().DOFade(_childColors[i].a, duration));
                fadeInSequence.Play();
            }

            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.AppendInterval(delayTime);
            scaleSequence.Append(child.DOScale(_childScale[i] * 1.1f, duration * 2f / 3f));
            scaleSequence.Append(child.DOScale(_childScale[i], duration / 3f));
            scaleSequence.Play();
        }
    }

    protected void DoEffectOut(float duration = 0.6f)
    {
        float delayTime = 0;
        float deltaDelayTime = 0.05f;
        for (int i = 0; i < transform.childCount; i++)
        {
            delayTime += deltaDelayTime * i;
            Transform child = transform.GetChild(i);

            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                Sequence fadeInSequence = DOTween.Sequence();
                fadeInSequence.AppendInterval(delayTime);
                fadeInSequence.Append(child.GetComponent<Image>().DOFade(0, duration));
                fadeInSequence.Play();
            }

            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.AppendInterval(delayTime);
            scaleSequence.Append(child.DOScale(_childScale[i] * 1.1f, duration / 3f));
            scaleSequence.Append(child.DOScale(Vector3.zero, duration * 2f / 3f));
            scaleSequence.Play();
        }

        float totalTime = deltaDelayTime * (transform.childCount - 1) + duration;
        Sequence doCallbackSequence = DOTween.Sequence();
        doCallbackSequence.AppendInterval(duration);
        doCallbackSequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }

    // ========== Private methods ==========
}
