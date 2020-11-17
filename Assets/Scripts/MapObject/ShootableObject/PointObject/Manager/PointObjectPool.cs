﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectPool : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static PointObjectPool _instance;
    public static PointObjectPool instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private List<GameObject> _prefabsList = new List<GameObject>();
    private List<GameObject> _pool;
    private PointObjectPool()
    {
        if (_instance == null)
            _instance = this;
        _pool = new List<GameObject>();
    }

    // ========== Public Methods ==========
    public GameObject GetPointObject(PointObjectType type)
    {
        GameObject gotObject = null;
        foreach (GameObject obj in _pool)
        {
            // LogUtils.instance.Log(GetClassName(), "GET_OBJECT", obj.activeSelf.ToString());
            PointObject basePointObject = obj.GetComponent<PointObject>();
            if (basePointObject != null && basePointObject.type == type && obj.activeSelf == false)
            {
                // LogUtils.instance.Log(GetClassName(), "GetObject", obj.activeSelf.ToString());
                gotObject = obj;
                break;
            }
        }
        if (gotObject == null)
            gotObject = CreateObject(type);

        if (gotObject != null)
        {
            gotObject.transform.position = new Vector3(-100, -100, 0);
            gotObject.SetActive(true);
        }

        return gotObject;
    }

    // ========== Private Methods ==========
    private GameObject CreateObject(PointObjectType type, Transform parent = null)
    {
        // LogUtils.instance.Log(GetClassName(), "CreateObject");
        GameObject objectPrefab = null;
        GameObject createdObject = null;

        // LogUtils.instance.Log(GetClassName(), "_prefabsList length", (_prefabsList.Count).ToString());
        foreach (GameObject prefab in _prefabsList)
        {
            PointObject baseObject = prefab.GetComponent<PointObject>();
            // LogUtils.instance.Log(GetClassName(), "baseObject", (baseObject.type).ToString());
            if (baseObject != null && baseObject.type == type)
            {
                // LogUtils.instance.Log(GetClassName(), "CreateObject", (baseObject.type == type).ToString());
                objectPrefab = prefab;
                break;
            }
        }
        // LogUtils.instance.Log(GetClassName(), "CreateObject", "(objectPrefab != null)", (objectPrefab != null).ToString());
        if (objectPrefab != null)
        {
            if (parent == null)
                parent = _instance.transform;
            createdObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, parent);
            if (createdObject != null)
            {
                // LogUtils.instance.Log(GetClassName(), "CreateObject", "(createdObject != null)", (createdObject != null).ToString());
                createdObject.SetActive(false);
                _pool.Add(createdObject);
            }
        }
        return createdObject;
    }
}