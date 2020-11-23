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
}
