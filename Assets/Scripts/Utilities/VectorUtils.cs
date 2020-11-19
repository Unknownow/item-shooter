using UnityEngine;

public static class VectorUtils
{
    public static float AngleBetweenInRad(Vector2 from, Vector2 to)
    {
        return Mathf.Atan2(from.y - to.y, from.x - to.x);
    }

    public static float AngleBetweenInReg(Vector2 from, Vector2 to)
    {
        return AngleBetweenInRad(from, to) * 180 / Mathf.PI;
    }
}