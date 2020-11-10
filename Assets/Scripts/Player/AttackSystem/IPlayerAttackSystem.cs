using UnityEngine;

public interface IPlayerAttackSystem
{
    void Attack();
    void Attack(Vector3 direction);
    void Attack(GameObject target);
}
