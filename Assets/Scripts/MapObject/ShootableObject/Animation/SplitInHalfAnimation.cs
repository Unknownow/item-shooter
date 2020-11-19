using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitInHalfAnimation : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private GameObject _leftHalf;
    private GameObject _rightHalf;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        if (transform.childCount <= 0)
            return;
        _leftHalf = transform.GetChild(0).gameObject;
        _rightHalf = transform.GetChild(1).gameObject;
        _leftHalf.SetActive(false);
        _rightHalf.SetActive(false);
    }

    // ========== Public methods ==========
    public void DoEffectSplitInHalf()
    {
        if (transform.childCount <= 0)
            return;
        ResetEffect();

        Vector3 currentRotation = transform.rotation.eulerAngles;

        _leftHalf.transform.localPosition = Vector2.zero;
        _leftHalf.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _leftHalf.SetActive(true);
        _leftHalf.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 3).normalized * 10, ForceMode2D.Impulse);

        _rightHalf.transform.localPosition = Vector2.zero;
        _rightHalf.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _rightHalf.SetActive(true);
        _rightHalf.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 3).normalized * 10, ForceMode2D.Impulse);
    }

    public void ResetEffect()
    {
        _leftHalf.transform.localPosition = Vector2.zero;
        _leftHalf.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _leftHalf.SetActive(false);
        
        _rightHalf.transform.localPosition = Vector2.zero;
        _rightHalf.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _rightHalf.SetActive(false);
    }
}
