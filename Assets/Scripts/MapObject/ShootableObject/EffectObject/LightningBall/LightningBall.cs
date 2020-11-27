using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class LightningBall : EffectObject
{
    // ========== Fields and properties ==========
    [SerializeField]
    private LightningBallConfig _config;
    public override MapObjectConfig config
    {
        get
        {
            return _config;
        }
    }

    [SerializeField]
    private LayerMask _affectedLayer;

    [SerializeField]
    private float _timeBeforeDeactivateObject;

    public float radius
    {
        get
        {
            return ((LightningBallConfig)config).RADIUS;
        }
    }

    private int _defaultTargetCount
    {
        get
        {
            return ((LightningBallConfig)config).TARGET_COUNT;
        }
    }
    private float _delayBetweenChain
    {
        get
        {
            return ((LightningBallConfig)config).DELAY_BETWEEN_CHAIN;
        }
    }

    private GameObjectPool _lightningBoltPool;
    private GameObject _lastStrokeObject;
    private GameObject _currentStrokeObject;
    private List<GameObject> _affectedGameObject;

    // ========== MonoBehaviour methods ==========
    protected void Awake()
    {
        _affectedGameObject = new List<GameObject>();
        _lightningBoltPool = GameObject.FindGameObjectWithTag(TagList.LIGHTNING_BOLT_POOL).GetComponent<GameObjectPool>();
    }

    // ========== Public methods ==========
    public override void StartObject()
    {
        LogUtils.instance.Log(GetClassName(), "StartObject", "NOT_YET_IMPLEMENT");
    }

    public override void StartObject(float percentIncrease)
    {
        ResetObject();
        float defaultMovementSpeed = gameObject.GetComponent<IObjectMovement>().movementSpeed;
        gameObject.GetComponent<IObjectMovement>().movementSpeed = defaultMovementSpeed * (1 + percentIncrease / 100);
        float defaultAccelerationRate = gameObject.GetComponent<IObjectMovement>().accelerationRate;
        gameObject.GetComponent<IObjectMovement>().accelerationRate = defaultAccelerationRate * (1 + percentIncrease / 100);
        gameObject.GetComponent<IObjectMovement>().Move(Vector3.down, Vector3.down);

        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectStartObject();
    }

    public override void DestroyObjectByBullet()
    {
        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectDestroyObject();
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.GetComponent<Collider2D>().enabled = false;
        OnDetonate();
        Invoke("DeactivateObject", _timeBeforeDeactivateObject);
    }

    public override void ResetObject()
    {
        gameObject.GetComponent<IObjectMovement>().StopMoving();
        gameObject.GetComponent<IObjectMovement>().moveDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().accelerationDirection = Vector3.zero;
        gameObject.GetComponent<IObjectMovement>().ResetSpeedToDefault();

        gameObject.GetComponent<IShootableObjectAnimation>().ResetEffectObject();
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public override void DeactivateObject()
    {
        gameObject.SetActive(false);
        ResetObject();
    }

    public override void OnAffectedByEffectObject(EffectObjectType type, GameObject sourceObject)
    {
        switch (type)
        {
            case EffectObjectType.BOMB:
                DestroyObjectByBullet();
                break;
            case EffectObjectType.ICE_BOMB:
                OnSlowedByIceBomb(sourceObject);
                break;
            case EffectObjectType.LIGHTNING_BALL:
                DestroyObjectByBullet();
                break;
            default:
                DestroyObjectByBullet();
                break;
        }
    }

    // ========== Private methods ==========
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ((LightningBallConfig)_config).RADIUS);
    }

    private void OnSlowedByIceBomb(GameObject sourceObject)
    {
        float slowPercentage = sourceObject.GetComponent<IceBomb>().slowPercentage;
        float slowDuration = sourceObject.GetComponent<IceBomb>().slowDuration;
        gameObject.GetComponent<IObjectMovement>().SlowDown(slowPercentage, slowDuration);
        gameObject.GetComponent<IShootableObjectAnimation>().DoEffectObjectAffectedByEffectObject(EffectObjectType.ICE_BOMB, sourceObject);
    }

    private void OnDetonate()
    {
        _currentStrokeObject = gameObject;
        _affectedGameObject.Clear();
        _affectedGameObject.Add(_currentStrokeObject);
        StartCoroutine("DoLightningStrike");
    }

    private IEnumerator DoLightningStrike()
    {
        while (_affectedGameObject.Count <= ((LightningBallConfig)config).TARGET_COUNT)
        {
            LightningStrike();
            if (_currentStrokeObject == null)
                break;
            yield return new WaitForSeconds(((LightningBallConfig)config).DELAY_BETWEEN_CHAIN);
        }
    }

    private void LightningStrike()
    {
        _lastStrokeObject = _currentStrokeObject;
        _currentStrokeObject = GetAffectedObject(_lastStrokeObject);

        if (_currentStrokeObject == null)
            return;

        IShootableObject currentStrokeClass = _currentStrokeObject.GetComponent<IShootableObject>();
        currentStrokeClass.OnAffectedByEffectObject(EffectObjectType.LIGHTNING_BALL, gameObject);
        _affectedGameObject.Add(_currentStrokeObject);

        LightningBoltScript lightningBolt = _lightningBoltPool.GetObject().GetComponent<LightningBoltScript>();
        lightningBolt.transform.position = Vector3.zero;
        lightningBolt.StartObject = _lastStrokeObject;
        lightningBolt.EndObject = _currentStrokeObject;
        lightningBolt.ManualMode = true;
        lightningBolt.Trigger();
    }

    private GameObject GetAffectedObject(GameObject sourceObject = null)
    {
        if (sourceObject == null)
            return null;
        Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(sourceObject.transform.position, ((LightningBallConfig)config).RADIUS, _affectedLayer);
        GameObject affectedObject = null;

        if (affectedColliders.Length >= 0)
        {
            float minDistance = ((LightningBallConfig)config).RADIUS + 1;
            ArrayUtils.Shuffle(affectedColliders);
            foreach (Collider2D collider in affectedColliders)
            {
                SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
                if (_affectedGameObject.Contains(collider.gameObject) || renderer == null || collider.GetComponent<IShootableObject>() == null)
                    // if (_affectedGameObject.Contains(collider.gameObject) || renderer == null || !ObjectUtils.CheckIfSpriteInScreen(renderer)|| collider.GetComponent<IShootableObject>() == null )
                    continue;

                float distance = Vector3.Distance(sourceObject.transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    affectedObject = collider.gameObject;
                    minDistance = distance;
                }
            }
        }
        return affectedObject;
    }
}
