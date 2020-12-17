using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectSpawner : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private ShootableObjectSpawnConfig _config;
    private PointObjectType[] _spawnArray;
    private float _currentTimeBetweenSpawn;
    private float _currentLevel;
    private int _currentPointNeededToNextLevel;
    private int _lastIncreasePoint;
    private float _currentIncreasePercentage;
    private float _spawnCounter;
    private EventListener[] _listeners;
    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _spawnArray = new PointObjectType[100];
        InitSpawnArray();
        ResetSpawner();
    }

    private void Update()
    {
        if (!GameFlowManager.instance.isPlaying)
            return;
        if (CheckPointToIncreaseLevel())
            OnIncreaseLevel();
        UpdateSpawnObject();
    }

    // ========== Private methods ==========
    private void ResetSpawner()
    {
        _currentTimeBetweenSpawn = _config.DEFAULT_TIME_BETWEEN_SPAWN;
        _currentPointNeededToNextLevel = _config.DEFAULT_POINT_NEEDED_TO_NEXT_LEVEL;
        _currentLevel = 0;
        _spawnCounter = 0;
    }

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
                _spawnArray[spawnArrayIndexCount] = (PointObjectType)i;
                count -= 1;
                spawnArrayIndexCount += 1;
            }
        }

        ArrayUtils.Shuffle<PointObjectType>(_spawnArray);
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
        GameObject pointObject = PointObjectPool.instance.GetObject(GetRandomSpawnObject());

        Bounds bounds = pointObject.GetComponent<SpriteRenderer>().bounds;
        Vector3 spawnPosition = new Vector3(Random.Range(0, Screen.width), Random.Range(Screen.height * 11f / 10, Screen.height * 12f / 10));
        spawnPosition = CameraManager.mainCamera.ScreenToWorldPoint(spawnPosition);
        spawnPosition = ObjectUtils.ClampXPositionToScreen(spawnPosition, bounds);

        pointObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        pointObject.GetComponent<IShootableObject>().StartObject(_currentIncreasePercentage);

        // pointObject.GetComponent<IShootableObject>().StartObject(_currentIncreasePercentage);
    }

    private PointObjectType GetRandomSpawnObject()
    {
        int randomIndex = Random.Range(0, _spawnArray.Length);
        return _spawnArray[randomIndex];
    }

    // ========== Event listener methods ==========
    private void AddListeners()
    {
        _listeners = new EventListener[1];
        _listeners[0] = CustomEventSystem.instance.AddListener(EventCode.ON_RESET_GAME, this, OnReset);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
            CustomEventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnReset(object[] eventParam)
    {
        ResetSpawner();
    }
}
