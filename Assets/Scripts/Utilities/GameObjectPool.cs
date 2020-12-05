using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private GameObject _prefab;
    private List<GameObject> _pool = new List<GameObject>();

    private EventListener[] _listeners;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Public methods ==========
    public GameObject GetObject()
    {
        GameObject createdObject = null;
        foreach (GameObject obj in _pool)
        {
            // LogUtils.instance.Log(GetClassName(), "GET_OBJECT", obj.activeSelf.ToString());
            if (obj.activeSelf == false)
            {
                // LogUtils.instance.Log(GetClassName(), "GetObject", obj.activeSelf.ToString());
                createdObject = obj;
                break;
            }
        }

        if (createdObject == null)
            createdObject = CreateNewObject();

        if (createdObject != null)
        {
            createdObject.transform.position = new Vector3(-100, -100, 0);
            createdObject.SetActive(true);
        }
        return createdObject;
    }

    // ========== Private Methods ==========
    private GameObject CreateNewObject(Transform parent = null)
    {
        // LogUtils.instance.Log(GetClassName(), "CreateObject");
        GameObject objectPrefab = _prefab;
        GameObject createdObject = null;

        // LogUtils.instance.Log(GetClassName(), "CreateObject", "(objectPrefab != null)", (objectPrefab != null).ToString());
        if (objectPrefab != null)
        {
            if (parent == null)
                parent = transform;
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

    private void ResetPool()
    {
        foreach (GameObject item in _pool)
        {
            item.SetActive(false);
        }
    }

    // ========== Event listener Methods ==========
    private void AddListeners()
    {
        _listeners = new EventListener[1];
        _listeners[0] = EventSystem.instance.AddListener(EventCode.ON_RESET_GAME, this, OnReset);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnReset(object[] eventParam)
    {
        ResetPool();
    }
}
