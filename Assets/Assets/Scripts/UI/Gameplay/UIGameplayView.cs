using TMPro;
using UI.Abstract;
using UI.Gameplay.ClustersPanel;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIGameplayView : UIView
    {
        [SerializeField] private UIClustersPanelView _clustersPanel;
        [SerializeField] private UIWordGridView _wordGridView;
        [SerializeField] private TMP_Text _categoryText;

        public UIClustersPanelView ClustersPanel => _clustersPanel;
        public UIWordGridView WordGridView => _wordGridView;
        public TMP_Text CategoryText => _categoryText;
    }
}