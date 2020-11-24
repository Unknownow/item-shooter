using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectAnimation : MonoBehaviour, IShootableObjectAnimation
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private Color _slowColor;
    private Color _defaultColor;
    private float[] _colorChangeSpeed;
    [SerializeField]
    private float _angularSpeed;
    private float _angularSpeedMultiplier;
    private float _currentAngularSpeed;
    private SplitInHalfAnimation _splitInHalfAnimation;
    private GameObject[] _miniGameObjectList;
    private float _slowDuration;
    // ========== MonoBehaviour methods ==========

    private void Awake()
    {
        _splitInHalfAnimation = gameObject.GetComponent<SplitInHalfAnimation>();
        _colorChangeSpeed = new float[4];
        _defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
        CreateMiniObjects();
    }

    private void Update()
    {
        UpdateRotation();
        UpdateSlowDuration();
    }

    // ========== Public methods ==========
    public void DoEffectStartObject()
    {
        StartRotation();
    }

    public void DoEffectDestroyObject()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StopRotation();
        _splitInHalfAnimation.DoEffectSplitInHalf();
    }

    public void DoEffectDestroyObjectByBomb(GameObject sourceObject)
    {
        StopRotation();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        DoEffectDestroyedByBomb(sourceObject);
    }

    public void ResetEffectObject()
    {
        _splitInHalfAnimation.ResetEffect();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        ResetEffectDestroyedByBomb();
        ResetSlowEffect();
    }

    public void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
    {
        switch (objectType)
        {
            case EffectObjectType.BOMB:
                DoEffectDestroyObject();
                break;
            case EffectObjectType.ICE_BOMB:
                DoEffectSlowedByIceBomb(sourceObject);
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
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + Time.deltaTime * _angularSpeedMultiplier * _currentAngularSpeed);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void CreateMiniObjects()
    {
        int minPieces = 16;
        int maxPieces = 32;
        int quantity = Random.Range(minPieces, maxPieces);
        _miniGameObjectList = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        {
            GameObject miniGameObject = new GameObject("miniGameObject_" + i, typeof(SpriteRenderer), typeof(Rigidbody2D));
            miniGameObject.transform.parent = transform;
            SpriteRenderer renderer = miniGameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            miniGameObject.GetComponent<Rigidbody2D>().gravityScale = 2.5f;
            _miniGameObjectList[i] = miniGameObject;
        }
        ResetEffectDestroyedByBomb();
    }

    private void ResetEffectDestroyedByBomb()
    {
        foreach (GameObject miniGameObject in _miniGameObjectList)
        {
            miniGameObject.transform.localPosition = Vector3.zero;
            miniGameObject.SetActive(false);
        }
    }

    private void DoEffectDestroyedByBomb(GameObject sourceObject)
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        foreach (GameObject miniGameObject in _miniGameObjectList)
        {
            miniGameObject.transform.localScale = new Vector3(Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f));
            miniGameObject.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            miniGameObject.transform.rotation = Quaternion.Euler(Random.Range(45, 315), Random.Range(45, 315), Random.Range(45, 315));

            float color = Random.Range(120, 150);
            miniGameObject.GetComponent<SpriteRenderer>().color = new Color(color / 255f, color / 255f, color / 255f, 1);
            miniGameObject.SetActive(true);

            Bomb bomb = sourceObject.GetComponent<Bomb>();
            Vector3 positionVectorOffset = miniGameObject.transform.position - sourceObject.transform.position;
            Vector3 direction = positionVectorOffset.normalized;
            float forceMagnitude = bomb.bombForce * (1 - positionVectorOffset.magnitude / bomb.radius);
            if (forceMagnitude < bomb.minBombForce)
                forceMagnitude = bomb.minBombForce;
            Vector3 force = forceMagnitude * direction;
            miniGameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void DoEffectSlowedByIceBomb(GameObject sourceObject)
    {
        float slowPercentage = sourceObject.GetComponent<IceBomb>().slowPercentage;
        float slowDuration = sourceObject.GetComponent<IceBomb>().slowDuration;
        SlowDownAngularSpeed(slowPercentage, slowDuration);
        gameObject.GetComponent<SpriteRenderer>().color = _slowColor;

        _colorChangeSpeed[0] = (_defaultColor.r - _slowColor.r) / slowDuration;
        _colorChangeSpeed[1] = (_defaultColor.g - _slowColor.g) / slowDuration;
        _colorChangeSpeed[2] = (_defaultColor.b - _slowColor.b) / slowDuration;
        _colorChangeSpeed[3] = (_defaultColor.a - _slowColor.a) / slowDuration;
    }

    private void UpdateSlowDuration()
    {
        if (_slowDuration <= 0)
        {
            if (_slowDuration != -100f)
                ResetSlowEffect();
            _slowDuration = -1;
            return;
        }
        _slowDuration -= Time.deltaTime;

        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        currentColor.r += _colorChangeSpeed[0] * Time.deltaTime;
        currentColor.g += _colorChangeSpeed[1] * Time.deltaTime;
        currentColor.b += _colorChangeSpeed[2] * Time.deltaTime;
        currentColor.a += _colorChangeSpeed[3] * Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    private void ResetSlowEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        SpeedUpAngularSpeed();
    }

    private void SlowDownAngularSpeed(float percentage = 0, float duration = -100)
    {
        _angularSpeedMultiplier = 1 - percentage / 100f;
        _slowDuration = duration;
    }

    private void SpeedUpAngularSpeed(float percentage = 0, float duration = -100)
    {
        _angularSpeedMultiplier = 1 + percentage / 100f;
        _slowDuration = duration;
    }
}
