using UnityEngine;

namespace UI.Gameplay.Elements
{
    public class UILetterSlotView : MonoBehaviour
    {
        [SerializeField] private UILetterView _letterView;
        
        public bool IsOccupied { get; private set; }
        
    }
}