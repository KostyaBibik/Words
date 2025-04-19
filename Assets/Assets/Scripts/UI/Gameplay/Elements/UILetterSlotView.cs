using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public class UILetterSlotView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private UILetterView _letterView;
        
        public bool IsOccupied { get; private set; }
        
        public void SetOccupied(bool value)
        {
            IsOccupied = value;
            _background.color = value ? Color.gray : Color.white;
            gameObject.SetActive(!value);
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