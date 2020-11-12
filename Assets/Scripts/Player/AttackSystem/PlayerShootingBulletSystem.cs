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
    private Transform _gun;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        // Get gun in child
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(TagList.PLAYER_GUN))
            {
                _gun = transform.GetChild(i);
                break;
            }
        }
    }

    // ========== Public methods ==========
    public void Attack()
    {
        LogUtils.instance.Log(GetClassName(), "Attack()", "NOT YET OVERRIDE");
    }

    public void Attack(Vector3 direction)
    {
        LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", direction.ToString());
        float directionAngle = Vector3.Angle(Vector3.up, direction);
        if (directionAngle > 90)
        {
            LogUtils.instance.Log(GetClassName(), "Attack(Vector3 direction)", "DIRECTION_ANGLE = ", directionAngle.ToString(), "NOT_VALID");
            if (Vector3.Angle(Vector3.right, direction) < 90)
                direction = Vector3.right;
            else
                direction = -Vector3.right;
        }
        GameObject bullet = BulletPool.instance.GetBullet(_currentBulletType);
        bullet.transform.position = _gun.position;
        bullet.GetComponent<IObjectMovement>().Move(direction);
    }

    public void Attack(GameObject target)
    {
        LogUtils.instance.Log(GetClassName(), "Attack(GameObject target)", "NOT YET OVERRIDE");
    }
}
