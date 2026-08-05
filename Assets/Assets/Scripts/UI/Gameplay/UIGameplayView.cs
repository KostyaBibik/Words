using Lean.Gui;
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

        [Header("Header")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _progressText;

        [Header("Hint")]
        [SerializeField] private LeanButton _hintBtn;
        [SerializeField] private TMP_Text _hintLabel;

        public UIClustersPanelView ClustersPanel => _clustersPanel;
        public UIWordGridView WordGridView => _wordGridView;
        public TMP_Text CategoryText => _categoryText;
        public LeanButton HintBtn => _hintBtn;

        public void SetLevel(string text)
        {
            if (_levelText != null)
                _levelText.text = text;
        }

        public void SetProgress(string text)
        {
            if (_progressText != null)
                _progressText.text = text;
        }

        public void SetHintLabel(string text)
        {
            if (_hintLabel != null)
                _hintLabel.text = text;
        }
    }
}
