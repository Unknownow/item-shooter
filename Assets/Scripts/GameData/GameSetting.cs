using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    private static bool _isLog = false;
    public static bool isLog
    {
        get
        {
            return _isLog;
        }
        set
        {
            _isLog = value;
        }
    }
    public const float DESIGN_HEIGHT = 1280;
    public const float DESIGN_WIDTH = 720;
}
