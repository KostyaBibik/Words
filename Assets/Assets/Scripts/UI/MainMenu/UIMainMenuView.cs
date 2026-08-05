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
        [SerializeField] private TextMeshProUGUI _titleText;

        public LeanButton StartPlayBtn => _startPlayBtn;
        public TextMeshProUGUI ProgressText => _progressText;

        public void SetTitle(string text)
        {
            if (_titleText != null)
                _titleText.text = text;
        }
    }
}