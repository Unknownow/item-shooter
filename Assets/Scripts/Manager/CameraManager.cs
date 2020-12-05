using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Singleton instance ==========
    private static CameraManager _instance;
    public static CameraManager instance
    {
        get
        {
            return _instance;
        }
    }
    private CameraManager()
    {
        if (_instance == null)
            _instance = this;
    }
    // ========== Fields and properties ==========
    [SerializeField]
    private Camera _mainCamera;
    public static Camera mainCamera { get { return _instance._mainCamera; } }
}
