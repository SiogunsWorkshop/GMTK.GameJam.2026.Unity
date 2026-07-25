using UnityEngine;

public static class CanvasGroupTools
{
    public static void SetProperties(this CanvasGroup canvasGroup, float alpha, bool interactable, bool blocksRaycasts)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = blocksRaycasts;
    }
}
