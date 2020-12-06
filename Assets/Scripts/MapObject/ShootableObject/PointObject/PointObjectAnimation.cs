using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectAnimation : ShootableObjectAnimation
{
    // ========== Fields and properties ==========
    private SplitInHalfAnimation _splitInHalfAnimation;
    private GameObject[] _miniGameObjectList;
    // ========== MonoBehaviour methods ==========
    protected override void Awake()
    {
        base.Awake();
        _splitInHalfAnimation = gameObject.GetComponent<SplitInHalfAnimation>();
        CreateMiniObjects();
    }

    // ========== Public methods ==========
    public override void DoEffectDestroyObject()
    {
        base.DoEffectDestroyObject();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _splitInHalfAnimation.DoEffectSplitInHalf();
    }

    public override void ResetEffectObject()
    {
        base.ResetEffectObject();
        _splitInHalfAnimation.ResetEffect();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        ResetEffectDestroyedByBomb();
    }

    public override void DoEffectObjectAffectedByEffectObject(EffectObjectType objectType, GameObject sourceObject)
    {
        switch (objectType)
        {
            case EffectObjectType.BOMB:
                DoEffectDestroyObjectByBomb(sourceObject);
                break;
            case EffectObjectType.ICE_BOMB:
                DoEffectSlowedByIceBomb(sourceObject);
                break;
            case EffectObjectType.LIGHTNING_BALL:
                DoEffectDestroyObjectByLightningBall(sourceObject);
                break;
            default:
                DoEffectDestroyObject();
                break;
        }
    }

    // ========== Private methods ==========
    private void CreateMiniObjects()
    {
        int minPieces = 16;
        int maxPieces = 32;
        int quantity = Random.Range(minPieces, maxPieces);
        _miniGameObjectList = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        {
            GameObject miniGameObject = new GameObject("miniGameObject_" + i, typeof(SpriteRenderer), typeof(Rigidbody2D));
            miniGameObject.transform.parent = transform;
            SpriteRenderer renderer = miniGameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            renderer.sortingLayerName = SortingLayerList.SHOOTABLE_OBJECT;
            renderer.sortingOrder = 0;
            miniGameObject.GetComponent<Rigidbody2D>().gravityScale = 2.5f;
            _miniGameObjectList[i] = miniGameObject;
        }
        ResetEffectDestroyedByBomb();
    }

    private void ResetEffectDestroyedByBomb()
    {
        foreach (GameObject miniGameObject in _miniGameObjectList)
        {
            miniGameObject.transform.localPosition = Vector3.zero;
            miniGameObject.SetActive(false);
        }
    }

    private void DoEffectDestroyObjectByBomb(GameObject sourceObject)
    {
        StopRotation();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        foreach (GameObject miniGameObject in _miniGameObjectList)
        {
            miniGameObject.transform.localScale = new Vector3(Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f));
            miniGameObject.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            miniGameObject.transform.rotation = Quaternion.Euler(Random.Range(45, 315), Random.Range(45, 315), Random.Range(45, 315));

            byte color = (byte)Random.Range(120, 150);
            miniGameObject.GetComponent<SpriteRenderer>().color = new Color32(color, color, color, 255);
            miniGameObject.SetActive(true);

            Bomb bomb = sourceObject.GetComponent<Bomb>();
            Vector3 positionVectorOffset = miniGameObject.transform.position - sourceObject.transform.position;
            Vector3 direction = positionVectorOffset.normalized;
            float forceMagnitude = bomb.bombForce * (1 - positionVectorOffset.magnitude / bomb.radius);
            if (forceMagnitude < bomb.minBombForce)
                forceMagnitude = bomb.minBombForce;
            Vector3 force = forceMagnitude * direction;
            miniGameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void DoEffectDestroyObjectByLightningBall(GameObject sourceObject)
    {
        StopRotation();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        foreach (GameObject miniGameObject in _miniGameObjectList)
        {
            miniGameObject.transform.localScale = new Vector3(Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f));
            miniGameObject.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            miniGameObject.transform.rotation = Quaternion.Euler(Random.Range(45, 315), Random.Range(45, 315), Random.Range(45, 315));

            byte color = (byte)Random.Range(100, 120);
            miniGameObject.GetComponent<SpriteRenderer>().color = new Color(color, color, color, 255);
            miniGameObject.SetActive(true);
        }
    }
}
