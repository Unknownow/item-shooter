using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private float _heightUnitCount;


    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        Camera mainCam = gameObject.GetComponent<Camera>();
        float designRatio = GameSetting.DESIGN_WIDTH / GameSetting.DESIGN_HEIGHT;
        float screenRatio = mainCam.aspect;
        if (designRatio > screenRatio)
            mainCam.orthographicSize = mainCam.orthographicSize * designRatio / screenRatio;
        else if (designRatio < screenRatio)
            mainCam.orthographicSize = mainCam.orthographicSize * screenRatio / designRatio;
    }
}
