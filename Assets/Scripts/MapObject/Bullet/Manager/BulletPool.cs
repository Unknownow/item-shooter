using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static BulletPool _instance;
    public static BulletPool instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private List<GameObject> _prefabsList = new List<GameObject>();
    private List<GameObject> _pool;
    private BulletPool()
    {
        if (_instance == null)
            _instance = this;
        _pool = new List<GameObject>();
    }

    private EventListener[] _listeners;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // ========== Public Methods ==========
    public GameObject GetBullet(BulletType type)
    {
        GameObject bullet = null;
        foreach (GameObject obj in _pool)
        {
            // LogUtils.instance.Log(GetClassName(), "GET_OBJECT", obj.activeSelf.ToString());
            Bullet baseBullet = obj.GetComponent<Bullet>();
            if (baseBullet != null && baseBullet.type == type && obj.activeSelf == false)
            {
                // LogUtils.instance.Log(GetClassName(), "GetObject", obj.activeSelf.ToString());
                bullet = obj;
                break;
            }
        }

        if (bullet == null)
            bullet = CreateBullet(type);

        if (bullet != null)
        {
            bullet.transform.position = new Vector3(-100, -100, 0);
            bullet.SetActive(true);
        }
        return bullet;
    }

    // ========== Private Methods ==========
    private GameObject CreateBullet(BulletType type, Transform parent = null)
    {
        // LogUtils.instance.Log(GetClassName(), "CreateObject");
        GameObject objectPrefab = null;
        GameObject createdObject = null;

        // LogUtils.instance.Log(GetClassName(), "_prefabsList length", (_prefabsList.Count).ToString());
        foreach (GameObject prefab in _prefabsList)
        {
            Bullet baseBullet = prefab.GetComponent<Bullet>();
            // LogUtils.instance.Log(GetClassName(), "baseBullet", (baseBullet.type).ToString());
            if (baseBullet != null && baseBullet.type == type)
            {
                // LogUtils.instance.Log(GetClassName(), "CreateObject", (baseBullet.type == type).ToString());
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
        _listeners[0] = CustomEventSystem.instance.AddListener(EventCode.ON_RESET_GAME, this, OnReset);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _listeners)
            CustomEventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnReset(object[] eventParam)
    {
        ResetPool();
    }
}
