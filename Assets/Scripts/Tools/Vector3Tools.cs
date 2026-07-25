using UnityEngine;

public static class Vector3Tools
{
    public static bool IsFinite(this Vector3 vector)
    {
        return vector.x.IsFinite() && vector.y.IsFinite() && vector.z.IsFinite();
    }
}
