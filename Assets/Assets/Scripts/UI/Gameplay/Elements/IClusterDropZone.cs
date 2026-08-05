using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public interface IClusterDropZone
    {
        public UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData);
    }
}