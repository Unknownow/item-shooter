using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingBulletSystem : MonoBehaviour, IPlayerAttackSystem
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private BulletType _currentBulletType;

    // ========== Public methods ==========
    public void Attack()
    {
        LogUtils.instance.Log(GetClassName(), "Attack()", "NOT YET OVERRIDE");
    }

    public void Attack(Vector3 direction)
    {
        LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", "NOT YET OVERRIDE");
        GameObject bullet = BulletPool.instance.GetBullet(_currentBulletType);
        bullet.transform.position = transform.position;
        bullet.GetComponent<IBulletMovement>().Move(direction);
    }

    public void Attack(GameObject target)
    {
        LogUtils.instance.Log(GetClassName(), "Attack(GameObject target)", "NOT YET OVERRIDE");
    }
}
