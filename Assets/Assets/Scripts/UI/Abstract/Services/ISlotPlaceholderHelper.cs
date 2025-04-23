using UI.Gameplay.Elements;

namespace UI.Services
{
    public interface ISlotPlaceholderHelper
    {
        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex);
        public void ClearPlaceholder();
    }
}