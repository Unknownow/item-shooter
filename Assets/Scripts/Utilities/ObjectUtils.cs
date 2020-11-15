using UnityEngine;
public class ObjectUtils
{
    public string getClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static ObjectUtils _instance;

    public static ObjectUtils instance
    {
        get
        {
            if (_instance == null)
                _instance = new ObjectUtils();
            return _instance;

        }
    }

    public bool CheckSpriteBetweenTwoX(SpriteRenderer spriteRenderer, float x1, float x2)
    {
        if (x1 > x2)
        {
            LogUtils.instance.Log(getClassName(), "CheckSpriteBetweenTwoX", "X1 =", x1, "X2 =", x2, "X1 > X2 INVALID");
            return false;
        }
        Bounds rendererBound = spriteRenderer.bounds;
        if (rendererBound.min.x >= x1 && rendererBound.max.x <= x2)
            return true;
        return false;
    }
}
