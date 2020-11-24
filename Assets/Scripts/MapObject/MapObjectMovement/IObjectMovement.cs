using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectMovement
{
    float movementSpeed { get; set; }
    float accelerationRate { get; set; }
    Vector3 moveDirection { get; set; }
    Vector3 accelerationDirection { get; set; }
    Vector3 velocity { get; }
    Vector3 acceleration { get; }
    void ResetSpeedToDefault();
    void MoveBy(Vector3 distance);
    void MoveTo(Vector3 endPosition);
    void Move();
    void Move(Vector3 moveDirection);
    void Move(Vector3 directmoveDirectionion, Vector3 accelerationDirection);
    void StopMoving();
    void SpeedUp(float percentage, float duration = -100f);
    void SlowDown(float percentage, float duration = -100f);
}
