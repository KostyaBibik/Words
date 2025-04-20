using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Utils
{
    public static class UIRaycastHelper
    {
        public static List<RaycastResult> RaycastAtScreenPosition(Vector2 screenPosition)
        {
            var results = new List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };
            EventSystem.current.RaycastAll(eventData, results);
            
            return results;
        }

        public static List<RaycastResult> RaycastWithEventData(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            return results;
        }
    }
}