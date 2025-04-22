using System;
using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UniRx;

namespace UI.Gameplay.Validation
{
    public sealed class UIValidationButtonPresenter : UIPresenter<UIValidationButtonView>
    {
        private readonly IValidationService _validationService;

        public UIValidationButtonPresenter(UIValidationButtonView view, IValidationService validationService)
            : base(view)
        {
            _validationService = validationService;

            _view.OnValidateCommand
                .Subscribe(_ => RunValidationAsync()) 
                .AddTo(_view);
        }
        
        private IObservable<Unit> RunValidationAsync()
        {
            return _validationService.Validate()
                .ToObservable()
                .Select(_ => Unit.Default);
        }
    }
}