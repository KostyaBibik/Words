using System;
using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UniRx;
using Zenject;

namespace UI.Gameplay.Validation
{
    public sealed class UIValidationButtonPresenter : UIPresenter<UIValidationButtonView>
    {
        [Inject] private IValidationService _validationService;

        public UIValidationButtonPresenter(UIValidationButtonView view) : base(view)
        {
        }
        
        public override void Initialize()
        {
            _view.OnValidateCommand
                .Subscribe(_ => RunValidation()) 
                .AddTo(_view);
        }

        private IObservable<Unit> RunValidation()
        {
            _validationService.Validate()
                .ToObservable()
                .Subscribe(isValid => 
                {
                    if (!isValid)
                    {
                        OnValidationFailed();
                    }
                })
                .AddTo(_view); 

            return Observable.ReturnUnit();
        }

        private void OnValidationFailed() =>
            _view.ShowErrorNotification();
    }
}