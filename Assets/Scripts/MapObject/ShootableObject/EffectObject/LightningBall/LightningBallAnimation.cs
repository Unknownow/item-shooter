using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallAnimation : ShootableObjectAnimation
{
    // ========== Fields and properties ==========

    private const string IDLE_ANIMATION = "Idle";

    // ========== MonoBehaviour methods ==========
    // ========== Public methods ==========
    public override void DoEffectStartObject()
    {
        base.DoEffectStartObject();
        gameObject.GetComponent<Animator>().Play(IDLE_ANIMATION);
    }

    public override void DoEffectDestroyObject()
    {
        base.DoEffectDestroyObject();

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    public override void ResetEffectObject()
    {
        base.ResetEffectObject();
        gameObject.GetComponent<Animator>().Play(IDLE_ANIMATION);
    }

    public override void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
    {
        switch (objectType)
        {
            case EffectObjectType.BOMB:
                DoEffectDestroyObject();
                break;
            case EffectObjectType.ICE_BOMB:
                DoEffectSlowedByIceBomb(sourceObject);
                break;
            default:
                DoEffectDestroyObject();
                break;
        }
    }
    // ========== Private methods ==========
}
