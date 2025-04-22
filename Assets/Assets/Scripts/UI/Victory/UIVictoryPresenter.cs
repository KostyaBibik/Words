using System;
using Core.Factories;
using Core.Systems.WordContainer;
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

        public IObservable<Unit> OnMenuBtnClick => _view.MenuBtn.OnClickAsObservable();
        public IObservable<Unit> OnContinueBtnClick => _view.ContinueBtn.OnClickAsObservable();
        
        public UIVictoryPresenter(UIVictoryView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            WordRepositoryTracker repositoryTracker,
            IUIWordContainerFactory wordContainerFactory
        )
        {
            _repositoryTracker = repositoryTracker;
            _wordContainerFactory = wordContainerFactory;
        }

        public override void Initialize() =>
            Hide();

        protected override void BeforeShow()
        {
            var data = _repositoryTracker.GetOrderedWords();

            var gridElementPrefab = _view.ElementPrefab;
            var gridParentLayer = _view.GridTransform;

            _finallyWords = _wordContainerFactory.CreateFinallyWords(gridElementPrefab, gridParentLayer, data);
        }

        protected override void BeforeHide() => Clear();
        
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