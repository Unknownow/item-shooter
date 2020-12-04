using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeartBarController : MonoBehaviour
{
    private void Awake()
    {
        DoHeartBeat();
    }
    
    private void DoHeartBeat()
    {
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.2f));
        scaleSequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f));
        scaleSequence.SetLoops(2);

        Sequence heartBeatSequence = DOTween.Sequence();
        heartBeatSequence.Append(scaleSequence);
        heartBeatSequence.SetLoops(-1);
        heartBeatSequence.AppendInterval(2);
        heartBeatSequence.Play();
    }
}
