using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootableObjectCollision : MapObjectCollision
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

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
