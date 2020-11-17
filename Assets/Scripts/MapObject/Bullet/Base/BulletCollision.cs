using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== MonoBehaviour methods ==========
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagList.SHOOTABLE_OBJECT))
            OnHitShootableObject(other.gameObject);
    }

    protected void OnHitShootableObject(GameObject shootableObject)
    {
        SpriteRenderer renderer = shootableObject.gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null || !ObjectUtils.instance.CheckIfSpriteAppearInScreen(renderer))
            return;

        ShootableObjectCollision shootableObjectCollision = shootableObject.GetComponent<ShootableObjectCollision>();
        shootableObjectCollision.OnGetHitByBullet(gameObject);
        gameObject.SetActive(false);
    }
}
