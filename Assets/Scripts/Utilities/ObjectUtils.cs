using UnityEngine;
public static class ObjectUtils
{
    public static Vector3 ClampXPositionToScreen(Vector3 position, Bounds bounds)
    {

        float minXPointInScreen = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
        float maxXPointInScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        position.x = Mathf.Clamp(position.x, minXPointInScreen + bounds.extents.x, maxXPointInScreen - bounds.extents.x);
        return position;
    }

    public static Vector3 ClampYPositionToScreen(Vector3 position, Bounds bounds)
    {

        float minYPointInScreen = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        float maxYPointInScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        position.y = Mathf.Clamp(position.y, minYPointInScreen + bounds.extents.y, maxYPointInScreen - bounds.extents.y);
        return position;
    }

    public static Vector3 ClampPositionToScreen(Vector3 position, Bounds bounds)
    {
        return ClampYPositionToScreen(ClampXPositionToScreen(position, bounds), bounds);
    }

    public static bool CheckIfSpriteAppearEntirelyInScreen(SpriteRenderer spriteRenderer)
    {
        Bounds rendererBound = spriteRenderer.bounds;
        Vector2 minScreenPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 maxScreenPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return rendererBound.min.x >= minScreenPosition.x &&
            rendererBound.min.y >= minScreenPosition.y &&
            rendererBound.max.x <= maxScreenPosition.x &&
            rendererBound.max.y <= maxScreenPosition.y;
    }

    public static bool CheckIfSpriteAppearInScreen(SpriteRenderer spriteRenderer)
    {
        Bounds rendererBound = spriteRenderer.bounds;
        Vector2 minScreenPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 maxScreenPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return (rendererBound.min.x > minScreenPosition.x - rendererBound.size.x && rendererBound.min.x < maxScreenPosition.x) &&
        (rendererBound.min.y > minScreenPosition.y - rendererBound.size.y && rendererBound.min.y < maxScreenPosition.y);
    }
}
