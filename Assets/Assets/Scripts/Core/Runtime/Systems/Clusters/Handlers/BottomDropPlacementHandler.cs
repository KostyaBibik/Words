using UI.Gameplay.Elements;
using UI.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Systems
{
    public sealed class BottomDropPlacementHandler : IDropPlacementHelper
    {
        public bool TryDropCluster(UIClusterElementView cluster, PointerEventData eventData, Transform parentLayer)
        {
            cluster.Presenter.SetContainer(null);
            cluster.transform.SetParent(parentLayer);
            
            return true;
        }
    }
}