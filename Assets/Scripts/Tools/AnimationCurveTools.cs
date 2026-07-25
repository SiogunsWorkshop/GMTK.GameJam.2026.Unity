using UnityEngine;

public static class AnimationCurveTools
{
    public static float Duration(this AnimationCurve curve)
    {
        if (curve == null || curve.length == 0)
            return 0f;

        return curve.keys[curve.length - 1].time;
    }
}
