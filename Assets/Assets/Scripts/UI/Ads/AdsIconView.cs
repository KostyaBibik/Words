using UnityEngine;

namespace UI.Ads
{
    // The "No Ads" icon lives on its own always-active root Canvas (it needs to render above
    // every window regardless of which one is open), so window Show/Hide can't reach it.
    // A CanvasGroup keeps it consistent with how every other UIView hides itself, and avoids
    // SetActive(false), which would kill NoAdButton's in-flight price-image download coroutine.
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class AdsIconView : MonoBehaviour, IAdsIconView
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            Hide();
        }

        public void Show() => SetVisible(true);

        public void Hide() => SetVisible(false);

        private void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.interactable = visible;
            _canvasGroup.blocksRaycasts = visible;
        }
    }
}
