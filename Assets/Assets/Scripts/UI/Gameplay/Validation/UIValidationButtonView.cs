using Lean.Gui;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonView : UIView
    {
        [SerializeField] private LeanButton _button;
        
        private readonly AsyncReactiveCommand<Unit> _onValidateCommand = new();
        public AsyncReactiveCommand<Unit> OnValidateCommand => _onValidateCommand;

        protected override void Awake()
        {
            _button.OnClick.AddListener(() =>
            {
                _onValidateCommand.Execute(Unit.Default);
            });
        }

        private void OnDestroy()
        {
            _onValidateCommand.Dispose();
        }
    }
}