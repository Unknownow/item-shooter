using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootableObjectCollision : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
    // ========== Fields and properties ==========

    // ========== MonoBehaviour methods ==========
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagList.PLAYER))
        {
            OnHitPlayer(other.gameObject);
        }
    }

    // ========== Public methods ==========
    public abstract void OnGetHitByBullet(GameObject bullet);

    // ========== Protected methods ==========
    protected abstract void OnHitPlayer(GameObject player);
}
