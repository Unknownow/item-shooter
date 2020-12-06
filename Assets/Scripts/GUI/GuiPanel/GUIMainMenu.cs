using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GUIMainMenu : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private GameObject _titleObject;
    private Color[] _childColors;
    private Vector3[] _childScale;
    private Vector3[] _titleTextPosition;
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

        Transform titleTransform = _titleObject.transform;
        _titleTextPosition = new Vector3[titleTransform.childCount];
        for (int i = 0; i < titleTransform.childCount; i++)
            _titleTextPosition[i] = titleTransform.GetChild(i).position;

        AddListeners();
        PrepareDoEffectIn();
    }

    private void Start()
    {
        DoEffectIn();
    }

    protected void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Public methods ==========
    public void OnStartClick()
    {
        float duration = 0.3f;
        float titleDuration = 0.8f;
        DoEffectOut(titleDuration, duration);
        Sequence onRestartSequence = DOTween.Sequence();
        onRestartSequence.AppendInterval(titleDuration + 0.1f);
        onRestartSequence.AppendCallback(() =>
        {
            GameFlowManager.instance.OnStartGame();
        });
        onRestartSequence.Play();
    }

    // ========== Protected methods ==========
    protected void PrepareDoEffectIn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child == _titleObject.transform)
                continue;

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
        PrepareDoEffectTitleIn();
        gameObject.SetActive(true);
    }

    protected void DoEffectIn(float titleDuration = 0.8f, float duration = 0.3f)
    {
        PrepareDoEffectIn();

        float titleDeltaDelay = 0.08f;
        float titleDelayTime = 0;
        Transform titleTransform = _titleObject.transform;
        for (int i = 0; i < titleTransform.childCount; i++)
        {
            Transform child = titleTransform.GetChild(i);
            Sequence effectSequence = DOTween.Sequence();
            effectSequence.AppendInterval(titleDelayTime);
            effectSequence.Append(child.DOMoveX(_titleTextPosition[i].x + 50, titleDuration * 2f / 3f));
            effectSequence.Append(child.DOMoveX(_titleTextPosition[i].x, titleDuration / 3f));
            effectSequence.Play();
            titleDelayTime += titleDeltaDelay;
        }

        float delayTime = titleDuration - 0.18f;
        float deltaDelayTime = 0.05f;
        for (int i = 0; i < transform.childCount; i++)
        {
            delayTime += deltaDelayTime * i;
            Transform child = transform.GetChild(i);

            if (child == _titleObject.transform)
                continue;

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

    protected void DoEffectOut(float titleDuration = 0.8f, float duration = 0.3f)
    {
        float titleDeltaDelay = 0.08f;
        float titleDelayTime = 0;
        Transform titleTransform = _titleObject.transform;
        for (int i = 0; i < titleTransform.childCount; i++)
        {
            Transform child = titleTransform.GetChild(i);
            Sequence effectSequence = DOTween.Sequence();
            effectSequence.AppendInterval(titleDelayTime);
            effectSequence.Append(child.DOMoveX(_titleTextPosition[i].x + 50, titleDuration / 3f));
            effectSequence.Append(child.DOMoveX(-Screen.width, titleDuration * 2f / 3f));
            effectSequence.Play();
            titleDelayTime += titleDeltaDelay;
        }

        float delayTime = 0;
        float deltaDelayTime = 0.05f;
        for (int i = 0; i < transform.childCount; i++)
        {
            delayTime += deltaDelayTime * i;
            Transform child = transform.GetChild(i);

            if (child == _titleObject.transform)
                continue;

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

        Sequence doCallbackSequence = DOTween.Sequence();
        doCallbackSequence.AppendInterval((titleDuration < duration) ? duration : titleDuration);
        doCallbackSequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }

    protected void PrepareDoEffectTitleIn()
    {
        Transform titleTransform = _titleObject.transform;
        for (int i = 0; i < titleTransform.childCount; i++)
        {
            Transform child = titleTransform.GetChild(i);
            Vector3 currentPosition = child.position;
            currentPosition.x = -Screen.width;
            child.position = currentPosition;
        }
    }

    // ========== Event listener methods ==========
    protected void AddListeners()
    {
        _listeners = new List<EventListener>();
        _listeners.Add(EventSystem.instance.AddListener(EventCode.ON_MAIN_MENU, this, OnMainMenu));
    }

    protected void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    protected void OnMainMenu(object[] eventParam)
    {
        DoEffectIn();
    }
}
