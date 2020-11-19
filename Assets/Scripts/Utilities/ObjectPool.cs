using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<Type, MainClass> : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static ObjectPool<Type, MainClass> _instance;
    public static ObjectPool<Type, MainClass> instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private List<GameObject> _prefabsList = new List<GameObject>();
    private List<GameObject> _pool;
    private ObjectPool()
    {
        if (_instance == null)
            _instance = this;
        _pool = new List<GameObject>();
    }

    // ========== Public Methods ==========
    public GameObject GetObject(Type type)
    {
        GameObject createdObject = null;
        foreach (GameObject obj in _pool)
        {
            // LogUtils.instance.Log(GetClassName(), "GET_OBJECT", obj.activeSelf.ToString());
            MainClass baseObject = obj.GetComponent<MainClass>();
            if (baseObject != null && GetObjectType(baseObject).Equals(type) && obj.activeSelf == false)
            {
                // LogUtils.instance.Log(GetClassName(), "GetObject", obj.activeSelf.ToString());
                createdObject = obj;
                break;
            }
        }

        if (createdObject == null)
            createdObject = CreateObject(type);

        if (createdObject != null)
        {
            createdObject.transform.position = new Vector3(-100, -100, 0);
            createdObject.SetActive(true);
        }
        return createdObject;
    }

    // ========== Private Methods ==========
    protected GameObject CreateObject(Type type, Transform parent = null)
    {
        // LogUtils.instance.Log(GetClassName(), "CreateObject");
        GameObject objectPrefab = null;
        GameObject createdObject = null;

        // LogUtils.instance.Log(GetClassName(), "_prefabsList length", (_prefabsList.Count).ToString());
        foreach (GameObject prefab in _prefabsList)
        {
            MainClass baseObject = prefab.GetComponent<MainClass>();
            // LogUtils.instance.Log(GetClassName(), "baseObject", (baseObject.type).ToString());
            if (baseObject != null && GetObjectType(baseObject).Equals(type))
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

    protected abstract Type GetObjectType(MainClass baseObject);
}
