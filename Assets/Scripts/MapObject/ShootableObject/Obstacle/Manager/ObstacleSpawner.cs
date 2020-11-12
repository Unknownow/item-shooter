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
    private float minTimeToSpawn = 0.2f;
    private float maxTimeToSpawn = 0.5f;

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
            int[] listType = (int[])System.Enum.GetValues(typeof(ObstacleType));
            ObstacleType randomType = (ObstacleType)listType[Random.Range(0, listType.Length)];
            GameObject obstacle = ObstaclePool.instance.GetObstacle(randomType);
            Vector3 spawnPosition = new Vector3(Random.Range(0, Screen.width), Random.Range(Screen.height * 4.0f / 3, Screen.height * 5.0f / 3));
            spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
            obstacle.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            obstacle.GetComponent<IShootableObject>().StartObject();

            _counter = Random.Range(minTimeToSpawn, maxTimeToSpawn);
            return;
        }
        _counter -= Time.deltaTime;
    }
}
