using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Singleton instance ==========
    private static GameFlowManager _instance;
    public static GameFlowManager instance
    {
        get
        {
            return _instance;
        }
    }
    private GameFlowManager()
    {
        if (_instance == null)
            _instance = this;
    }

    private bool _isPlaying;
    public bool isPlaying { get { return _isPlaying; } }
    // ========== Fields and properties ==========
    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _isPlaying = true;
    }
    // ========== Public methods ==========
    public void OnPause()
    {
        _isPlaying = false;
    }

    public void OnResume()
    {
        _isPlaying = true;
    }
    // ========== Private methods ==========
}
