using UnityEngine;

public static class Vector2Tools
{
    public static void EnsureMinMaxOrder(ref Vector2 vector)
    {
        if (vector.x > vector.y)
            (vector.x, vector.y) = (vector.y, vector.x);
    }
}
