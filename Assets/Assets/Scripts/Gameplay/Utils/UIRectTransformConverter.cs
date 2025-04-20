using UnityEngine;

namespace UI.Gameplay.Utils
{
    public static class UIRectTransformConverter
    {
        public static Vector3 ScreenToWorld(RectTransform rectTransform, Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform, screenPosition, null, out var worldPoint);
            return worldPoint;
        }

        public static Vector2 ScreenToLocal(RectTransform rectTransform, Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, screenPosition, null, out var localPoint);
            return localPoint;
        }
    }
}