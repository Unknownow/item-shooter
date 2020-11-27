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
}
