using UI.Abstract;
using UnityEngine;

namespace UI.Gameplay.BottomPanel
{
    public class UIBottomPanelView : UIView
    {
        [SerializeField] private ClusterPanelSettings _clusterPanelSettings;

        public ClusterPanelSettings ClusterPanelSettings => _clusterPanelSettings;
    }
}