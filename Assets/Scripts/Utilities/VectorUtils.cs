using UnityEngine;

public class VectorUtils
{
    public string getClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static VectorUtils _instance;

    public static VectorUtils instance
    {
        get
        {
            if (_instance == null)
                _instance = new VectorUtils();
            return _instance;
        }
    }

    // public static float Angle(Vector3 from, Vector3 to)
    // {
    //     return Mathf.Atan2(to.y - from.y, to.x - from.x);
    // }

    public float AngleBetweenInRad(Vector2 from, Vector2 to)
    {
        return Mathf.Atan2(from.y - to.y, from.x - to.x);
    }

    public float AngleBetweenInReg(Vector2 from, Vector2 to)
    {
        return AngleBetweenInRad(from, to) * 180 / Mathf.PI;
    }
}