using UI.Gameplay.Elements;
using UnityEngine;

namespace Core.Systems.Placeholder
{
    public sealed class ClusterPlaceholderHandler
    {
        private UIPlaceholderView _placeholder;

        public void Initialize(UIPlaceholderView placeholderView)
        {
            _placeholder = placeholderView;
        }

        public void ActivatePlaceholder(UIClusterElementView cluster, int siblingIndex)
        {
            _placeholder.Activate(cluster.GetComponent<RectTransform>());
            _placeholder.transform.SetSiblingIndex(siblingIndex);
        }

        public void DeactivatePlaceholder()
        {
            _placeholder.Deactivate();
        }
    }
}