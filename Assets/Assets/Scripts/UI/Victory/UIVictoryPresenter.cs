using System;
using Core.Factories;
using Core.Services;
using Core.Systems.WordContainer;
using Enums;
using UI.Abstract;
using UI.Victory.Grid;
using UniRx;
using Zenject;

namespace UI.Victory
{
    public sealed class UIVictoryPresenter : UIPresenter<UIVictoryView>
    {
        private WordRepositoryTracker _repositoryTracker;
        private IUIWordContainerFactory _wordContainerFactory;
        private UIFinallyWordPresenter[] _finallyWords;
        private IAudioService _audioService;

        public IObservable<Unit> OnMenuBtnClick => _view.MenuBtn.OnClick.AsObservable();
        public IObservable<Unit> OnContinueBtnClick => _view.ContinueBtn.OnClick.AsObservable();
        
        public UIVictoryPresenter(UIVictoryView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            WordRepositoryTracker repositoryTracker,
            IUIWordContainerFactory wordContainerFactory,
            IAudioService audioService
        )
        {
            _repositoryTracker = repositoryTracker;
            _wordContainerFactory = wordContainerFactory;
            _audioService = audioService;
        }

        public override void Initialize() 
        {
            Hide();

            AddAudioSubsToButton(OnMenuBtnClick);
            AddAudioSubsToButton(OnContinueBtnClick);
        }

        private void AddAudioSubsToButton(IObservable<Unit> button)
        {
            button
                .Subscribe(_ => PlayAudioClick())
                .AddTo(_view);
        }

        protected override void BeforeShow()
        {
            var data = _repositoryTracker.GetOrderedWords();

            var gridElementPrefab = _view.ElementPrefab;
            var gridParentLayer = _view.GridTransform;

            _finallyWords = _wordContainerFactory.CreateFinallyWords(gridElementPrefab, gridParentLayer, data);
        }

        protected override void BeforeHide() => Clear();
        
        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
        
        private void Clear()
        {
            if(_finallyWords == null)
                return;
            
            for (var iterator = 0; iterator < _finallyWords.Length; iterator++)
            {
                _finallyWords[iterator].Destroy();
            }
        }
    }
}