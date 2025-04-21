using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonView : UIView
    {
        [SerializeField] private Button _validateButton;
        
        private readonly AsyncReactiveCommand<Unit> _onValidateCommand = new();
        public AsyncReactiveCommand<Unit> OnValidateCommand => _onValidateCommand;

        protected override void Awake()
        {
            _validateButton.onClick.AddListener(() =>
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