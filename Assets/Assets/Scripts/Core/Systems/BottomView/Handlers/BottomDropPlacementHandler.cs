using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Systems.BottomView
{
    public class BottomDropPlacementHandler : IDropPlacementHelper
    {
        private readonly Transform _parentLayer;

        public BottomDropPlacementHandler(Transform parentLayer)
        {
            _parentLayer = parentLayer;
        }
        
        public bool TryDropCluster(UIClusterElementView cluster, PointerEventData eventData)
        {
            cluster.Presenter.SetContainer(null);
            cluster.transform.SetParent(_parentLayer);
            
            return true;
        }
    }
}