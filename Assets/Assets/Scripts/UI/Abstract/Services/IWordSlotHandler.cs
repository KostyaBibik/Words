using UI.Gameplay.Elements;

namespace UI.Services
{
    public interface IWordSlotHandler
    {
        public bool IsValidDropPosition(int startIndex, int length);
        public void OccupySlots(int startIndex, int length);
        public void ReleaseSlots(UIClusterElementView cluster);
        public void ReevaluateFullState();
    }
}