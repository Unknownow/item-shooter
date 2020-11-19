using UnityEngine;

public static class TouchUtils
{
    // ========== Public Methods ==========
    public static Touch GetTouchByFingerID(int fingerId)
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
