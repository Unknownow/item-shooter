using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _intervalBetweenHeartBeat;
    [SerializeField]
    private int _numberOfPulsePerHeartBeat;
    [SerializeField]
    private float _rateOfHeartBeatChange;
    [SerializeField]
    private Image _imgHealthBar;
    private Sequence _currentSequence;
    private EventListener[] _eventListener;
    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        DoHeartBeat();
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Event listener methods ==========
    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_PLAYER_HEALTH_UPDATE, this, OnPlayerHealthPointUpdate);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    private void OnPlayerHealthPointUpdate(object[] eventParam)
    {
        if (_imgHealthBar == null)
            return;
        int currentHealthPoint = (int)eventParam[0];
        int maxHealthPoint = (int)eventParam[1];

        float percentage = currentHealthPoint * 1f / maxHealthPoint;
        float currentPercentage = _imgHealthBar.fillAmount;
        _imgHealthBar.fillAmount = percentage;

        if (currentHealthPoint <= 0)
        {
            StopHeartBeat();
            return;
        }

        if (currentPercentage < percentage)
            _intervalBetweenHeartBeat *= _rateOfHeartBeatChange;
        else
            _intervalBetweenHeartBeat /= _rateOfHeartBeatChange;
        DoHeartBeat();
    }

    // ========== Public methods ==========
    // ========== Private methods ==========

    private void DoHeartBeat()
    {
        StopHeartBeat();

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), _intervalBetweenHeartBeat / 5f));
        scaleSequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), _intervalBetweenHeartBeat / 10f));
        scaleSequence.SetLoops(_numberOfPulsePerHeartBeat);

        Sequence heartBeatSequence = DOTween.Sequence();
        heartBeatSequence.Append(scaleSequence);
        heartBeatSequence.SetLoops(-1);
        heartBeatSequence.AppendInterval(_intervalBetweenHeartBeat);
        heartBeatSequence.SetId(DOTweenIdList.HEART_BEAT);
        heartBeatSequence.Play();
    }

    private void StopHeartBeat()
    {
        List<Tween> tweenList = DOTween.TweensById(DOTweenIdList.HEART_BEAT);
        if (tweenList != null && tweenList.Count > 0)
            foreach (Tween item in tweenList)
                item.Kill(false);
    }
}
