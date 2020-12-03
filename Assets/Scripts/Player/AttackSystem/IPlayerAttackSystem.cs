using UnityEngine;

public interface IPlayerAttackSystem
{
    void Attack();
    void Attack(Vector3 direction);
    void Attack(Vector2 position);
    void Attack(GameObject target);
}
