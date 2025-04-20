using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public interface IClusterDropZone: IDropHandler
    {
        public UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData);
    }
}