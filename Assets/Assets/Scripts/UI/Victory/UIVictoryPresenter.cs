using System;
using UI.Abstract;
using UI.Factories;
using UI.Juice;
using UI.Services;
using UI.Victory.Grid;
using UniRx;
using VYandexTools.Localization.Scripts;
using Zenject;

namespace UI.Victory
{
    public sealed class UIVictoryPresenter : UIPresenter<UIVictoryView>
    {
        [Inject] private IWordRepositoryTracker _repositoryTracker;
        [Inject] private IUIWordContainerFactory _wordContainerFactory;

        private const float VICTORY_REVEAL_DELAY = 0.25f;
        private const float VICTORY_REVEAL_STEP = 0.09f;

        private UIFinallyWordPresenter[] _finallyWords;

        public IObservable<Unit> OnMenuBtnClick => _view.MenuBtn.OnClick.AsObservable();
        public IObservable<Unit> OnContinueBtnClick => _view.ContinueBtn.OnClick.AsObservable();
        
        public UIVictoryPresenter(UIVictoryView view) : base(view)
        {
        }

        public override void Initialize() 
        {
            Hide();
        }

        protected override void BeforeShow()
        {
            _view.SetTitle(Loc.Text(LocalizationKey.victory_title, "ПОБЕДА!"));

            var data = _repositoryTracker.GetOrderedWords();

            var gridElementPrefab = _view.ElementPrefab;
            var gridParentLayer = _view.GridTransform;

            _finallyWords = _wordContainerFactory.CreateFinallyWords(gridElementPrefab, gridParentLayer, data);

            PlayCelebration();
        }

        private void PlayCelebration()
        {
            for (var iterator = 0; iterator < _finallyWords.Length; iterator++)
                _finallyWords[iterator].PlayReveal(VICTORY_REVEAL_DELAY + iterator * VICTORY_REVEAL_STEP);

            if (UIVfxLayer.Instance != null)
            {
                // Big opening wave, then it settles into a light, ongoing trickle for as long
                // as the victory screen stays up — stopped in BeforeHide.
                UIVfxLayer.Instance.Rain(160, 340f);
                UIVfxLayer.Instance.StartContinuousRain();
            }
        }

        protected override void BeforeHide()
        {
            if (UIVfxLayer.Instance != null)
                UIVfxLayer.Instance.StopContinuousRain();

            Clear();
        }

        private void Clear()
        {
            if(_finallyWords == null)
                return;

            for (var iterator = 0; iterator < _finallyWords.Length; iterator++)
                _finallyWords[iterator].Destroy();
        }
    }
}