using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallAnimation : ShootableObjectAnimation
{
    // ========== Fields and properties ==========

    private const string IDLE_ANIMATION = "Idle";
    private const string EXPLOSION = "Explosion";

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

        gameObject.GetComponent<Animator>().Play(EXPLOSION);
        float time = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Invoke("OnExplosionDone", time);

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    public override void ResetEffectObject()
    {
        base.ResetEffectObject();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
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
            case EffectObjectType.LIGHTNING_BALL:
                DoEffectDestroyObject();
                break;
            default:
                DoEffectDestroyObject();
                break;
        }
    }
    // ========== Private methods ==========
    private void OnExplosionDone()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
