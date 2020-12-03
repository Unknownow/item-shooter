using UnityEngine;

public interface IPlayerAttackSystem
{
    void Attack();
    void Attack(Vector3 direction);
    void Attack(Vector2 position);
    void Attack(GameObject target);
    void ChangeBulletShootIntervalMultiplier(float multiplier, float duration);
    void ResetBulletShootIntervalMultiplier();
}
