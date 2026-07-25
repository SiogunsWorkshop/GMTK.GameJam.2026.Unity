using UnityEngine;

public static class FloatTools
{
    public static bool IsFinite(this float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    public static float RemapClamped(float value, float inMin, float inMax, float outMin, float outMax)
    {
        float t = Mathf.InverseLerp(inMin, inMax, value);
        return Mathf.Lerp(outMin, outMax, t);
    }

    public static Vector2 AngleToDirection(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
    }
}
