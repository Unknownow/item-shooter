using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GUIEndGame : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private TextMeshProUGUI _textPoint;
    [SerializeField]
    private TextMeshProUGUI _textHighscore;
    private float _currentPoint;
    private Color[] _childColors;
    private Vector3[] _childScale;
    private List<EventListener> _listeners;
    // ========== MonoBehaviour methods ==========
    protected void Awake()
    {
        _childColors = new Color[transform.childCount];
        _childScale = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Image>() != null)
                _childColors[i] = child.GetComponent<Image>().color;
            else if (child.GetComponent<TextMeshProUGUI>() != null)
                _childColors[i] = child.GetComponent<TextMeshProUGUI>().color;
            _childScale[i] = child.localScale;
        }
        AddListeners();
        gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Public methods ==========
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

    public void OnBackToMenuClick()
    {
        float duration = 0.3f;
        DoEffectOut(duration);
        Sequence onMainMenuSequence = DOTween.Sequence();
        onMainMenuSequence.AppendInterval(duration + 0.1f);
        onMainMenuSequence.AppendCallback(() =>
        {
            GameFlowManager.instance.OnMainMenu();
        });
        onMainMenuSequence.Play();
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

            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            if (childImage != null)
            {
                Color currentColor = childImage.color;
                currentColor.a = 0;
                childImage.color = currentColor;
            }

            child.localScale = Vector3.zero;
        }

        _textPoint.SetText("0");
        _currentPoint = 0;
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


            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            if (childText != null)
            {
                Sequence fadeInSequence = DOTween.Sequence();
                fadeInSequence.AppendInterval(delayTime);
                fadeInSequence.Append(childText.DOFade(_childColors[i].a, duration));
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

            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            if (childText != null)
            {
                Sequence fadeInSequence = DOTween.Sequence();
                fadeInSequence.AppendInterval(delayTime);
                fadeInSequence.Append(childText.DOFade(0, duration));
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

    protected IEnumerator DoEffectPointIncrease(int targetPoint = 200, float delayTime = 0, float duration = 1.2f, float numberOfIncrease = 80)
    {
        yield return new WaitForSeconds(delayTime);

        float deltaTime = duration / numberOfIncrease;
        float deltaPoint = targetPoint / numberOfIncrease;
        while (numberOfIncrease > 0)
        {
            _currentPoint += deltaPoint;
            _textPoint.SetText(Mathf.RoundToInt(_currentPoint).ToString());
            yield return new WaitForSeconds(deltaTime);
            numberOfIncrease -= 1;
        }

        _textPoint.SetText(Mathf.RoundToInt(targetPoint).ToString());
    }

    // ========== Private methods ==========
    // ========== Event listener methods ==========
    protected void AddListeners()
    {
        _listeners = new List<EventListener>();
        _listeners.Add(EventSystem.instance.AddListener(EventCode.ON_PLAYER_DIED, this, OnPlayerDied));
    }

    protected void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    protected void OnPlayerDied(object[] eventParam)
    {
        GameFlowManager.instance.OnPause();
        DoEffectIn();
        int playerPoint = Manager.instance.totalPoint;
        StartCoroutine(DoEffectPointIncrease(playerPoint, 1));
        _textHighscore.SetText(Manager.instance.highScore.ToString());
    }
}
