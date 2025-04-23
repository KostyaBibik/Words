using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Services
{
    public interface IDropPlacementHelper
    {
        public bool TryDropCluster(UIClusterElementView cluster, PointerEventData eventData, Transform parentLayer);
    }
}