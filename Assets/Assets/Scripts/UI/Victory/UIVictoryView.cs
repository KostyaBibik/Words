using Lean.Gui;
using TMPro;
using UI.Abstract;
using UI.Victory.Grid;
using UnityEngine;

namespace UI.Victory
{
    public sealed class UIVictoryView : UIView
    {
        [Header("Title")]
        [SerializeField] private TMP_Text _titleText;

        [Header("Buttons")]
        [SerializeField] private LeanButton _continueBtn;
        [SerializeField] private LeanButton _menuBtn;
        
        [Header("Grid Settings")]
        [SerializeField] private UIFinallyWordView _gridElementPrefab;
        [SerializeField] private Transform _gridTransform;
        
        public LeanButton ContinueBtn => _continueBtn;
        public LeanButton MenuBtn => _menuBtn;
        public UIFinallyWordView ElementPrefab => _gridElementPrefab;
        public Transform GridTransform => _gridTransform;

        public void SetTitle(string text)
        {
            if (_titleText != null)
                _titleText.text = text;
        }
    }
}