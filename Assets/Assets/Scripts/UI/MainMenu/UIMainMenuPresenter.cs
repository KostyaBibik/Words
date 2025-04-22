using System;
using Core.Services;
using Enums;
using UI.Abstract;
using UniRx;
using Zenject;

namespace UI.Gameplay
{
    public sealed class UIMainMenuPresenter : UIPresenter<UIMainMenuView>
    {
        private IAudioService _audioService;
        
        public IObservable<Unit> OnStartPlayBtnClick => _view.StartPlayBtn.OnClickAsObservable();
        
        public UIMainMenuPresenter(UIMainMenuView view) : base(view) 
        {
        }
        
        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public override void Initialize()
        {
            OnStartPlayBtnClick
                .Subscribe(_ => PlayAudioClick()) 
                .AddTo(_view);
        }

        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
    }
}