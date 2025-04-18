using UnityEngine;

namespace UI.Gameplay.Elements
{
    [RequireComponent(typeof(RectTransform))]
    public class UIPlaceholderView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _originalSizeDelta;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalSizeDelta = _rectTransform.sizeDelta;
        
            gameObject.SetActive(false);
        }

        public void Activate(RectTransform source)
        {
            _rectTransform.sizeDelta = source.sizeDelta;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _rectTransform.sizeDelta = _originalSizeDelta;
            gameObject.SetActive(false);
        }
    }
}