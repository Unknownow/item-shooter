using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletMovement
{
    float movementSpeed { get; set; }
    float accelerationRate { get; set; }
    Vector3 moveDirection { get; set; }
    Vector3 accelerationDirection { get; set; }
    Vector3 velocity { get; }
    Vector3 acceleration { get; }
    void MoveBy(Vector3 distance);
    void MoveTo(Vector3 endPosition);
    void Move(Vector3 direction);
    void StopMoving();
}
