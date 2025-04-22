using Lean.Gui;
using UI.Abstract;
using UI.Victory.Grid;
using UnityEngine;

namespace UI.Victory
{
    public sealed class UIVictoryView : UIView
    {
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
    }
}