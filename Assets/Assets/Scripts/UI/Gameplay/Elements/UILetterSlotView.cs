using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public class UILetterSlotView : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private Color _occupiedColor = new Color(0.8f, 0f, 0.1f);
        [SerializeField] private Color _basedColor = Color.yellow;
        [SerializeField] private Color _placeholderColor = new Color(0.5f, 0f, 0.5f);
        
        [Header("References")] 
        [SerializeField] private Image _background;
        
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
            _background.color = flag 
                ? _occupiedColor 
                : _basedColor;
            
            gameObject.SetActive(!flag);
        }

        public void SetAsPlaceholder(bool isPlaceholder)
        {
            if (!IsOccupied)
            {
                _background.color = isPlaceholder 
                    ? _placeholderColor
                    : _basedColor;
            }
        }
    }
}