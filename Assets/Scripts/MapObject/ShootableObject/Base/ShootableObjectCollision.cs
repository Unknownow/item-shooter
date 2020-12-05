using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootableObjectCollision : MapObjectCollision
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (!GameFlowManager.instance.isPlaying)
            return;
        if (other.CompareTag(TagList.PLAYER))
            OnHitPlayer(other.gameObject);
    }

    // ========== Public methods ==========
    public virtual void OnGetHitByBullet(GameObject bullet)
    {
        gameObject.GetComponent<IShootableObject>().DestroyObjectByBullet();
    }

    // ========== Protected methods ==========
    protected abstract void OnHitPlayer(GameObject player);
    protected override void OnHitDestroyer()
    {
        base.OnHitDestroyer();
        gameObject.GetComponent<IShootableObject>().DeactivateObject();
    }
}
