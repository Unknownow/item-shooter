using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectSpawner : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private ShootableObjectSpawnConfig _config;
    public EffectObjectType[] _spawnArray;
    private float _currentTimeBetweenSpawn;
    private float _currentLevel;
    private int _currentPointNeededToNextLevel;
    private int _lastIncreasePoint;
    private float _currentIncreasePercentage;
    private float _spawnCounter;
    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _currentTimeBetweenSpawn = _config.DEFAULT_TIME_BETWEEN_SPAWN;
        _currentPointNeededToNextLevel = _config.DEFAULT_POINT_NEEDED_TO_NEXT_LEVEL;
        _currentLevel = 0;
        _spawnCounter = 0;

        _spawnArray = new EffectObjectType[100];
        InitSpawnArray();
    }

    private void Update()
    {
        if (CheckPointToIncreaseLevel())
            OnIncreaseLevel();
        UpdateSpawnObject();
    }

    // ========== Private methods ==========
    private void InitSpawnArray()
    {
        // Check spawn rate percentage is equal 100 percents.
        int countPercentage = 0;
        for (int i = 0; i < _config.SPAWN_RATE_ARRAY.Length; i++)
            countPercentage += _config.SPAWN_RATE_ARRAY[i];

        if (countPercentage != 100)
        {
            LogUtils.instance.Log(GetClassName(), gameObject.name, "SPAWN PERCENTAGE NOT EQUAL 100", "DISABLE SPAWNER!");
            gameObject.SetActive(false);
            return;
        }

        int spawnArrayIndexCount = 0;
        for (int i = 0; i < _config.SPAWN_RATE_ARRAY.Length; i++)
        {
            int count = _config.SPAWN_RATE_ARRAY[i];
            while (count > 0)
            {
                _spawnArray[spawnArrayIndexCount] = (EffectObjectType)i;
                count -= 1;
                spawnArrayIndexCount += 1;
            }
        }

        ArrayUtils.Shuffle<EffectObjectType>(_spawnArray);
    }

    private bool CheckPointToIncreaseLevel()
    {
        if (Manager.instance.totalPoint >= _lastIncreasePoint + _currentPointNeededToNextLevel && _currentLevel < _config.MAX_LEVEL)
            return true;
        return false;
    }

    private void OnIncreaseLevel()
    {
        _lastIncreasePoint += _currentPointNeededToNextLevel;
        _currentPointNeededToNextLevel = Mathf.CeilToInt(_currentPointNeededToNextLevel * (1 + _currentPointNeededToNextLevel / 100f));
        _currentTimeBetweenSpawn -= _currentTimeBetweenSpawn * _config.PERCENTAGE_INCREASE_EACH_LEVEL / 100f;
        _currentIncreasePercentage = _currentLevel * _config.PERCENTAGE_INCREASE_EACH_LEVEL;
        _currentLevel += 1;
    }

    private void UpdateSpawnObject()
    {
        if (_spawnCounter <= 0)
        {
            SpawnObject();
            _spawnCounter = _currentTimeBetweenSpawn;
            return;
        }
        _spawnCounter -= Time.deltaTime;
    }

    private void SpawnObject()
    {
        GameObject effectObject = EffectObjectPool.instance.GetObject(GetRandomSpawnObject());
        Bounds bounds = effectObject.GetComponent<SpriteRenderer>().bounds;
        Vector3 spawnPosition = new Vector3(Random.Range(0, Screen.width), Random.Range(Screen.height * 11f / 10, Screen.height * 12f / 10));
        spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
        spawnPosition = ObjectUtils.ClampXPositionToScreen(spawnPosition, bounds);

        effectObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        effectObject.GetComponent<IShootableObject>().StartObject(_currentIncreasePercentage);
    }

    private EffectObjectType GetRandomSpawnObject()
    {
        int randomIndex = Random.Range(0, _spawnArray.Length);
        return _spawnArray[randomIndex];
    }
}
