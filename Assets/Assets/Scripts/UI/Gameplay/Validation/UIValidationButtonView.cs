using Lean.Gui;
using UI.Abstract;
using UniRx;
using UnityEngine;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonView : UIView
    {
        [SerializeField] private LeanButton _button;
        [SerializeField] private LeanPulse _errorNotification;
        
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

        public void ShowErrorNotification() =>
            _errorNotification.Pulse();
    }
}