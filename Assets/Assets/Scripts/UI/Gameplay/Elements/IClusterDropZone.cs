using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public interface IClusterDropZone: IDropHandler
    {
        public bool TryDrop(UIClusterElementView cluster, PointerEventData eventData);
    }
}