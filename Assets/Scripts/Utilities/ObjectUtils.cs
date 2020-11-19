using UnityEngine;
public static class ObjectUtils
{
    // ========== Fields and properties ==========
    public static bool CheckSpriteBetweenTwoX(SpriteRenderer spriteRenderer, float x1, float x2)
    {
        if (x1 > x2)
        {
            LogUtils.instance.Log("ObjectUtils", "CheckSpriteBetweenTwoX", "X1 =", x1, "X2 =", x2, "X1 > X2 INVALID");
            return false;
        }
        Bounds rendererBound = spriteRenderer.bounds;
        if (rendererBound.min.x >= x1 && rendererBound.max.x <= x2)
            return true;
        return false;
    }

    public static bool CheckIfSpriteInScreen(SpriteRenderer spriteRenderer)
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
