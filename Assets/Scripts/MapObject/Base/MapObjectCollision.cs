using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectCollision : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== MonoBehaviour Methods ==========
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagList.OBJECT_DESTROYER))
        {
            LogUtils.instance.Log(GetClassName(), "NAME =", gameObject.name, "DESTROYED");
            gameObject.SetActive(false);
        }
    }
}
