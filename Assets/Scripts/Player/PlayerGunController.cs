using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========

    [SerializeField]
    private GameObject[] _bulletPrefabList;

    // ========== MonoBehaviour methods ==========

    // ========== Public methods ==========
    public void ChangeWeapon(BulletType bulletType)
    {
        Transform currentWeapon = transform.GetChild(0);
        if (currentWeapon != null)
            GameObject.Destroy(currentWeapon);

        GameObject newBullet = Instantiate(_bulletPrefabList[(int)bulletType], Vector3.zero, Quaternion.identity, transform);
        newBullet.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    public void ChangeWeaponDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
}
