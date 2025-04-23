using UI.Abstract;
using UI.Gameplay.ClustersPanel;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIGameplayView : UIView
    {
        [SerializeField] private UIClustersPanelView _clustersPanel;
        [SerializeField] private UIWordGridView _wordGridView;

        public UIClustersPanelView ClustersPanel => _clustersPanel;
        public UIWordGridView WordGridView => _wordGridView;
    }
}