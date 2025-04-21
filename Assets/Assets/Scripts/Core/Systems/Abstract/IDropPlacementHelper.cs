using UI.Gameplay.Elements;
using UnityEngine.EventSystems;

namespace Core.Systems
{
    public interface IDropPlacementHelper
    {
        public bool TryDropCluster(UIClusterElementView cluster, PointerEventData eventData);
    }
}