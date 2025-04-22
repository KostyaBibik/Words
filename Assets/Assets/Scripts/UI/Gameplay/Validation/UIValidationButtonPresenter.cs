using System;
using Core.Services;
using Cysharp.Threading.Tasks;
using Enums;
using UI.Abstract;
using UniRx;
using Zenject;

namespace UI.Gameplay.Validation
{
    public sealed class UIValidationButtonPresenter : UIPresenter<UIValidationButtonView>
    {
        private IValidationService _validationService;
        private IAudioService _audioService;

        public UIValidationButtonPresenter(UIValidationButtonView view) : base(view)
        {
        }

        [Inject]
        public void Construct(IAudioService audioService, IValidationService validationService)
        {
            _audioService = audioService;
            _validationService = validationService;
        }
        
        public override void Initialize()
        {
            _view.OnValidateCommand
                .Subscribe(_ => RunValidationAsync()) 
                .AddTo(_view);
        }

        private IObservable<Unit> RunValidationAsync()
        {
            PlayAudioClick();
            
            return _validationService.Validate()
                .ToObservable()
                .Select(_ => Unit.Default);
        }
        
        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
    }
}