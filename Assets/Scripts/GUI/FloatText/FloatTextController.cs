using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum FloatTextType
{
    FLOAT_UP,
    BLINK,
}

public class FloatTextController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private GameObject _floatTextPrefab;

    [Header("Float up")]
    [SerializeField]
    private float _defaultFloatDuration;
    [SerializeField]
    private Vector3 _defaultFloatOffset;

    [Header("Blink")]
    [SerializeField]
    private float _defaultBlinkDuration;
    [SerializeField]
    private int _defaultBlinkCount;

    private List<GameObject> _pool;
    private static FloatTextController _instance;
    public static FloatTextController instance
    {
        get
        {
            return _instance;
        }
    }
    private FloatTextController()
    {
        if (_instance == null)
        {
            _pool = new List<GameObject>();
            _instance = this;
        }
    }
    // ========== MonoBehaviour methods ==========
    // ========== Public methods ==========
    public static void DoFloatUpText(string content, Vector3 position, Vector3 floatOffset, float duration, Color32 color, bool isWorldPosition = true)
    {
        instance.CreateFloatUpText(content, position, floatOffset, duration, color, isWorldPosition);
    }

    public static void DoFloatUpText(string content, Vector3 position, Color32 color, bool isWorldPosition = true)
    {
        float defaultDuration = instance._defaultFloatDuration;
        Vector3 defaultOffset = instance._defaultFloatOffset;
        DoFloatUpText(content, position, defaultOffset, defaultDuration, color, isWorldPosition);
    }

    public static void DoFloatUpText(string content, Vector3 position, bool isWorldPosition = true)
    {
        float defaultDuration = instance._defaultFloatDuration;
        Vector3 defaultOffset = instance._defaultFloatOffset;
        DoFloatUpText(content, position, defaultOffset, defaultDuration, Colors.TURQUOISE, isWorldPosition);
    }

    public static void DoFloatBlinkText(string content, Vector3 position, int blinkCount, float duration, Color32 color, bool isWorldPosition = true)
    {
        instance.CreateFloatBlinkText(content, position, blinkCount, duration, color, isWorldPosition);
    }

    public static void DoFloatBlinkText(string content, Vector3 position, Color32 color, bool isWorldPosition = true)
    {
        float defaultDuration = instance._defaultBlinkDuration;
        int defaultBlinkCount = instance._defaultBlinkCount;
        DoFloatBlinkText(content, position, defaultBlinkCount, defaultDuration, color, isWorldPosition);
    }

    public static void DoFloatBlinkText(string content, Vector3 position, bool isWorldPosition = true)
    {
        float defaultDuration = instance._defaultBlinkDuration;
        int defaultBlinkCount = instance._defaultBlinkCount;
        DoFloatBlinkText(content, position, defaultBlinkCount, defaultDuration, Colors.TURQUOISE, isWorldPosition);
    }

    // ========== Private methods ==========
    private void CreateFloatUpText(string content, Vector3 position, Vector3 floatOffset, float duration, Color32 color, bool isWorldPosition = true)
    {
        if (isWorldPosition)
            position = CameraManager.mainCamera.WorldToScreenPoint(position);
        GameObject textObject = instance.GetText();
        TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
        textComponent.SetText(content);
        textObject.transform.position = position;
        textComponent.color = color;
        DoFloatUpEffect(textObject, floatOffset, duration);
    }

    private void CreateFloatBlinkText(string content, Vector3 position, int blinkCount, float duration, Color32 color, bool isWorldPosition = true)
    {
        if (isWorldPosition)
            position = CameraManager.mainCamera.WorldToScreenPoint(position);
        GameObject textObject = instance.GetText();
        TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
        textComponent.SetText(content);
        textObject.transform.position = position;
        textComponent.color = color;
        DoFloatBlinkEffect(textObject, blinkCount, duration);
    }

    private void DoFloatUpEffect(GameObject target, Vector3 floatOffset, float duration)
    {
        Transform targetTransform = target.transform;
        Sequence floatUpSequence = DOTween.Sequence();
        floatUpSequence.Append(targetTransform.DOMove(targetTransform.position + floatOffset, duration));
        floatUpSequence.AppendCallback(() =>
        {
            target.SetActive(false);
            target.GetComponent<TextMeshProUGUI>().color = Colors.TURQUOISE;
        });

        Sequence fadeOutSequence = DOTween.Sequence();
        fadeOutSequence.AppendInterval(duration * 3 / 4f);
        fadeOutSequence.Append(target.GetComponent<TextMeshProUGUI>().DOFade(0, duration / 4f));

        floatUpSequence.Play();
        fadeOutSequence.Play();
    }

    private void DoFloatBlinkEffect(GameObject target, int blinkCount, float duration)
    {
        float intervalBetweenBlink = duration / (blinkCount * 2f);

        Transform targetTransform = target.transform;
        TextMeshProUGUI text = target.GetComponent<TextMeshProUGUI>();
        Sequence blinkSequence = DOTween.Sequence();
        blinkSequence.Append(text.DOFade(1, 0));
        blinkSequence.AppendInterval(intervalBetweenBlink);
        blinkSequence.Append(text.DOFade(0, 0));
        blinkSequence.AppendInterval(intervalBetweenBlink);
        blinkSequence.SetLoops(blinkCount);
        blinkSequence.OnComplete(() =>
        {
            target.SetActive(false);
            target.GetComponent<TextMeshProUGUI>().color = Colors.TURQUOISE;
        });
    }

    private GameObject GetText()
    {
        GameObject createdObject = null;
        foreach (GameObject obj in _pool)
        {
            if (obj.activeSelf == false)
            {
                createdObject = obj;
                break;
            }
        }

        if (createdObject == null)
            createdObject = CreateNewText();

        if (createdObject != null)
        {
            createdObject.transform.position = new Vector3(-100, -100, 0);
            createdObject.SetActive(true);
        }
        return createdObject;
    }

    private GameObject CreateNewText(Transform parent = null)
    {
        GameObject objectPrefab = _floatTextPrefab;
        GameObject createdObject = null;

        if (objectPrefab != null)
        {
            if (parent == null)
                parent = transform;
            createdObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, parent);
            if (createdObject != null)
            {
                createdObject.SetActive(false);
                _pool.Add(createdObject);
            }
        }
        return createdObject;
    }
}
