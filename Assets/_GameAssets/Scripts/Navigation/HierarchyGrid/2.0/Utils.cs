using UnityEngine;

public static class Utils
{
    public static void SplitArea(Vector2 size, float density,
        out float edgeSize, out int rows, out int cols)
    {
        edgeSize = 10f / density;
        rows = (int)(size.y / edgeSize);
        cols = (int)(size.x / edgeSize);
    }



}
