using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public class UILetterSlotView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private UILetterView _letterView;
        
        public bool IsOccupied { get; private set; }
        public int Index { get; private set; }

        public void Initialize(int index)
        {
            transform.name = $"LetterSlot_{index}";
            Index = index;
        }
        
        public void SetOccupied(bool flag)
        {
            IsOccupied = flag;
            _background.color = flag ? Color.gray : Color.white;
            gameObject.SetActive(!flag);
        }

        public void SetAsPlaceholder(bool isPlaceholder)
        {
            if (!IsOccupied)
            {
                _background.color = isPlaceholder ? new Color(0.5f, 0f, 0.5f) : Color.white;
            }
        }
    }
}