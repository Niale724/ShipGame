using UnityEngine;

public static class ScreenUtils
{
    #region Fields

    // saved to support resolution changes
    static int screenWidth;
    static int screenHeight;

    // cached for efficient boundary checking
    static float screenLeft;
    static float screenRight;
    static float screenTop;
    static float screenBottom;
    #endregion
    public static float ScreenLeft
    {
        get { return screenLeft; }
    }

    public static float ScreenRight
    {
        get { return screenRight; }
    }

    public static float ScreenTop
    {
        get { return screenTop; }
    }

    public static float ScreenBottom
    {
        get { return screenBottom; }
    }

    public static void Initialize()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        float screenZ = -Camera.main.transform.position.z;

        Vector3 lowerLeftCornerScreen = new Vector3(0, 0, screenZ);
        Vector3 lowerLeftCornerWorld = Camera.main.ScreenToWorldPoint(lowerLeftCornerScreen);
        Vector3 topRightCornerScreen = new Vector3(Screen.width, Screen.height, screenZ);
        Vector3 topRightCornerWorld = Camera.main.ScreenToWorldPoint(topRightCornerScreen);

        screenLeft = lowerLeftCornerWorld.x;
        screenRight = topRightCornerWorld.x;
        screenTop = topRightCornerWorld.y;
        screenBottom = lowerLeftCornerWorld.y;
        Debug.Log($"Screen Boundaries: L={screenLeft}, R={screenRight}, T={screenTop}, B={screenBottom}");
    }

    public static Vector2 ClampedPosition(Vector2 position, float colliderHalfWidth, float colliderHalfHeight)
    {
        Vector2 clampedPosition = position;
        clampedPosition.x = Mathf.Clamp(position.x, screenLeft + colliderHalfWidth, screenRight - colliderHalfWidth);
        clampedPosition.y = Mathf.Clamp(position.y, screenBottom + colliderHalfHeight, screenTop - colliderHalfHeight);
        return clampedPosition;
    }

    public static bool IsInScreen(Vector2 position, float colliderHalfWidth, float colliderHalfHeight)
    {
        return position.x >= screenLeft + colliderHalfWidth &&
               position.x <= screenRight - colliderHalfWidth &&
               position.y >= screenBottom + colliderHalfHeight &&
               position.y <= screenTop - colliderHalfHeight;
    }
}