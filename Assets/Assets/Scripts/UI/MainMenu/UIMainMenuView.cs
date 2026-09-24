using Lean.Gui;
using TMPro;
using UI.Abstract;
using UnityEngine;
namespace UI.Gameplay
{
    public sealed class UIMainMenuView : UIView
    {
        [SerializeField] private LeanButton _startPlayBtn;
        [SerializeField] private TextMeshProUGUI _progressText;

        public LeanButton StartPlayBtn => _startPlayBtn;
        public TextMeshProUGUI ProgressText => _progressText;
    }
}