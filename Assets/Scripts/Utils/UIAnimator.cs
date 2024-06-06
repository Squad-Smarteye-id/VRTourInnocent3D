using UnityEngine;

namespace VRInnocent.Utils
{
    public static class UIAnimator
    {
        // Method contoh untuk menggeser panel
        public static void SlideHorizontalWithFade(GameObject panelContainer, CanvasGroup canvasGroup, float targetX, float targetAlpha)
        {
            LeanTween.moveLocalX(panelContainer, targetX, 1.0f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alphaCanvas(canvasGroup, targetAlpha, .5f).setEase(LeanTweenType.easeOutQuad);
        }
    }
}
