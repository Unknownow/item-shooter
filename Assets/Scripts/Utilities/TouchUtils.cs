using UnityEngine;

public class TouchUtils
{
    public string getClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static TouchUtils _instance;

    public static TouchUtils instance
    {
        get
        {
            if (_instance == null)
                _instance = new TouchUtils();
            return _instance;
        }
    }

    // ========== Public Methods ==========
    public Touch GetTouchByFingerID(int fingerId)
    {
        if (Input.touchCount <= 0)
            throw new UnityException("NO TOUCHES EXIST");

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch currentTouch = Input.GetTouch(i);
            if (currentTouch.fingerId == fingerId)
                return currentTouch;
        }

        throw new UnityException("TOUCH WITH FINGER_ID" + fingerId.ToString() + "DOES NOT EXIST");
    }
}
