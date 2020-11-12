using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private int minObstacleSpawnPerTime;
    [SerializeField]
    private int maxObstacleSpawnPerTime;
    private float minTimeToSpawn = 0.8f;
    private float maxTimeToSpawn = 2f;

    private float _counter;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _counter = 0;
    }

    private void Update()
    {
        SpawnObstacle();
    }

    // ========== Private methods ==========
    private void SpawnObstacle()
    {
        if (_counter <= 0)
        {
            int amountOfObstacle = Random.Range(minObstacleSpawnPerTime, maxObstacleSpawnPerTime);

            for (int i = 0; i < amountOfObstacle; i++)
            {
                GameObject obstacle = ObstaclePool.instance.GetObstacle(ObstacleType.POINT_1);
                Vector3 spawnPosition = new Vector3(Random.Range(0, Screen.width), Random.Range(Screen.height * 4.0f / 3, Screen.height * 5.0f / 3));
                spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
                obstacle.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
                obstacle.GetComponent<IShootableObject>().StartObject();
            }

            _counter = Random.Range(minTimeToSpawn, maxTimeToSpawn);
            return;
        }
        _counter -= Time.deltaTime;
    }
}
