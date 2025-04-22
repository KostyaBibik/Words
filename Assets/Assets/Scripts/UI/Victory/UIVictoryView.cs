using UI.Abstract;
using UI.Victory.Grid;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Victory
{
    public class UIVictoryView : UIView
    {
        [Header("Buttons")]
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _menuBtn;
        
        [Header("Grid Settings")]
        [SerializeField] private UIFinallyWordView _gridElementPrefab;
        [SerializeField] private Transform _gridTransform;
        
        public Button ContinueBtn => _continueBtn;
        public Button MenuBtn => _menuBtn;
        public UIFinallyWordView ElementPrefab => _gridElementPrefab;
        public Transform GridTransform => _gridTransform;
    }
}